using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Gallery
{
    public interface IGalleryAPI
    {
        Task<long> GalleryMaintenance(GalleryEntity galleryInfo);
        Task<List<GalleryEntity>> GetAllGallery(int type, long branchID);
        Task<List<GalleryEntity>> GetAllGalleryWithoutContent(int type, long branchID);
        Task<GalleryEntity> GetGalleryByUniqueID(long uniqueID);
        bool RemoveGallery(long uniqueID, string lastupdatedby);
    }
}
