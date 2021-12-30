using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Link;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

namespace Ashirvad.Web.Controllers
{
    public class LiveVideoController : BaseController
    {
        private readonly ILinkService _linkService;
        public LiveVideoController(ILinkService linkService)
        {
            _linkService = linkService;
        }

        // GET: LiveVideo
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> LiveVideoMaintenance(long linkID)
        {
            LinkMaintenanceModel branch = new LinkMaintenanceModel();
            if (linkID > 0)
            {
                var result = await _linkService.GetLinkByUniqueID(linkID);
                branch.LinkInfo = result.Data;
            }

            //var branchData = await _linkService.GetAllLink(1,SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branch.LinkData = new List<LinkEntity>();

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveLink(LinkEntity liveVideo)
        {
            liveVideo.LinkType = 1;
            liveVideo.Transaction = GetTransactionData(liveVideo.UniqueID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            liveVideo.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _linkService.LinkMaintenance(liveVideo);
            if (data != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult RemoveLink(long linkID)
        {
            var result = _linkService.RemoveLink(linkID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            List<string> columns = new List<string>();
            columns.Add("Title");
            columns.Add("LinkDesc");
            columns.Add("StandardName");
            columns.Add("LinkURL");
            foreach (var item in model.order)
            {
                item.name = columns[item.column];
            }
            var branchData = await _linkService.GetAllCustomVideoLink(model, SessionContext.Instance.LoginUser.BranchInfo.BranchID,1);
            long total = 0;
            if (branchData.Count > 0)
            {
                total = branchData[0].Count;
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