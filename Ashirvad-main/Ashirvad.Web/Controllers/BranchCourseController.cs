using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
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
                branchCourse.RowStatus.RowStatus = Enums.RowStatus.Active;
                branchCourse = await _branchcourseService.BranchCourseMaintenance(branchCourse);
                if ((long)branchCourse.Data < 0)
                {
                    break;
                }
            }
            if ((long)branchCourse.Data > 0)
            {
                response.Status = true;
                response.Message = branchCourse.course_dtl_id > 0 ? "Updated Successfully!!" : "Created Successfully!!";


            }
            else if ((long)branchCourse.Data < 0)
            {
                response.Status = false;
                response.Message = "Already Exists!!";
            }
            else
            {
                response.Status = false;
                response.Message = branchCourse.course_dtl_id > 0 ? "Failed To Update!!" : "Failed To Create!!";
            }
            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> RemoveCourseDetail(long PackageRightID)
        {
            response.Status = false;
            response.Message = "Failed To Delete";
            var result = _branchcourseService.RemoveBranchCourse(PackageRightID, SessionContext.Instance.LoginUser.Username);
            if (result)
            {
                response.Status = true;
                response.Message = "Successfully Deleted!!";
            }

            return Json(response);
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
            catch(Exception ex)
            {
                return Json(null);
            }
            
        }
    }
}