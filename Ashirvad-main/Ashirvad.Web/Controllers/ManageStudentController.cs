using Ashirvad.Common;
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
    public class ManageStudentController : Controller
    {
        private readonly IStudentService _studentService;
        public ManageStudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: ManageStudent
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ManageStudentMaintenance(long branchID)
        {
            StudentMaintenanceModel branch = new StudentMaintenanceModel();
            if (branchID > 0)
            {
                var result = await _studentService.GetAllStudent(branchID);
                branch.StudentData = result;
            }

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<ActionResult> GetAllActiveStudent(long branchID)
        {
            var data = await this._studentService.GetAllStudent(branchID, (int)Enums.RowStatus.Active);
            return View("~/Views/ManageStudent/Manage.cshtml", data);
        }

        [HttpPost]
        public async Task<ActionResult> GetAllInActiveStudent(long branchID)
        {
            var data = await this._studentService.GetAllStudent(branchID, (int)Enums.RowStatus.Inactive);
            return View("~/Views/ManageStudent/Manage.cshtml", data);
        }

        [HttpPost]
        public JsonResult Removestudent(long studentID)
        {
            var result = _studentService.RemoveStudent(studentID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }
    }
}