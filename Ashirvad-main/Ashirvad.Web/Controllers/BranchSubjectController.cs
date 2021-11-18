using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Subject;
using Ashirvad.ServiceAPI.ServiceAPI.Area.SuperAdminSubject;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class BranchSubjectController : BaseController
    {
        BranchSubjectMaintenanceModel branchSubject = new BranchSubjectMaintenanceModel();

        ResponseModel response = new ResponseModel();
        private readonly ISuperAdminSubjectService _subjectService;
        private readonly IBranchSubjectService _branchSubjectService;
        public BranchSubjectController(ISuperAdminSubjectService SubjectService, IBranchSubjectService branchSubjectService)
        {
            _subjectService = SubjectService;
            _branchSubjectService = branchSubjectService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> SubjectMaintenance(long SubjectID,long CourseID)
        {
            branchSubject.BranchSubjectInfo = new BranchSubjectEntity();
            branchSubject.BranchSubjectInfo.SubjectData = new List<SuperAdminSubjectEntity>();
            if (SubjectID > 0)
            {
                var result = await _branchSubjectService.GetBranchSubjectByBranchSubjectID(SubjectID, SessionContext.Instance.LoginUser.BranchInfo.BranchID, CourseID);
                if (result.Count > 0)
                {
                    branchSubject.BranchSubjectInfo = result[0].branchSubject;
                }
                branchSubject.BranchSubjectInfo.BranchSubjectData = result;

                branchSubject.BranchSubjectInfo.IsEdit = true;
            }

            var SubjectData = await _subjectService.GetAllSubject();
            branchSubject.BranchSubjectInfo.SubjectData = SubjectData.Data;

            var BranchSubject = await _branchSubjectService.GetAllBranchSubject(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branchSubject.BranchSubjectData = BranchSubject;
            return View("Index", branchSubject);
        }

        [HttpPost]
        public async Task<JsonResult> SaveSubjectDetails(BranchSubjectEntity branchSubject)
        {
            response.Status = false;


            BranchSubjectEntity branchSubjectEntity = new BranchSubjectEntity();

            long CourseDetailID = branchSubject.BranchCourse.course_dtl_id;
            long ClassDetailID = branchSubject.BranchClass.Class_dtl_id;
            var List = JsonConvert.DeserializeObject<List<BranchSubjectEntity>>(branchSubject.JsonData);
            foreach (var item in List)
            {
                branchSubject.branch = new BranchEntity();
                branchSubject.Subject = new SuperAdminSubjectEntity();
                branchSubject.BranchCourse = new BranchCourseEntity();
                branchSubject.BranchClass = new BranchClassEntity();
                branchSubject.branch.BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
                branchSubject.Transaction = GetTransactionData(item.Subject_dtl_id > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
                branchSubject.Subject_dtl_id = item.Subject_dtl_id;
                branchSubject.BranchCourse.course_dtl_id = CourseDetailID;
                branchSubject.Subject.SubjectID = item.Subject.SubjectID;
                branchSubject.Subject.SubjectName = item.Subject.SubjectName;
                branchSubject.isSubject = item.isSubject;
                branchSubject.BranchClass.Class_dtl_id = ClassDetailID;
                branchSubject = await _branchSubjectService.BranchSubjectMaintenance(branchSubject);
                if ((long)branchSubject.Data < 0)
                {
                    break;
                }
            }
            if ((long)branchSubject.Data > 0)
            {
                response.Status = true;
                response.Message = branchSubject.Subject_dtl_id > 0 ? "Updated Successfully!!" : "Created Successfully!!";


            }
            else if ((long)branchSubject.Data < 0)
            {
                response.Status = false;
                response.Message = "Already Exists!!";
            }
            else
            {
                response.Status = false;
                response.Message = branchSubject.Subject_dtl_id > 0 ? "Failed To Update!!" : "Failed To Create!!";
            }
            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> RemoveSubjectDetail(long CourseID,long ClassID)
        {
            response.Status = false;
            response.Message = "Failed To Delete";
            var result = _branchSubjectService.RemoveBranchSubject(CourseID, ClassID, SessionContext.Instance.LoginUser.BranchInfo.BranchID, SessionContext.Instance.LoginUser.Username);
            if (result)
            {
                response.Status = true;
                response.Message = "Successfully Deleted!!";
            }

            return Json(response);
        }
    }
}