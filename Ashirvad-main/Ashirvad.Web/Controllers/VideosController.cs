
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

namespace Ashirvad.Web.Controllers
{
    public class VideosController : BaseController
    {
        private readonly IGalleryService _gallaryService;
        OperationResult operation = new OperationResult();
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

            var branchData = await _gallaryService.GetAllGalleryWithoutContent(2,SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branch.GalleryData = branchData.Data;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveVideos(GalleryEntity videos)
        {
            //photos.FileInfo = Common.Common.ReadFully(photos.ImageFile.InputStream);
            string _FileName = Path.GetFileName(videos.ImageFile.FileName);
            string extension = System.IO.Path.GetExtension(videos.ImageFile.FileName);
            string randomfilename = Common.Common.RandomString(20);
            string _Filepath = "/GalleryImage/" + randomfilename + extension;
            string _path = Path.Combine(Server.MapPath("~/GalleryImage"), randomfilename + extension);
            videos.ImageFile.SaveAs(_path);
            videos.FileName = _FileName;
            videos.FilePath = _Filepath;
            videos.GalleryType = 2;
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

        public async Task<JsonResult> DownloadVideo(long videoID)
        {
            string[] array = new string[3];
            try
            {
                OperationResult<GalleryEntity> operationResult = new OperationResult<GalleryEntity>();
                operationResult = await _gallaryService.GetGalleryByUniqueID(videoID);
                if (operationResult != null)
                {
                    string ext = GetFileExtension(operationResult.Data.FileEncoded);
                    string file = operationResult.Data.FileEncoded;
                    string filename = DateTime.Now.ToString("dd/MM/yyyy");
                    array[0] = ext;
                    array[1] = file;
                    array[2] = filename;
                    return Json(array);
                }
            }
            catch(Exception ex)
            {

            }
            return Json(array);
        }

        public static string GetFileExtension(string base64String)
        {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "AAAAF":
                    return "mp4";
                case "AAAAG":
                    return "mp4";
                case "JVBER":
                    return "pdf";
                case "AAABA":
                    return "ico";
                case "UMFYI":
                    return "rar";
                case "UEsDBB":
                    return "docx";
                default:
                    return string.Empty;
            }
        }

    }
}