using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Gallery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        [Route("GalleryMaintenance/{UniqID}/{BranchID}/{Remark}/{UploadType}/{CreateId}/{CreateBy}/{TransactionId}/{FileName}/{Extension}/{HasFile}")]
        [HttpPost]
        public OperationResult<GalleryEntity> GalleryMaintenance(long UniqID,long BranchID, string Remark, int UploadType, long CreateId, string CreateBy, long TransactionId, string FileName, string Extension, bool HasFile)
        {
            OperationResult<GalleryEntity> result = new OperationResult<GalleryEntity>();
            var httpRequest = HttpContext.Current.Request;            
            GalleryEntity galleryEntity = new GalleryEntity();
            GalleryEntity data = new GalleryEntity();
            galleryEntity.Branch = new BranchEntity();
            galleryEntity.UniqueID = UniqID;
            galleryEntity.Branch.BranchID = BranchID;
            galleryEntity.Remarks = Remark;
            galleryEntity.GalleryType = UploadType;
            galleryEntity.FileName = FileName;
            galleryEntity.FilePath = "/GalleryImage/" + FileName + "." + Extension;       
            galleryEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            galleryEntity.Transaction = new TransactionEntity()
            {
                TransactionId = TransactionId,
                LastUpdateBy = CreateBy,
                LastUpdateId = CreateId,
                CreatedBy = CreateBy,
                CreatedId = CreateId,
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
                            // for live server
                            string UpdatedPath = currentDir.Replace("AshirvadAPI", "ashivadproduct");
                            // for local server
                            //string UpdatedPath = currentDir.Replace("Ashirvad.API", "Ashirvad.Web");
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
                string[] filename = FileName.Split(',');
                galleryEntity.FileName = filename[0];
                galleryEntity.FilePath = "/GalleryImage/" + filename[1] + "." + Extension;
            }
            data = this._galleryService.GalleryMaintenance(galleryEntity).Result;
            result.Completed = false;
            result.Data = null;
            if (data.UniqueID > 0)
            {
                result.Completed = true;
                result.Data = data;
                if (UniqID > 0)
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
    }
}
