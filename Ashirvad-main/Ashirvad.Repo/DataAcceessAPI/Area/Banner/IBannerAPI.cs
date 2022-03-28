using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Banner
{
    public interface IBannerAPI
    {
        Task<ResponseModel> BannerMaintenance(BannerEntity bannerInfo);
        Task<List<BannerEntity>> GetAllBanner(long branchID);
        Task<List<BannerEntity>> GetAllBannerWithoutImage(long branchID);
        Task<BannerEntity> GetBannerByBannerID(long bannerID);
        ResponseModel RemoveBanner(long bannerID, string lastupdatedby);
        Task<List<BannerEntity>> GetAllBanner(long branchID, int bannerTypeID);
        Task<List<BannerEntity>> GetAllCustomBanner(DataTableAjaxPostModel model, long branchID, int bannerTypeID);
        Task<List<BannerEntity>> GetAllBannerforexcel(long branchID);
    }
}
