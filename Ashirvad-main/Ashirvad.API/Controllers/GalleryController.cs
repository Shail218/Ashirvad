using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Gallery;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/gallery/v1")]
    [AshirvadAuthorization]
    public class GalleryController : ApiController
    {
        private readonly IGalleryService _galleryService = null;
        public GalleryController(IGalleryService galleryService)
        {
            this._galleryService = galleryService;
        }

        [Route("GalaryImageMaintenance")]
        [HttpPost]
        public OperationResult<GalleryEntity> GalaryImageMaintenance(GalleryEntity galleryInfo)
        {
            galleryInfo.GalleryType = 1;
            var data = this._galleryService.GalleryMaintenance(galleryInfo);
            OperationResult<GalleryEntity> result = new OperationResult<GalleryEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllGalleryImages")]
        [HttpGet]
        public OperationResult<List<GalleryEntity>> GetAllGalleryImages()
        {
            var data = this._galleryService.GetAllGallery(1);
            OperationResult<List<GalleryEntity>> result = new OperationResult<List<GalleryEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllGalleryImagesByBranch")]
        [HttpGet]
        public OperationResult<List<GalleryEntity>> GetAllGalleryImages(long branchID)
        {
            var data = this._galleryService.GetAllGallery(1, branchID);
            OperationResult<List<GalleryEntity>> result = new OperationResult<List<GalleryEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }


        [Route("GetGalleryByID")]
        [HttpPost]
        public OperationResult<GalleryEntity> GetGalleryImageByID(long uniqueID)
        {
            var data = this._galleryService.GetGalleryByUniqueID(uniqueID);
            OperationResult<GalleryEntity> result = new OperationResult<GalleryEntity>();
            result = data.Result;
            return result;
        }


        [Route("GalaryVideoMaintenance")]
        [HttpPost]
        public OperationResult<GalleryEntity> GalaryVideoMaintenance(GalleryEntity galleryInfo)
        {
            galleryInfo.GalleryType = 2;
            var data = this._galleryService.GalleryMaintenance(galleryInfo);
            OperationResult<GalleryEntity> result = new OperationResult<GalleryEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllGalleryVideo")]
        [HttpGet]
        public OperationResult<List<GalleryEntity>> GetAllGalleryVideo()
        {
            var data = this._galleryService.GetAllGallery(2);
            OperationResult<List<GalleryEntity>> result = new OperationResult<List<GalleryEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllGalleryVideoByBranch")]
        [HttpGet]
        public OperationResult<List<GalleryEntity>> GetAllGalleryVideo(long branchID)
        {
            var data = this._galleryService.GetAllGallery(2, branchID);
            OperationResult<List<GalleryEntity>> result = new OperationResult<List<GalleryEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("RemoveGallery")]
        [HttpPost]
        public OperationResult<bool> RemoveGallery(long uniqueID, string lastupdatedby)
        {
            var data = this._galleryService.RemoveGallery(uniqueID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GalleryMaintenance")]
        [HttpPost]
        public OperationResult<GalleryEntity> GalleryMaintenance(string model, bool HasFile)
        {
            OperationResult<GalleryEntity> result = new OperationResult<GalleryEntity>();
            var httpRequest = HttpContext.Current.Request;            
            GalleryEntity galleryEntity = new GalleryEntity();
            galleryEntity.Branch = new BranchEntity();
            var entity = JsonConvert.DeserializeObject<GalleryEntity>(model);
            galleryEntity.UniqueID = entity.UniqueID;
            galleryEntity.Branch.BranchID = entity.Branch.BranchID;
            galleryEntity.Remarks = entity.Remarks;
            galleryEntity.GalleryType = entity.GalleryType;     
            galleryEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            galleryEntity.Transaction = new TransactionEntity()
            {
                TransactionId = entity.Transaction.TransactionId,
                LastUpdateBy = entity.Transaction.LastUpdateBy,
                LastUpdateId = entity.Transaction.LastUpdateId,
                CreatedBy = entity.Transaction.CreatedBy,
                CreatedId = entity.Transaction.CreatedId
            };
            if (HasFile)
            {
                try
                {
                    if (httpRequest.Files.Count > 0)
                    {
                        foreach (string file in httpRequest.Files)
                        {
                            string fileName;
                            string extension;
                            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                            string UpdatedPath = currentDir.Replace("Ashirvad.API", "Ashirvad.Web");
                            var postedFile = httpRequest.Files[file];
                            string randomfilename = Common.Common.RandomString(20);
                            extension = Path.GetExtension(postedFile.FileName);
                            fileName = Path.GetFileName(postedFile.FileName);
                            string _Filepath = "/GalleryImage/" + randomfilename + extension;
                            string _Filepath1 = "GalleryImage/" + randomfilename + extension;
                            var filePath = HttpContext.Current.Server.MapPath("~/GalleryImage/" + randomfilename + extension);
                            string _path = UpdatedPath + _Filepath1;
                            postedFile.SaveAs(_path);
                            galleryEntity.FileName = fileName;
                            galleryEntity.FilePath = _Filepath;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Completed = false;
                    result.Data = null;
                    result.Message = ex.ToString();
                }
            }
            else
            {
                galleryEntity.FileName = entity.FileName;
                galleryEntity.FilePath = entity.FilePath;
            }
            var data = this._galleryService.GalleryMaintenance(galleryEntity).Result;
            result.Completed = false;
            result.Data = null;
            if (data.UniqueID > 0)
            {
                result.Completed = true;
                result.Data = data;
                if (entity.UniqueID > 0)
                {
                    result.Message = "Gallery Updated Successfully.";
                }
                else
                {
                    result.Message = "Gallery Created Successfully.";
                }
            }
            return result;
        }

        public static string Decode(string Path)
        {
            byte[] mybyte = Convert.FromBase64String(Path);
            string returntext = Encoding.UTF8.GetString(mybyte);
            return returntext;
        }
    }
}
