using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Course;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class CaurseController : BaseController
    {
        private readonly ICourseService _courseService;
        public CaurseController(ICourseService courseservice)
        {
            _courseService = courseservice;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> CourseMaintenance(long courseID)
        {
            CourseMaintenanceModel course = new CourseMaintenanceModel();
            if (courseID > 0)
            {
                var result = await _courseService.GetCourseByCourseID(courseID);
                course.CourseInfo = result.Data;
            }

            var courseData = await _courseService.GetAllCourse();
            course.CourseData = courseData.Data;

            return View("Index", course);
        }

        [HttpPost]
        public async Task<JsonResult> SaveCourse(CourseEntity course)
        {
            if (course.ImageFile != null)
            {
                string _FileName = Path.GetFileName(course.ImageFile.FileName);
                string extension = System.IO.Path.GetExtension(course.ImageFile.FileName);
                string randomfilename = Common.Common.RandomString(20);
                string _Filepath = "/CourseImage/" + randomfilename + extension;
                string _path = Path.Combine(Server.MapPath("~/CourseImage"), randomfilename + extension);
                course.ImageFile.SaveAs(_path);
                course.filename = _FileName;
                course.filepath = _Filepath;
            }
            course.Transaction = GetTransactionData(course.CourseID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            course.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _courseService.CourseMaintenance(course);
            if (data != null)
            {
                return Json(data);
            }

            return Json(0);
        }

        [HttpPost]
        public JsonResult RemoveCourse(long courseID)
        {
            var result = _courseService.RemoveCourse(courseID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> CourseData()
        {
            var branchData = await _courseService.GetAllCourse();
            return Json(branchData.Data);
        }
    }
}