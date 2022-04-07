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
using static Ashirvad.Common.Common;

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
            else{
                branch.BranchInfo = new BranchEntity();
                branch.BranchInfo.RowStatus = new RowStatusEntity()
                {
                    RowStatusId = 1
                };
            }
            //var branchData = await _branchService.GetAllBranchWithoutImage();
            branch.BranchData = new List<BranchEntity>();

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveBranch(BranchEntity branch)
        {
            ResponseModel response = new ResponseModel();
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
                if (branch.AppImageFile != null)
                {
                    //photos.FileInfo = Common.Common.ReadFully(photos.ImageFile.InputStream);
                    string _AppFileName = Path.GetFileName(branch.AppImageFile.FileName);
                    string extension = System.IO.Path.GetExtension(branch.AppImageFile.FileName);
                    string randomfilename = Common.Common.RandomString(20);
                    string _AppFilepath = "/BranchImage/" + randomfilename + extension;
                    string _path = Path.Combine(Server.MapPath("~/BranchImage"), randomfilename + extension);
                    branch.AppImageFile.SaveAs(_path);
                    branch.BranchMaint.AppFileName = _AppFileName;
                    branch.BranchMaint.AppFilePath = _AppFilepath;
                }
                branch.Transaction = GetTransactionData(branch.BranchID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
              

         
            var data = await _branchService.BranchMaintenance(branch);
            response = data;
            return Json(response);
        }

        [HttpPost]
        public JsonResult RemoveBranch(long branchID)
        {
            var result = _branchService.RemoveBranch(branchID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> BranchData()
        {
            var branchData = await _branchService.GetAllBranch();
            return Json(branchData);
        }

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            List<string> columns = new List<string>();
            columns.Add("BranchName");
            columns.Add("aliasName");
            columns.Add("");
            columns.Add("");
            columns.Add("RowStatusText");
            foreach (var item in model.order)
            {
                item.name = columns[item.column];
            }
            var branchData = await _branchService.GetAllCustomBranch(model);
            long total = 0;
            if (branchData.Count > 0)
            {
                total = branchData[0].Count;
            }
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                iTotalRecords = total,
                iTotalDisplayRecords = total,
                data = branchData
            });

        }

    }
}