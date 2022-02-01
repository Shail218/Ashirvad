using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

namespace Ashirvad.Web.Controllers
{
    public class ResultRegisterController : BaseController
    {
        private readonly IMarksService _MarksService;
        ResponseModel response = new ResponseModel();

        public ResultRegisterController(IMarksService marksService)
        {
            _MarksService = marksService;
        }
        // GET: ResultRegister
        public ActionResult Index()
        {
            MarksMaintenanceModel model = new MarksMaintenanceModel();
            model.MarksInfo = new MarksEntity();
            model.MarksData = new List<MarksEntity>();
            return View(model);
        }

        public async Task<ActionResult> GetAllAchieveMarks(long Std, long Batch, long MarksID)
        {
            var result = _MarksService.GetAllAchieveMarks(Std,SessionContext.Instance.LoginUser.BranchInfo.BranchID,Batch,MarksID).Result;
            return View("~/Views/ResultRegister/Manage.cshtml", result);
        }

        public async Task<JsonResult> UpdateMarksDetails(long MarksID, long StudentID, string AchieveMarks)
        {
            MarksEntity marks = new MarksEntity();
            marks.testEntityInfo = new TestEntity();
            marks.student = new StudentEntity();
            marks.MarksID = MarksID;
            marks.student.StudentID = StudentID;
            marks.AchieveMarks = AchieveMarks;
            marks.Transaction = GetTransactionData(Common.Enums.TransactionType.Insert);
            var result1 = _MarksService.UpdateMarksDetails(marks).Result;
            if (result1.MarksID > 0)
            {
                response.Status = true;
                response.Message = "Marks Updated Successfully!!";
            }
            else
            {
                response.Status = false;
                response.Message = "Marks Failed To Updated!!";
            }
            return Json(response);
        }

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model, long Std, long courseid, long Batch, long MarksID)
        {
            List<string> columns = new List<string>();
            columns.Add("AchieveMarks");
            if (model.order != null)
            {
                foreach (var item in model.order)
                {
                    item.name = columns[item.column];
                }
            }
            var branchData = await _MarksService.GetAllCustomMarks(model, Std,courseid, SessionContext.Instance.LoginUser.BranchInfo.BranchID, Batch, MarksID);
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