using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Banner
{
    public interface IBannerAPI
    {
        Task<long> BannerMaintenance(BannerEntity bannerInfo);
        Task<List<BannerEntity>> GetAllBanner(long branchID);
        Task<List<BannerEntity>> GetAllBannerWithoutImage(long branchID);
        Task<BannerEntity> GetBannerByBannerID(long bannerID);
        bool RemoveBanner(long bannerID, string lastupdatedby);
        Task<List<BannerEntity>> GetAllBanner(long branchID, int bannerTypeID);
    }
}
