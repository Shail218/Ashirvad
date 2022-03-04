using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Repo.Services.Area;
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
using static Ashirvad.Common.Common;

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
                var result = await _branchSubjectService.GetSubjectByclasscourseid(SubjectID, SessionContext.Instance.LoginUser.BranchInfo.BranchID, CourseID);
                if (result.Count > 0)
                {
                    branchSubject.BranchSubjectInfo = result[0].branchSubject;
                    branchSubject.BranchSubjectInfo.Subject_dtl_id = result[0].Subject_dtl_id;
                }
                branchSubject.BranchSubjectInfo.BranchSubjectData = result;

                branchSubject.BranchSubjectInfo.IsEdit = true;
            }

            //var SubjectData = await _subjectService.GetAllSubject();
            //branchSubject.BranchSubjectInfo.SubjectData = SubjectData.Data;

            //var BranchSubject = await _branchSubjectService.GetAllBranchSubject(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branchSubject.BranchSubjectData = new List<BranchSubjectEntity>();
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
                branchSubject.UserType = SessionContext.Instance.LoginUser.UserType;
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
            var result = _branchSubjectService.RemoveBranchSubject(CourseID, ClassID, SessionContext.Instance.LoginUser.BranchInfo.BranchID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> GetSubjectDDL(long ClassID,long CourseID)
        {

            try
            {
                var result = await _branchSubjectService.GetBranchSubjectByBranchSubjectID(ClassID, SessionContext.Instance.LoginUser.BranchInfo.BranchID, CourseID);
                if (result.Count > 0)
                {
                    return Json(result);
                }
                else
                {
                    return Json(null);
                }

            }
            catch (Exception ex)
            {
                return Json(null);
            }

        }

        public async Task<JsonResult> GetLibrarySubjectDDL(long courseid, string std)
        {

            try
            {
                var result = await _branchSubjectService.GetSubjectDDL(courseid, SessionContext.Instance.LoginUser.BranchInfo.BranchID, std);
                if (result.Count > 0)
                {
                    return Json(result);
                }
                else
                {
                    return Json(null);
                }

            }
            catch (Exception ex)
            {
                return Json(null);
            }

        }

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            var branchData = await _branchSubjectService.GetAllSubjects(model, SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            long total = 0;
            if (branchData.Count > 0)
            {
                total = branchData[0].Count;
            }
            return Json(new
            {
                draw = model.draw,
                iTotalRecords = total,
                iTotalDisplayRecords = total,
                data = branchData
            });

        }

        public ActionResult GetAllSubjectByCourse(long courseid,long classid,long subjecdetailID=0)
        {
            var data = _subjectService.GetAllSubjectByCourseClassddl(courseid,classid);
            return View("~/Views/BranchSubject/UpdateSubjectDataTable.cshtml", data.Result);
        }

        [HttpPost]
        public async Task<JsonResult> Check_SubjectDetail(long subjectdetailid = 0)
        {
            Check_Delete subject = new Check_Delete();
            response.Status = true;
            response.Message = "";
            if (subjectdetailid > 0)
            {
                var result = subject.check_delete_subject(SessionContext.Instance.LoginUser.BranchInfo.BranchID, subjectdetailid).Result;
                return Json(result);
            }
            else
            {
                return Json(response);
            }

        }

        [HttpPost]
        public async Task<ActionResult> GetExportData(string Search)
        {
            var branchData = await _branchSubjectService.GetAllSelectedSubjects(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            return View("~/Views/BranchSubject/_Export_BranchSubject.cshtml", branchData);

        }
    }
}