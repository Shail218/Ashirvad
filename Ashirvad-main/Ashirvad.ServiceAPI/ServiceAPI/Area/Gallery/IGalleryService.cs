using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Gallery
{
    public interface IGalleryService
    {
        Task<ResponseModel> GalleryMaintenance(GalleryEntity galleryInfo);
        Task<OperationResult<List<GalleryEntity>>> GetAllGalleryWithoutContent(int type, long branchID);
        Task<OperationResult<GalleryEntity>> GetGalleryByUniqueID(long uniqueID);
        Task<List<GalleryEntity>> GetAllGallery(int type, long branchID = 0);
        ResponseModel RemoveGallery(long galleryID, string lastupdatedby);
        Task<List<GalleryEntity>> GetAllCustomPhotos(DataTableAjaxPostModel model, long branchID, int type);
    }
}
