using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Banner
{
    public interface IBannerService
    {
        Task<BannerEntity> BannerMaintenance(BannerEntity bannerInfo);
        Task<OperationResult<List<BannerEntity>>> GetAllBannerWithoutImage(long branchID = 0);
        Task<OperationResult<BannerEntity>> GetBannerByBannerID(long bannerID);
        Task<List<BannerEntity>> GetAllBanner(long branchID = 0);
        bool RemoveBanner(long bannerID, string lastupdatedby);
        Task<OperationResult<List<BannerEntity>>> GetAllBanner(long branchID, int bannerTypeID);
    }
}
