using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

namespace Ashirvad.Web.Controllers
{
    public class PageController : BaseController
    {
        private readonly IPageService _pageService;
        public ResponseModel res = new ResponseModel();

        public PageController(IPageService PageService)
        {
            _pageService = PageService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> PageMaintenance(long branchID)
        {
            long pageID = branchID;
            PageMaintenanceModel branch = new PageMaintenanceModel();
            if (pageID > 0) 
            {
                var result = await _pageService.GetPageByIDAsync(pageID);
                branch.PageInfo = result;
            }

            //var branchData = await _pageService.GetAllPages(SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branch.PageData = new List<PageEntity>();

            return View("Index", branch);
        }

        public async Task<ActionResult> EditPage(long pageID, long branchID)
        {
            PageMaintenanceModel branch = new PageMaintenanceModel();
            if (pageID > 0)
            {
                var result = await _pageService.GetPageByIDAsync(pageID);
                branch.PageInfo = result;
            }

            if (branchID > 0)
            {
                var result = await _pageService.GetAllPages(branchID);
                branch.PageData = result;
            }

            var branchData = await _pageService.GetAllPages();
            branch.PageData = branchData;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SavePage(PageEntity branch)
        {
            branch.Transaction = GetTransactionData(branch.PageID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            branch.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            branch.BranchInfo = new BranchEntity()
            {
                BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID
            };
            res = await _pageService.PageMaintenance(branch);
            //res.Status = data.PageID > 0 ? true : false;
            //res.Message = data.PageID == -1 ? "Page Already exists!!" : data.PageID == 0 ? "Page failed to insert!!" : "Page Inserted Successfully!!";
            return Json(res);
        }

        [HttpPost]
        public JsonResult RemovePage(long pageID)
        {
            var result = _pageService.RemovePage(pageID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> PageData()
        {
            var branchData = await _pageService.GetAllPages();
            return Json(branchData);
        }

        public async Task<JsonResult> PageDataByBranch(long branchID)
        {
            var branchData = await _pageService.GetAllPages(branchID);
            return Json(branchData);
        }


        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            List<string> columns = new List<string>();
            columns.Add("Page");
            foreach(var item in model.order)
            {
                item.name = columns[item.column];
            }
            var branchData = await _pageService.GetAllCustomPages(model);
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