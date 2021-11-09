using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
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
    public class ResultEntryController : BaseController
    {
        // GET: ResultEntry
        private readonly IMarksService _MarksService;
        private readonly IStudentService _studentService;
        public ResponseModel res = new ResponseModel();

        public ResultEntryController(IMarksService marksService, IStudentService studentService)
        {
            _MarksService = marksService;
            _studentService = studentService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> MarksMaintenance(long MarksID)
        {
            MarksMaintenanceModel Marks = new MarksMaintenanceModel();
            if (MarksID > 0)
            {
                var result = await _MarksService.GetMarksByMarksID(MarksID);
                Marks.MarksInfo = result;
            }
            Marks.StudentData = new List<StudentEntity>();
            //var MarksData = await _MarksService.GetAllMarks();
            //Marks.MarksData = MarksData;

            return View("Index", Marks);
        }

        //[HttpPost]
        //public async Task<string> GetMarksLogo(long MarksID)
        //{
        //    var data = await _MarksService.GetMarksByMarksID(MarksID);
        //    var result = data.Data;
        //    return "data:image/jpg;base64, " + Convert.ToBase64String(result.MarksMaint.MarksLogo, 0, result.MarksMaint.MarksLogo.Length);
        //}

        [HttpPost]
        public async Task<JsonResult> SaveMarks(MarksEntity Marks)
        {
            if (Marks.FileInfo != null)
            {

                Marks.MarksContent = Common.Common.ReadFully(Marks.FileInfo.InputStream);
                string _FileName = Path.GetFileName(Marks.FileInfo.FileName);
                string extension = System.IO.Path.GetExtension(Marks.FileInfo.FileName);
                string randomfilename = Common.Common.RandomString(20);
                string _Filepath = "/MarksImage/" + randomfilename + extension;
                string _path = Path.Combine(Server.MapPath("~/MarksImage"), randomfilename + extension);
                Marks.FileInfo.SaveAs(_path);
                Marks.MarksContentFileName = _FileName;
                Marks.MarksFilepath = _Filepath;
            }

            Marks.Transaction = GetTransactionData(Marks.MarksID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            Marks.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _MarksService.MarksMaintenance(Marks);
            if (data != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult RemoveMarks(long MarksID)
        {
            var result = _MarksService.RemoveMarks(MarksID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> MarksData()
        {
            var MarksData = await _MarksService.GetAllMarksWithoutImage();

            return Json(MarksData);
        }

        public async Task<ActionResult> GetStudentByStd(long Std, long BatchTime)
        {
            var result = _studentService.GetStudentByStd(Std, SessionContext.Instance.LoginUser.BranchInfo.BranchID, BatchTime).Result;
            return View("~/Views/ResultEntry/Manage.cshtml", result);
        }
    }
}