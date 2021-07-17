using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class StudentController : BaseController
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> StudentMaintenance(long studentID)
        {
            StudentMaintenanceModel branch = new StudentMaintenanceModel();
            if (studentID > 0)
            {
                var result = await _studentService.GetStudentByID(studentID);
                branch.StudentInfo = result;
            }

            return View("Index", branch);
        }


        [HttpPost]
        public async Task<JsonResult> SaveStudent(StudentEntity branch)
        {
            branch.GrNo = "Gr 1";
            branch.Transaction = GetTransactionData(branch.StudentID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            var data = await _studentService.StudentMaintenance(branch);
            if (data != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult RemoveStudent(long studentID)
        {
            var result = _studentService.RemoveStudent(studentID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

    }
}