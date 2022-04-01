using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Gallery
{
    public interface IGalleryAPI
    {
        Task<ResponseModel> GalleryMaintenance(GalleryEntity galleryInfo);
        Task<List<GalleryEntity>> GetAllGallery(int type, long branchID);
        Task<List<GalleryEntity>> GetAllGalleryWithoutContent(int type, long branchID);
        Task<GalleryEntity> GetGalleryByUniqueID(long uniqueID);
        ResponseModel RemoveGallery(long uniqueID, string lastupdatedby);
        Task<List<GalleryEntity>> GetAllCustomPhotos(DataTableAjaxPostModel model, long branchID, int type);
    }
}
