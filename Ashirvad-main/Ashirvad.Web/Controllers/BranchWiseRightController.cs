using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Page;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class BranchWiseRightController : BaseController
    {
        // GET: BranchWiseRight
       

        private readonly IBranchRightsService _BranchRightService;
        private readonly IPageService _pageService;


        public BranchWiseRightController(IBranchRightsService BranchRightService, IPageService pageService)
        {

            _BranchRightService = BranchRightService;
            _pageService = pageService;
        }
        // GET: BranchRight
        public ActionResult Index()
        {
            
            return View();
        }

        public async Task<ActionResult> BranchRightMaintenance(long BranchRightID)
        {
            BranchWiseRightMaintenanceModel BranchRight = new BranchWiseRightMaintenanceModel();
            BranchRight.BranchWiseRightInfo = new BranchWiseRightEntity();
            if (BranchRightID > 0)
            {
                var result = await _BranchRightService.GetBranchRightsByID(BranchRightID);
                BranchRight.BranchWiseRightInfo = result;
            }

            var BranchRightData = await _BranchRightService.GetAllBranchRightss();
            BranchRight.BranchWiseRightData = BranchRightData;
            

            var branchData = await _pageService.GetAllPages(SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            BranchRight.BranchWiseRightInfo.PageList = branchData;
            return View("Index", BranchRight);
        }

        [HttpPost]
        public async Task<JsonResult> SaveBranchRight(BranchWiseRightEntity BranchRight)
        {

            BranchWiseRightEntity BranchRightEntity = new BranchWiseRightEntity();
            BranchRight.Transaction = GetTransactionData(BranchRight.BranchWiseRightsID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            BranchRight.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            
            BranchRightEntity = await _BranchRightService.BranchRightsMaintenance(BranchRight);

            if (BranchRightEntity != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult RemoveBranchRight(long BranchRightID)
        {
            var result = _BranchRightService.RemoveBranchRights(BranchRightID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> BranchRightData()
        {
            var BranchRightData = await _BranchRightService.GetAllBranchRightss();

            return Json(BranchRightData);
        }

        public async Task<ActionResult> BranchRightUniqueData(long PackageRightID)
        {
            var BranchRightData = await _BranchRightService.GetAllBranchRightsUniqData(PackageRightID);

            return View("~/Views/BranchWiseRight/RightsDataTable.cshtml",BranchRightData);
        }

    }
}