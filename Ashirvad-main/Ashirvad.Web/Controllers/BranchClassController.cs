using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Repo.Services.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Class;
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
    public class BranchClassController : BaseController
    {

        BranchClassMaintenanceModel branchClass = new BranchClassMaintenanceModel();

        ResponseModel response = new ResponseModel();
        private readonly IClassService _ClassService;
        private readonly IBranchClassService _branchClassService;
        public BranchClassController(IClassService ClassService, IBranchClassService branchClassService)
        {
            _ClassService = ClassService;
            _branchClassService = branchClassService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> ClassMaintenance(long ClassID)
        {
            branchClass.BranchClassInfo = new BranchClassEntity();
            branchClass.BranchClassInfo.ClassData = new List<ClassEntity>();
            if (ClassID > 0)
            {
                var result = await _branchClassService.GetBranchClassByBranchClassID(ClassID, SessionContext.Instance.LoginUser.BranchInfo.BranchID);
                if (result.Count > 0)
                {
                    branchClass.BranchClassInfo = result[0].branchClass;
                    branchClass.BranchClassInfo.Class_dtl_id = result[0].Class_dtl_id;
                }
                branchClass.BranchClassInfo.BranchClassData = result;

                branchClass.BranchClassInfo.IsEdit = true;
            }

            //var ClassData = await _ClassService.GetAllClass();
            //branchClass.BranchClassInfo.ClassData = ClassData.Data;

            //var BranchClass = await _branchClassService.GetAllBranchClass(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branchClass.BranchClassData = new List<BranchClassEntity>();
            //branchClass.BranchClassData = BranchClass;
            return View("Index", branchClass);
        }

        [HttpPost]
        public async Task<JsonResult> SaveClassDetails(BranchClassEntity branchClass)
        {
            response.Status = false;


            BranchClassEntity branchClassEntity = new BranchClassEntity();

            long CourseDetailID = branchClass.BranchCourse.course_dtl_id;
            ResponseModel model = new ResponseModel();
            var List = JsonConvert.DeserializeObject<List<BranchClassEntity>>(branchClass.JsonData);
            foreach (var item in List)
            {
                branchClass.branch = new BranchEntity();
                branchClass.Class = new ClassEntity();
                branchClass.BranchCourse = new BranchCourseEntity();
                branchClass.branch.BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
                branchClass.Transaction = GetTransactionData(item.Class_dtl_id > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
                branchClass.Class_dtl_id = item.Class_dtl_id;
                branchClass.BranchCourse.course_dtl_id = CourseDetailID;
                branchClass.Class.ClassID = item.Class.ClassID;
                branchClass.Class.ClassName = item.Class.ClassName;
                branchClass.isClass = item.isClass;
                branchClass.UserType = SessionContext.Instance.LoginUser.UserType;
                model = await _branchClassService.BranchClassMaintenance(branchClass);
                if (!model.Status)
                {
                    break;
                }
            }
            return Json(model);
        }

        [HttpPost]
        public async Task<JsonResult> RemoveClassDetail(long PackageRightID)
        {
            var result = _branchClassService.RemoveBranchClass(PackageRightID, SessionContext.Instance.LoginUser.BranchInfo.BranchID, SessionContext.Instance.LoginUser.Username);           
            return Json(result);
        }

        public async Task<JsonResult> GetClassDDL(long CourseID)
        {

            try
            {
                var BranchCourse = await _branchClassService.GetAllBranchClassDDL(SessionContext.Instance.LoginUser.BranchInfo.BranchID, CourseID);
                if (BranchCourse.Count > 0)
                {
                    return Json(BranchCourse);
                }
                else
                {
                    return Json(0);
                }

            }
            catch (Exception ex)
            {
                return Json(null);
            }

        }

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            var branchData = await _branchClassService.GetAllBranchClass(model,SessionContext.Instance.LoginUser.BranchInfo.BranchID);
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
        public ActionResult GetAllClassByCourse(long courseid,long classdetailID=0)
        {
            bool IsUpdate = classdetailID>0;            
            var data = _ClassService.GetAllClassByCourseddl(courseid, IsUpdate);
            return View("~/Views/BranchClass/UpdateClassDataTable.cshtml", data.Result);
        }

        [HttpPost]
        public async Task<JsonResult> Check_ClassDetail(long classdetailid = 0)
        {
            Check_Delete cls = new Check_Delete();
            response.Status = true;
            response.Message = "";
            if (classdetailid > 0)
            {
                var result = cls.check_delete_class(SessionContext.Instance.LoginUser.BranchInfo.BranchID, classdetailid).Result;
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
            var branchData = await _branchClassService.GetAllSelectedClasses(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            return View("~/Views/BranchClass/_Export_BranchClass.cshtml", branchData);

        }
    }
}