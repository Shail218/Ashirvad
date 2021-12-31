using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.UPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class UpiController : BaseController
    {
        private readonly IUPIService _upiService;
        public ResponseModel res = new ResponseModel();

        public UpiController(IUPIService upiService)
        {
            _upiService = upiService;
        }
        // GET: Upi
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> UPIMaintenance(long upiID)
        {
            UPIModel branch = new UPIModel();
            if (upiID > 0)
            {
                var result = await _upiService.GetUPIByUPIID(upiID);
                branch.UPIInfo = result.Data;
            }

            var branchData = await _upiService.GetAllUPIs(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branch.UPIData = branchData.Data;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveUPI(UPIEntity upi)
        {
            upi.TransactionData = GetTransactionData(upi.UPIId > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            upi.RowStatusData = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            upi.BranchData = new BranchEntity()
            {
                BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID
            };
            var data = await _upiService.UPIMaintenance(upi);
            if(data != null)
            {
                return Json(data);
            }
            return Json(0);
        }

        [HttpPost]
        public JsonResult RemoveUPI(long upiID)
        {
            var result = _upiService.RemoveUPI(upiID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }
    }
}