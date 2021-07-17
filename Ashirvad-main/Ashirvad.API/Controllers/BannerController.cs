using Ashirvad.API.Filter;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Banner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/banner/v1")]
    [AshirvadAuthorization]
    public class BannerController : ApiController
    {
        private readonly IBannerService _bannerService = null;
        public BannerController(IBannerService bannerService)
        {
            this._bannerService = bannerService;
        }

        [Route("BannerMaintenance")]
        [HttpPost]
        public OperationResult<BannerEntity> BannerMaintenance(BannerEntity bannerInfo)
        {
            var data = this._bannerService.BannerMaintenance(bannerInfo);
            OperationResult<BannerEntity> result = new OperationResult<BannerEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllBanner")]
        [HttpGet]
        public OperationResult<List<BannerEntity>> GetAllBanner()
        {
            var data = this._bannerService.GetAllBanner();
            OperationResult<List<BannerEntity>> result = new OperationResult<List<BannerEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllBannerByBranchAndType")]
        [HttpGet]
        public OperationResult<List<BannerEntity>> GetAllBanner(long branchID, int bannerTypeID)
        {
            var data = this._bannerService.GetAllBanner(branchID, bannerTypeID);
            return data.Result;
        }

        [Route("GetAllBannerByBranch")]
        [HttpGet]
        public OperationResult<List<BannerEntity>> GetAllBanner(long branchID)
        {
            var data = this._bannerService.GetAllBanner(branchID);
            OperationResult<List<BannerEntity>> result = new OperationResult<List<BannerEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }


        [Route("GetBannerByID")]
        [HttpPost]
        public OperationResult<BannerEntity> GetBannerByID(long bannerID)
        {
            var data = this._bannerService.GetBannerByBannerID(bannerID);
            OperationResult<BannerEntity> result = new OperationResult<BannerEntity>();
            result = data.Result;
            return result;
        }

        [Route("RemoveBanner")]
        [HttpPost]
        public OperationResult<bool> RemoveBanner(long bannerID, string lastupdatedby)
        {
            var data = this._bannerService.RemoveBanner(bannerID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }
    }
}
