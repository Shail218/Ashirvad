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

            var branchData = await _linkService.GetAllLink(1,SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branch.LinkData = branchData.Data;

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

    }
}