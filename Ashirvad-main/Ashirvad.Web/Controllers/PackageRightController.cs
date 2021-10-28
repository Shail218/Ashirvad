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

        ResponseModel response = new ResponseModel();
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
            PackageRight.PackageRightsInfo.list = new List<PackageRightEntity>();
            if (PackageRightID > 0)
            {
                var result = await _PackageRightService.GetPackaegrightsByID(PackageRightID);
                var result2 = await _PackageRightService.GetPackageRightsByPackageRightsID(PackageRightID);
                PackageRight.PackageRightsInfo = result;
                PackageRight.PackageRightsInfo.list = result2;
            }
            var PackageRightData = await _PackageRightService.GetAllPackageRights();


            PackageRight.PackageRightsData = PackageRightData;

            var branchData = await _pageService.GetAllPages(SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            PackageRight.PackageRightsInfo.PageList = branchData;
            return View("Index", PackageRight);
        }

        [HttpPost]
        public async Task<JsonResult> SavePackageRight(PackageRightEntity PackageRight)
        {
            response.Status = false;

            long rightsID = PackageRight.PackageRightsId;
            PackageRightEntity packageRightEntity = new PackageRightEntity();

            PackageRight.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var List = JsonConvert.DeserializeObject<List<PackageRightEntity>>(PackageRight.JasonData);
            foreach (var item in List)
            {
                PackageRight.Transaction = GetTransactionData(rightsID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
                PackageRight.PackageRightsId = rightsID == 0 ? rightsID : item.PackageRightsId;
                PackageRight.PageInfo = item.PageInfo;
                PackageRight.Createstatus = item.Createstatus;
                PackageRight.Viewstatus = item.Viewstatus;
                PackageRight.Deletestatus = item.Deletestatus;
                packageRightEntity = await _PackageRightService.PackageRightsMaintenance(PackageRight);
                if (packageRightEntity.PackageRightsId < 0)
                {
                    break;
                }
            }
            if (packageRightEntity.PackageRightsId > 0)
            {
                response.Status = true;
                response.Message = PackageRight.PackageRightsId > 0 ? "Updated Successfully!!" : "Created Successfully!!";


            }
            else if (packageRightEntity.PackageRightsId < 0)
            {
                response.Status = false;
                response.Message = "Already Exists!!";
            }
            else
            {
                response.Status = false;
                response.Message = PackageRight.PackageRightsId > 0 ? "Failed To Update!!" : "Failed To Create!!";
            }
            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> RemovePackageRight(long PackageRightID)
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