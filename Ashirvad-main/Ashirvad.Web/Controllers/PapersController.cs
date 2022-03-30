using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Paper;
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
    public class PapersController : BaseController
    {
        private readonly IPaperService _paperService;
        public PapersController(IPaperService paperService)
        {
            _paperService = paperService;
        }
        // GET: Papers
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> PaperMaintenance(long paperID)
        {
            PaperMaintenanceModel branch = new PaperMaintenanceModel();
            if (paperID > 0)
            {
                var result = await _paperService.GetPaperByPaperID(paperID);
                branch.PaperInfo = result.Data;
            }
            //var paperData = await _paperService.GetAllPaperWithoutContent(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branch.PaperData = new List<PaperEntity>();
            return View("Index", branch);
        }


        [HttpPost]
        public async Task<JsonResult> SavePaper(PaperEntity paperEntity)
        {
            if (paperEntity.PaperData.PaperFile != null)
            {
                string _FileName = Path.GetFileName(paperEntity.PaperData.PaperFile.FileName);
                string extension = System.IO.Path.GetExtension(paperEntity.PaperData.PaperFile.FileName);
                string randomfilename = Common.Common.RandomString(20);
                string _Filepath = "/PaperDocument/" + randomfilename + extension;
                string _path = Path.Combine(Server.MapPath("~/PaperDocument"), randomfilename + extension);
                paperEntity.PaperData.PaperFile.SaveAs(_path);
                paperEntity.PaperData.PaperPath = _FileName;
                paperEntity.PaperData.FilePath = _Filepath;
            }
            paperEntity.Transaction = GetTransactionData(paperEntity.PaperID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            paperEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _paperService.PaperMaintenance(paperEntity);
            

            return Json(data);
        }

        [HttpPost]
        public JsonResult RemovePaper(long paperID)
        {
            var result = _paperService.RemovePaper(paperID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            List<string> columns = new List<string>();
            columns.Add("Remark");
            foreach (var item in model.order)
            {
                item.name = columns[item.column];
            }
            var branchData = await _paperService.GetAllCustomPaper(model, SessionContext.Instance.LoginUser.BranchInfo.BranchID);
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