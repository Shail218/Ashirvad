using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.AboutUs;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Branch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/AboutUs/v1")]
    [AshirvadAuthorization]
    public class AboutUsController : ApiController
    {
        
        private readonly IAboutUsService _aboutUsService = null;
        public AboutUsController(IAboutUsService aboutUsService)
        {
            this._aboutUsService = aboutUsService;
        }

        [Route("GetAllAboutUs")]
        [HttpGet]
        public async Task<OperationResult<AboutUsEntity>> GetAllAboutUs(long BranchID)
        {
            var data =  this._aboutUsService.GetAboutUsByUniqueID(0, BranchID).Result;
            OperationResult<AboutUsEntity> result = new OperationResult<AboutUsEntity>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetAllAboutusDetails")]
        [HttpGet]
        public async Task<OperationResult<List<AboutUsDetailEntity>>> GetAllAboutusDetails(long BranchID)
        {
            var data =  this._aboutUsService.GetAllAboutUs(BranchID).Result;
            OperationResult<List<AboutUsDetailEntity>> result = new OperationResult<List<AboutUsDetailEntity>>();
            result.Completed = false;
            result.Data = null;
            if (data.Count > 0)
            {
                result.Completed = true;
                result.Data= data;
            }

            return result;
        }

        [Route("GetAllAboutUsWithoutContent")]
        [HttpGet]
        public async Task<OperationResult<List<AboutUsEntity>>> GetAllAboutUsWithoutContent(long BranchID)
        {
            var data = this._aboutUsService.GetAllAboutUsWithoutContent(BranchID).Result.Data;
            OperationResult<List<AboutUsEntity>> result = new OperationResult<List<AboutUsEntity>>();
            result.Completed = false;
            result.Data = null;
            if (data.Count > 0)
            {
                result.Completed = true;
                result.Data = data;
            }

            return result;
        }


    }
}
