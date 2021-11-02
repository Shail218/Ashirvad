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
        public async Task<JsonResult> SaveBranch(BranchEntity branch)
        {
            if (branch.ImageFile != null)
            {
                //photos.FileInfo = Common.Common.ReadFully(photos.ImageFile.InputStream);
                string _FileName = Path.GetFileName(branch.ImageFile.FileName);
                string extension = System.IO.Path.GetExtension(branch.ImageFile.FileName);
                string randomfilename = Common.Common.RandomString(20);
                string _Filepath = "/BranchImage/" + randomfilename + extension;
                string _path = Path.Combine(Server.MapPath("~/BranchImage"), randomfilename + extension);
                branch.ImageFile.SaveAs(_path);
                branch.BranchMaint.FileName = _FileName;
                branch.BranchMaint.FilePath = _Filepath;
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