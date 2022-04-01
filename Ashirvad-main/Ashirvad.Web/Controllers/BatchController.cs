using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Batch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

namespace Ashirvad.Web.Controllers
{
    public class BatchController : BaseController
    {
        private readonly IBatchService _batchService;
        public ResponseModel res = new ResponseModel();

        public BatchController(IBatchService batchService)
        {
            _batchService = batchService;
        }

        // GET: Batch
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> BatchMaintenance(long branchID)
        {
            long batchID = branchID;
            BatchMaintenanceModel branch = new BatchMaintenanceModel();
            if (batchID > 0)
            {
                var result = await _batchService.GetBatchByID(batchID);
                branch.BatchInfo = result;
            }

            //var batchData = await _batchService.GetAllBatches(SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branch.BatchData = new List<BatchEntity>();

            return View("Index", branch);
        }

        public async Task<ActionResult> EditBatch(long batchID, long branchID)
        {
            BatchMaintenanceModel branch = new BatchMaintenanceModel();
            if (batchID > 0)
            {
                var result = await _batchService.GetBatchByID(batchID);
                branch.BatchInfo = result;
            }

            if (branchID > 0)
            {
                var result = await _batchService.GetAllBatches(branchID);
                branch.BatchData = result;
            }

            var batchData = await _batchService.GetAllBatches();
            branch.BatchData = batchData;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveBatch(BatchEntity branch)
        {
            branch.Transaction = GetTransactionData(branch.BatchID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            branch.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _batchService.BatchMaintenance(branch);
            res.Status = data.Status;
            res.Message = data.Message;
            return Json(res);
        }

        [HttpPost]
        public JsonResult RemoveBatch(long batchID)
        {
            var result = _batchService.RemoveBatch(batchID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            List<string> columns = new List<string>();
            columns.Add("BatchText");
            foreach (var item in model.order)
            {
                item.name = columns[item.column];
            }
            var branchData = await _batchService.GetAllCustomBatch(model, SessionContext.Instance.LoginUser.BranchInfo.BranchID);
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

        [HttpPost]
        public async Task<ActionResult> GetExportData(string Search)
        {
            var branchData = await _batchService.GetAllBatches(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            return View("~/Views/Batch/_Export_Batch.cshtml", branchData);
        }
    }
}