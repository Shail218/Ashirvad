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
            return View();
        }

        public async Task<ActionResult> AgreementMaintenance(long agreeID)
        {
            AgreementMaintenanceModel agreement = new AgreementMaintenanceModel();
            if (agreeID > 0)
            {
                var result = await _branchService.GetAgreementByAgreementID(agreeID);
                agreement.AgreementInfo = result.Data;
            }

            var branchData = await _branchService.GetAllAgreement(0);
            agreement.AgreementData = branchData.Data;

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
    }
}