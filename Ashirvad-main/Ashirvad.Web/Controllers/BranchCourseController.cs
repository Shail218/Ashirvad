using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Repo.Services.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Course;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class BranchCourseController : BaseController
    {
        BranchCourseMaintenanceModel branchCourse = new BranchCourseMaintenanceModel();

        ResponseModel response = new ResponseModel();
        private readonly ICourseService _courseService;
        private readonly IBranchCourseService _branchcourseService;
        public BranchCourseController(ICourseService courseService, IBranchCourseService branchCourseService)
        {
            _courseService = courseService;
            _branchcourseService = branchCourseService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> CourseMaintenance(long courseID)
        {
            branchCourse.BranchCourseInfo = new BranchCourseEntity();
            branchCourse.BranchCourseInfo.CourseData = new List<CourseEntity>();
            if (courseID > 0)
            {
                var result = await _branchcourseService.GetBranchCourseByBranchCourseID(courseID);
                branchCourse.BranchCourseInfo.BranchCourseData = result;
                branchCourse.BranchCourseInfo.IsEdit = true;
            }

            var courseData = await _courseService.GetAllCourse();
            branchCourse.BranchCourseInfo.CourseData = courseData.Data;

            var BranchCourse = await _branchcourseService.GetAllBranchCourse(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branchCourse.BranchCourseData = BranchCourse;
            return View("Index", branchCourse);
        }

        [HttpPost]
        public async Task<JsonResult> SaveCourseDetails(BranchCourseEntity branchCourse)
        {
            response.Status = false;


            BranchCourseEntity branchCourseEntity = new BranchCourseEntity();



            var List = JsonConvert.DeserializeObject<List<BranchCourseEntity>>(branchCourse.JsonData);
            foreach (var item in List)
            {
                branchCourse.branch = new BranchEntity();
                branchCourse.course = new CourseEntity();
                branchCourse.branch.BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
                branchCourse.Transaction = GetTransactionData(item.course_dtl_id > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
                branchCourse.course_dtl_id = item.course_dtl_id;
                branchCourse.course.CourseID = item.course.CourseID;
                branchCourse.iscourse = item.iscourse;
                branchCourse.RowStatus = new RowStatusEntity()
                {
                    RowStatus = Enums.RowStatus.Active
                };
                response = await _branchcourseService.BranchCourseMaintenance(branchCourse);
                if (!response.Status)
                {
                    break;
                }
            }
           
            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> RemoveCourseDetail(long PackageRightID)
        {
            response.Status = true;
            response.Message = "";
            var result = _branchcourseService.RemoveBranchCourse(PackageRightID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> GetCourseDDL()
        {
            try
            {
                var BranchCourse = await _branchcourseService.GetAllBranchCourse(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
                branchCourse.BranchCourseData = BranchCourse;

                if (branchCourse.BranchCourseData.Count > 0)
                {
                    return Json(branchCourse.BranchCourseData);
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
        [HttpPost]
        public async Task<JsonResult> Check_CourseDetail(long coursedetailid=0)
        {
            Check_Delete course = new Check_Delete();
            response.Status = true;
            response.Message = "";
            if(coursedetailid > 0)
            {
                var result = course.check_delete_course(SessionContext.Instance.LoginUser.BranchInfo.BranchID, coursedetailid).Result;
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
            var branchData = await _branchcourseService.GetAllBranchCourseforExport(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            return View("~/Views/BranchCourse/_Export_BranchCourse.cshtml", branchData);

        }
    }
}