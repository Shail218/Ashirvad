using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Gallery;
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

            //var branchData = await _gallaryService.GetAllGalleryWithoutContent(1,SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branch.GalleryData = new List<GalleryEntity>();

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SavePhotos(GalleryEntity photos)
        {
            if (photos.ImageFile != null)
            {
                //photos.FileInfo = Common.Common.ReadFully(photos.ImageFile.InputStream);
                string _FileName = Path.GetFileName(photos.ImageFile.FileName);
                string extension = System.IO.Path.GetExtension(photos.ImageFile.FileName);
                string randomfilename = Common.Common.RandomString(20);
                string _Filepath = "/GalleryImage/" + randomfilename + extension;
                string _path = Path.Combine(Server.MapPath("~/GalleryImage"), randomfilename + extension);
                photos.ImageFile.SaveAs(_path);
                photos.FileName = _FileName;
                photos.FilePath = _Filepath;
            }

            photos.GalleryType = 1;
            photos.Transaction = GetTransactionData(photos.UniqueID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            photos.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _gallaryService.GalleryMaintenance(photos);
           
            return Json(data);
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

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            List<string> columns = new List<string>();
            columns.Add("Remark");
            foreach (var item in model.order)
            {
                item.name = columns[item.column];
            }
            var branchData = await _gallaryService.GetAllCustomPhotos(model, SessionContext.Instance.LoginUser.BranchInfo.BranchID,1);
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