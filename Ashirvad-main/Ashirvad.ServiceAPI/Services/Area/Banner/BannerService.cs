using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Banner;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Banner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area.Banner
{
    public class BannerService : IBannerService
    {
        private readonly IBannerAPI _bannerContext;
        public BannerService(IBannerAPI bannerContext)
        {
            this._bannerContext = bannerContext;
        }

        public async Task<ResponseModel> BannerMaintenance(BannerEntity bannerInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            BannerEntity banner = new BannerEntity();
            try
            {
                responseModel= await _bannerContext.BannerMaintenance(bannerInfo);
               
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<OperationResult<List<BannerEntity>>> GetAllBannerWithoutImage(long branchID = 0)
        {
            try
            {
                OperationResult<List<BannerEntity>> banner = new OperationResult<List<BannerEntity>>();
                banner.Data = await _bannerContext.GetAllBannerWithoutImage(branchID);
                banner.Completed = true;
                return banner;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<BannerEntity>> GetBannerByBannerID(long bannerID)
        {
            try
            {
                OperationResult<BannerEntity> banner = new OperationResult<BannerEntity>();
                banner.Data = await _bannerContext.GetBannerByBannerID(bannerID);
                banner.Completed = true;
                return banner;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<BannerEntity>> GetAllBanner(long branchID = 0)
        {
            try
            {
                return await this._bannerContext.GetAllBanner(branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveBanner(long bannerID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                return this._bannerContext.RemoveBanner(bannerID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<List<BannerEntity>> GetAllCustomBanner(DataTableAjaxPostModel model, long branchID, int bannerTypeID)
        {
            try
            {
                return await this._bannerContext.GetAllCustomBanner(model, branchID, bannerTypeID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<List<BannerEntity>>> GetAllBanner(long branchID, int bannerTypeID)
        {
            try
            {
                OperationResult<List<BannerEntity>> banner = new OperationResult<List<BannerEntity>>();
                banner.Data = await _bannerContext.GetAllBanner(branchID, bannerTypeID);
                banner.Completed = true;
                return banner;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<BannerEntity>> GetAllBannerforexcel(long branchID = 0)
        {
            try
            {
                return await this._bannerContext.GetAllBannerforexcel(branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

    }
}
