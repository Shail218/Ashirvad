using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Student;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

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
            return View("Index", Marks);
        }

        [HttpPost]
        public async Task<JsonResult> SaveMarks(MarksEntity Marks)
        {
            var data = new MarksEntity();
            if (Marks.ImageFile != null)
            {
                string _FileName = Path.GetFileName(Marks.ImageFile.FileName);
                string extension = System.IO.Path.GetExtension(Marks.ImageFile.FileName);
                string randomfilename = Common.Common.RandomString(20);
                string _Filepath = "/MarksImage/" + randomfilename + extension;
                string _path = Path.Combine(Server.MapPath("~/MarksImage"), randomfilename + extension);
                Marks.ImageFile.SaveAs(_path);
                Marks.MarksContentFileName = _FileName;
                Marks.MarksFilepath = _Filepath;
            }

            Marks.Transaction = GetTransactionData(Marks.MarksID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            Marks.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            Marks.BranchInfo = new BranchEntity()
            {
                BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID
            };
            Marks.MarksDate = DateTime.Now;
            var line = JsonConvert.DeserializeObject<List<MarksEntity>>(Marks.JsonData);
            foreach(var item in line)
            {
                Marks.AchieveMarks = item.AchieveMarks;
                Marks.student = new StudentEntity(){
                    StudentID = item.student.StudentID
            };
                res = await _MarksService.MarksMaintenance(Marks);
            }
            return Json(res);
        }

        [HttpPost]
        public JsonResult RemoveMarks(long MarksID)
        {
            var result = _MarksService.RemoveMarks(MarksID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

       

        public async Task<ActionResult> GetStudentByStd(long Std, long BatchTime)
        {
            var result = _studentService.GetStudentByStd(Std, SessionContext.Instance.LoginUser.BranchInfo.BranchID, BatchTime).Result;
            return View("~/Views/ResultEntry/Manage.cshtml", result);
        }

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model, long Std, long courseid, long BatchTime)
        {
            List<string> columns = new List<string>();
            var branchData = await _studentService.GetAllCustomStudentMarks(model, Std, courseid,SessionContext.Instance.LoginUser.BranchInfo.BranchID, BatchTime);
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
    }
}