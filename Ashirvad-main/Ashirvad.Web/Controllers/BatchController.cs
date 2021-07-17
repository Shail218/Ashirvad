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

namespace Ashirvad.Web.Controllers
{
    public class BatchController : BaseController
    {
        private readonly IBatchService _batchService;
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

            var batchData = await _batchService.GetAllBatches(SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branch.BatchData = batchData;

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
            if (data != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult RemoveBatch(long batchID)
        {
            var result = _batchService.RemoveBatch(batchID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }
    }
}