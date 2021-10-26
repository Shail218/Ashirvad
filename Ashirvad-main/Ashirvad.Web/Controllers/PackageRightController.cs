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
    public class PackageRightController : BaseController
    {
        // GET: PackageRight
        private readonly IPackageRightsService _PackageRightService;
        private readonly IPageService _pageService;


        public PackageRightController(IPackageRightsService PackageRightService, IPageService pageService)
        {

            _PackageRightService = PackageRightService;
            _pageService = pageService;
        }
        // GET: PackageRight
        public ActionResult Index()
        {
            PackageRightMaintenanceModel packageRightMaintenance = new PackageRightMaintenanceModel();
            packageRightMaintenance.PackageRightsData = new List<PackageRightEntity>();
            return View(packageRightMaintenance);
        }

        public async Task<ActionResult> PackageRightMaintenance(long PackageRightID)
        {
            PackageRightMaintenanceModel PackageRight = new PackageRightMaintenanceModel();
            PackageRight.PackageRightsInfo = new PackageRightEntity();
            if (PackageRightID > 0)
            {
                var result = await _PackageRightService.GetPackageRightsByPackageRightsID(PackageRightID);
                PackageRight.PackageRightsInfo = result;
            }

            var PackageRightData = await _PackageRightService.GetAllPackageRights();
            PackageRight.PackageRightsInfo.list = PackageRightData;
            PackageRight.PackageRightsData = PackageRightData;

            var branchData = await _pageService.GetAllPages(SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            PackageRight.PackageRightsInfo.PageList = branchData;
            return View("Index", PackageRight);
        }

        [HttpPost]
        public async Task<JsonResult> SavePackageRight(PackageRightEntity PackageRight)
        {

            PackageRightEntity packageRightEntity = new PackageRightEntity();
            PackageRight.Transaction = GetTransactionData(PackageRight.PackageRightsId > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            PackageRight.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var List = JsonConvert.DeserializeObject<List<PackageRightEntity>>(PackageRight.JasonData);
            foreach (var item in List)
            {
                PackageRight.Packageinfo = item.Packageinfo;
                PackageRight.PackageRightsId = item.PackageRightsId;
                PackageRight.Createstatus = item.Createstatus;
                PackageRight.Viewstatus = item.Viewstatus;
                PackageRight.Deletestatus = item.Deletestatus;
                packageRightEntity = await _PackageRightService.PackageRightsMaintenance(PackageRight);
            }

            if (packageRightEntity != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult RemovePackageRight(long PackageRightID)
        {
            var result = _PackageRightService.RemovePackageRights(PackageRightID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> PackageRightData()
        {
            var PackageRightData = await _PackageRightService.GetAllPackageRights();

            return Json(PackageRightData);
        }
    }
}