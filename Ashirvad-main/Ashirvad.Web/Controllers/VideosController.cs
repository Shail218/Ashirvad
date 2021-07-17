
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Gallery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class VideosController : BaseController
    {
        private readonly IGalleryService _gallaryService;
        public VideosController(IGalleryService gallaryService)
        {
            _gallaryService = gallaryService;
        }

        // GET: Videos
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> VideosMaintenance(long videoID)
        {
            GalleryMaintenanceModel branch = new GalleryMaintenanceModel();
            if (videoID > 0)
            {
                var result = await _gallaryService.GetGalleryByUniqueID(videoID);
                branch.GalleryInfo = result.Data;
            }

            var branchData = await _gallaryService.GetAllGalleryWithoutContent(2);
            branch.GalleryData = branchData.Data;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveVideos(GalleryEntity videos)
        {
            videos.GalleryType = 2;
            if (videos.ImageFile != null)
            {
                videos.FileInfo = Common.Common.ReadFully(videos.ImageFile.InputStream);
            }

            videos.Transaction = GetTransactionData(videos.UniqueID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            videos.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _gallaryService.GalleryMaintenance(videos);
            if (data != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult RemoveVideos(long videoID)
        {
            var result = _gallaryService.RemoveGallery(videoID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        [HttpPost]
        public async Task<string> GetPhoto(long videoID)
        {
            var data = await _gallaryService.GetGalleryByUniqueID(videoID);
            var result = data.Data;

            return "data:video/*;base64, " + result.FileEncoded;
        }

    }
}