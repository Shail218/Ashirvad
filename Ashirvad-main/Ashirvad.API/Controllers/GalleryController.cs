using Ashirvad.API.Filter;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Gallery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

    }
}
