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

            var paperData = await _paperService.GetAllPaperWithoutContent();
            branch.PaperData = paperData.Data;

            return View("Index", branch);
        }


        [HttpPost]
        public async Task<JsonResult> SavePaper(PaperEntity paperEntity)
        {
            paperEntity.Transaction = GetTransactionData(paperEntity.PaperID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            //if (branch.PaperData.PaperFile != null)
            //{
            //    branch.PaperData = new PaperData();
            //    branch.PaperData.PaperContent = Common.Common.ReadFully(branch.PaperData.PaperFile.InputStream);
            //    branch.PaperData.PaperPath = branch.PaperData.PaperFile.FileName;
            //}
            paperEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _paperService.PaperMaintenance(paperEntity);
            if (data != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult RemovePaper(long paperID)
        {
            var result = _paperService.RemovePaper(paperID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

    }
}