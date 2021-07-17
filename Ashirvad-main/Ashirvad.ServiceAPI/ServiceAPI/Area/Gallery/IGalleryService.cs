using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Gallery
{
    public interface IGalleryService
    {
        Task<GalleryEntity> GalleryMaintenance(GalleryEntity galleryInfo);
        Task<OperationResult<List<GalleryEntity>>> GetAllGalleryWithoutContent(int type, long branchID = 0);
        Task<OperationResult<GalleryEntity>> GetGalleryByUniqueID(long uniqueID);
        Task<List<GalleryEntity>> GetAllGallery(int type, long branchID = 0);
        bool RemoveGallery(long galleryID, string lastupdatedby);
    }
}
