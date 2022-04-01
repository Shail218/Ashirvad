
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

            //var branchData = await _gallaryService.GetAllGalleryWithoutContent(2,SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branch.GalleryData = new List<GalleryEntity>();

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveVideos(GalleryEntity videos,HttpPostedFileBase ImageFile)
        {
       
            if(videos.ImageFile != null)
            {
                string _FileName = Path.GetFileName(videos.ImageFile.FileName);
                string extension = System.IO.Path.GetExtension(videos.ImageFile.FileName);
                string randomfilename = Common.Common.RandomString(20);
                string _Filepath = "/GalleryImage/" + randomfilename + extension;
                string _path = Path.Combine(Server.MapPath("~/GalleryImage"), randomfilename + extension);
                videos.ImageFile.SaveAs(_path);
                videos.FileName = _FileName;
                videos.FilePath = _Filepath;
                
            }
            
            //photos.FileInfo = Common.Common.ReadFully(photos.ImageFile.InputStream);
            videos.GalleryType = 2;
            videos.Transaction = GetTransactionData(videos.UniqueID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            videos.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            videos.Branch = new BranchEntity()
            {
                BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID
            };
            var data = await _gallaryService.GalleryMaintenance(videos);
            
            return Json(data);
        }

        [HttpPost]
        public JsonResult RemoveVideos(long videoID)
        {
            var result = _gallaryService.RemoveGallery(videoID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            List<string> columns = new List<string>();
            columns.Add("Remark");
            foreach (var item in model.order)
            {
                item.name = columns[item.column];
            }
            var branchData = await _gallaryService.GetAllCustomPhotos(model, SessionContext.Instance.LoginUser.BranchInfo.BranchID, 2);
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