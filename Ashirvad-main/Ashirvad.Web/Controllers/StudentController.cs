using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Student;
using System;
using System.Collections.Generic;
using System.IO;
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
        public async Task<JsonResult> getcount()
        {
            var getstudentno = await _studentService.CheckPackage(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            return Json(getstudentno);
        }

        [HttpPost]
        public async Task<JsonResult> SaveStudent(StudentEntity branch)
        {

            StudentEntity data = new StudentEntity();
            branch.GrNo = "Gr 1";

            if (branch.ImageFile != null)
            {
                string _FileName = Path.GetFileName(branch.ImageFile.FileName);
                string extension = System.IO.Path.GetExtension(branch.ImageFile.FileName);
                string randomfilename = Common.Common.RandomString(20);
                string _Filepath = "/StudentImage/" + randomfilename + extension;
                string _path = Path.Combine(Server.MapPath("~/StudentImage"), randomfilename + extension);
                branch.ImageFile.SaveAs(_path);
                branch.FileName = _FileName;
                branch.FilePath = _Filepath;
            }
            branch.Transaction = GetTransactionData(branch.StudentID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            if (branch.StudentID == 0)
            {
                branch.RowStatus = new RowStatusEntity()
                {
                    RowStatusId = (int)Enums.RowStatus.Active
                };
            }
            if (branch.LastYearResult == null)
            {
                branch.LastYearResult = 1;
            }
            data = await _studentService.StudentMaintenance(branch);
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

        public ActionResult StudentTransfer()
        {
            return View();
        }

    }
}