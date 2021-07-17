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
    public class PhotosController : BaseController
    {
        private readonly IGalleryService _gallaryService;
        public PhotosController(IGalleryService gallaryService)
        {
            _gallaryService = gallaryService;
        }

        // GET: Photos
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> PhotosMaintenance(long photoID)
        {
            GalleryMaintenanceModel branch = new GalleryMaintenanceModel();
            if (photoID > 0)
            {
                var result = await _gallaryService.GetGalleryByUniqueID(photoID);
                branch.GalleryInfo = result.Data;
            }

            var branchData = await _gallaryService.GetAllGalleryWithoutContent(1);
            branch.GalleryData = branchData.Data;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SavePhotos(GalleryEntity photos)
        {
            if (photos.ImageFile != null)
            {
                photos.FileInfo = Common.Common.ReadFully(photos.ImageFile.InputStream);
            }

            photos.GalleryType = 1;
            photos.Transaction = GetTransactionData(photos.UniqueID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            photos.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _gallaryService.GalleryMaintenance(photos);
            if (data != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult RemovePhotos(long photoID)
        {
            var result = _gallaryService.RemoveGallery(photoID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        [HttpPost]
        public async Task<string> GetPhoto(long photoID)
        {
            var data = await _gallaryService.GetGalleryByUniqueID(photoID);
            var result = data.Data;
            return "data:image/jpg;base64, " + data.Data.FileEncoded;
        }

    }
}