using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

namespace Ashirvad.Web.Controllers
{
    public class PackageController : BaseController
    {
        private readonly IPackageService _packageService;
        public ResponseModel res = new ResponseModel();

        public PackageController(IPackageService PackageService)
        {
            _packageService = PackageService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> PackageMaintenance(long branchID)
        {
            long packageID = branchID;
            PackageMaintenanceModel branch = new PackageMaintenanceModel();
            if (packageID > 0)
            {
                var result = await _packageService.GetPackageByIDAsync(packageID);
                branch.PackageInfo = result;
            }

            //var branchData = await _packageService.GetAllPackages(SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branch.PackageData = new List<PackageEntity>();

            return View("Index", branch);
        }

        public async Task<ActionResult> EditPackage(long packageID, long branchID)
        {
            PackageMaintenanceModel branch = new PackageMaintenanceModel();
            if (packageID > 0)
            {
                var result = await _packageService.GetPackageByIDAsync(packageID);
                branch.PackageInfo = result;
            }

            if (branchID > 0)
            {
                var result = await _packageService.GetAllPackages(branchID);
                branch.PackageData = result;
            }

            var branchData = await _packageService.GetAllPackages();
            branch.PackageData = branchData;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SavePackage(PackageEntity branch)
        {
            branch.Transaction = GetTransactionData(branch.PackageID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            branch.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            branch.BranchInfo = new BranchEntity();
            branch.BranchInfo = SessionContext.Instance.LoginUser.BranchInfo;
            res = await _packageService.PackageMaintenance(branch);
            return Json(res);
        }

        [HttpPost]
        public JsonResult RemovePackage(long packageID)
        {
            var result = _packageService.RemovePackage(packageID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> PackageData()
        {
            var branchData = await _packageService.GetAllPackages();
            return Json(branchData);
        }

        public async Task<JsonResult> PackageDataByBranch(long branchID)
        {
            var branchData = await _packageService.GetAllPackages(branchID);
            return Json(branchData);
        }

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            var branchData = await _packageService.GetAllCustomPackage(model, SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            long total = 0;
            if (branchData.Count > 0)
            {
                total = branchData.Count;
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