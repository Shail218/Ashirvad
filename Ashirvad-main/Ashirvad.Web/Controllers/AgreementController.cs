using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Branch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

namespace Ashirvad.Web.Controllers
{
    public class AgreementController : BaseController
    {
        private readonly IBranchService _branchService;
        public ResponseModel res = new ResponseModel();

        public AgreementController(IBranchService branchService)
        {
            _branchService = branchService;
        }
        // GET: Agreement
        public ActionResult Index()
        {
            return View(new AgreementMaintenanceModel());
        }

        public async Task<ActionResult> AgreementMaintenance(long agreeID)
        {
            AgreementMaintenanceModel agreement = new AgreementMaintenanceModel();
            if (agreeID > 0)
            {
                var result = await _branchService.GetAgreementByAgreementID(agreeID);
                agreement.AgreementInfo = result.Data;
            }
            agreement.AgreementData = new List<BranchAgreementEntity>();
            return View("Index", agreement);
        }

        [HttpPost]
        public async Task<JsonResult> SaveAgreement(BranchAgreementEntity branch)
        {
            branch.TranscationData = GetTransactionData(branch.AgreementID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            var data = await _branchService.AgreementMaintenance(branch);
            if(data != null)
            {
                return Json(data);
            }
            return Json(0);
        }

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            List<string> columns = new List<string>();
           // columns.Add("SerialKey");
            columns.Add("AgreementFromDate");
            columns.Add("AgreementToDate");
            foreach (var item in model.order)
            {
                item.name = columns[item.column];
            }
            var branchData = await _branchService.GetAllCustomAgreement(model, 0);
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