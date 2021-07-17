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
    public class YoutubeController : BaseController
    {
        private readonly ILinkService _linkService;
        public YoutubeController(ILinkService linkService)
        {
            _linkService = linkService;
        }

        // GET: Youtube
        public ActionResult Index()
        {
            return View();
        }


        public async Task<ActionResult> YoutubeMaintenance(long linkID)
        {
            LinkMaintenanceModel branch = new LinkMaintenanceModel();
            if (linkID > 0)
            {
                var result = await _linkService.GetLinkByUniqueID(linkID);
                branch.LinkInfo = result.Data;
            }

            var branchData = await _linkService.GetAllLink(2);
            branch.LinkData = branchData.Data;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveYoutube(LinkEntity youtubeVideo)
        {
            youtubeVideo.LinkType = 2;
            youtubeVideo.Transaction = GetTransactionData(youtubeVideo.UniqueID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);

            youtubeVideo.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };

            var data = await _linkService.LinkMaintenance(youtubeVideo);
            if (data != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult RemoveYoutube(long linkID)
        {
            var result = _linkService.RemoveLink(linkID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

    }
}