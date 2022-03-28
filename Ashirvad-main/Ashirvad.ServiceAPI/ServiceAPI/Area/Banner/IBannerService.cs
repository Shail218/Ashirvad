using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Banner
{
    public interface IBannerService
    {
        Task<ResponseModel> BannerMaintenance(BannerEntity bannerInfo);
        Task<OperationResult<List<BannerEntity>>> GetAllBannerWithoutImage(long branchID = 0);
        Task<OperationResult<BannerEntity>> GetBannerByBannerID(long bannerID);
        Task<List<BannerEntity>> GetAllBanner(long branchID = 0);
        ResponseModel RemoveBanner(long bannerID, string lastupdatedby);
        Task<OperationResult<List<BannerEntity>>> GetAllBanner(long branchID, int bannerTypeID);
        Task<List<BannerEntity>> GetAllCustomBanner(DataTableAjaxPostModel model, long branchID, int bannerTypeID);
        Task<List<BannerEntity>> GetAllBannerforexcel(long branchID = 0);
    }
}
