using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Class;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class StandardController : BaseController
    {
        private readonly IStandardService _standardService;
       
        public ResponseModel res = new ResponseModel();

        public StandardController(IStandardService standardService)
        {
            _standardService = standardService;
        }

        // GET: Standard
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> StandardMaintenance(long branchID)
        {
            long standardID = branchID;
            StandardMaintenanceModel branch = new StandardMaintenanceModel();
            if (standardID > 0)
            {
                var result = await _standardService.GetStandardsByID(standardID);
                branch.StandardInfo = result;
            }

            var branchData = await _standardService.GetAllStandards(SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branch.StandardData = branchData;

            return View("Index", branch);
        }

        public async Task<ActionResult> EditStandard(long standardID, long branchID)
        {
            StandardMaintenanceModel branch = new StandardMaintenanceModel();
            if (standardID > 0)
            {
                var result = await _standardService.GetStandardsByID(standardID);
                branch.StandardInfo = result;
            }

            if (branchID > 0)
            {
                var result = await _standardService.GetAllStandards(branchID);
                branch.StandardData = result;
            }

            //var branchData = await _standardService.GetAllStandards();
            //branch.StandardData = branchData;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveStandard(StandardEntity branch)
        {
            branch.Transaction = GetTransactionData(branch.StandardID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            branch.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _standardService.StandardMaintenance(branch);
            res.Status = data.StandardID > 0 ? true : false;
            res.Message = data.StandardID == -1 ? "Standard Already exists!!" : data.StandardID == 0 ? "Standard failed to insert!!" : "Standard Inserted Successfully!!";
            return Json(res);

        }

        [HttpPost]
        public JsonResult RemoveStandard(long standardID)
        {
            var result = _standardService.RemoveStandard(standardID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> StandardData(long branchID)
        {
            if(SessionContext.Instance.LoginUser.UserType == Ashirvad.Common.Enums.UserType.SuperAdmin)
            {
                var branchData = await _standardService.GetAllStandardsName(0);
                return Json(branchData);
            }
            else
            {
                var branchData = await _standardService.GetAllStandards(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
                return Json(branchData);
            }           
        }
    }
}