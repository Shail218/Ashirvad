using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Branch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class BranchController : BaseController
    {

        private readonly IBranchService _branchService;
        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }
        // GET: Branch
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> BranchMaintenance(long branchID)
        {
            BranchMaintenanceModel branch = new BranchMaintenanceModel();
            if (branchID > 0)
            {
                var result = await _branchService.GetBranchByBranchID(branchID);
                branch.BranchInfo = result.Data;
            }

            var branchData = await _branchService.GetAllBranchWithoutImage();
            branch.BranchData = branchData.Data;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<string> GetBranchLogo(long branchID)
        {
            var data = await _branchService.GetBranchByBranchID(branchID);
            var result = data.Data;
            return "data:image/jpg;base64, " + Convert.ToBase64String(result.BranchMaint.BranchLogo, 0, result.BranchMaint.BranchLogo.Length);
        }

        [HttpPost]
        public async Task<JsonResult> SaveBranch(BranchEntity branch)
        {
            if (branch.ImageFile != null)
            {
                branch.BranchMaint = new BranchMaint();
                branch.BranchMaint.BranchLogo = Common.Common.ReadFully(branch.ImageFile.InputStream);
                branch.BranchMaint.BranchLogoExt = Path.GetExtension(branch.ImageFile.FileName);
            }

            branch.Transaction = GetTransactionData(branch.BranchID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            var data = await _branchService.BranchMaintenance(branch);
            if (data != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult RemoveBranch(long branchID)
        {
            var result = _branchService.RemoveBranch(branchID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> BranchData()
        {
            var branchData = await _branchService.GetAllBranchWithoutImage();
            //if (SessionContext.Instance.LoginUser.UserType != Common.Enums.UserType.SuperAdmin)
            //{
            //    var result = branchData.Data.Where(x => x.BranchID == SessionContext.Instance.LoginUser.BranchInfo.BranchID).ToList();
            //    return Json(result);
            //}

            return Json(branchData.Data);
        }

    }
}