using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Gallery;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Gallery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Gallery
{
    public class GalleryService : IGalleryService
    {
        private readonly IGalleryAPI _galleryContext;
        public GalleryService(IGalleryAPI galleryContext)
        {
            this._galleryContext = galleryContext;
        }

        public async Task<ResponseModel> GalleryMaintenance(GalleryEntity galleryInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            GalleryEntity gallery = new GalleryEntity();
            try
            {
                responseModel = await _galleryContext.GalleryMaintenance(galleryInfo);
                //if (uniqueID > 0)
                //{
                //    gallery.UniqueID = uniqueID;
                //}
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<OperationResult<List<GalleryEntity>>> GetAllGalleryWithoutContent(int type, long branchID = 0)
        {
            try
            {
                OperationResult<List<GalleryEntity>> banner = new OperationResult<List<GalleryEntity>>();
                banner.Data = await _galleryContext.GetAllGalleryWithoutContent(type, branchID);
                banner.Completed = true;
                return banner;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<GalleryEntity>> GetAllCustomPhotos(DataTableAjaxPostModel model, long branchID,int type)
        {
            try
            {
                return await this._galleryContext.GetAllCustomPhotos(model, branchID,type);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<GalleryEntity>> GetGalleryByUniqueID(long uniqueID)
        {
            try
            {
                OperationResult<GalleryEntity> banner = new OperationResult<GalleryEntity>();
                banner.Data = await _galleryContext.GetGalleryByUniqueID(uniqueID);
                banner.Completed = true;
                return banner;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<GalleryEntity>> GetAllGallery(int type, long branchID=0)
        {
            try
            {
                return await this._galleryContext.GetAllGallery(type,branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveGallery(long galleryID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                return this._galleryContext.RemoveGallery(galleryID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }
    }
}
