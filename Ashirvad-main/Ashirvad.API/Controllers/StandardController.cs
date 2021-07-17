using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [System.Web.Http.RoutePrefix("api/standard/v1")]
    [AshirvadAuthorization]
    public class StandardController : ApiController
    {
        private readonly IStandardService _standardService = null;
        public StandardController(IStandardService standardService)
        {
            this._standardService = standardService;
        }

        [Route("StandardMaintenance")]
        [HttpPost]
        public OperationResult<StandardEntity> StandardMaintenance(StandardEntity standardInfo)
        {
            var data = this._standardService.StandardMaintenance(standardInfo);
            OperationResult<StandardEntity> result = new OperationResult<StandardEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllStandards")]
        [HttpPost]
        public OperationResult<List<StandardEntity>> GetAllStandards(long branchID)
        {
            var data = this._standardService.GetAllStandards(branchID);
            OperationResult<List<StandardEntity>> result = new OperationResult<List<StandardEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }


        [Route("RemoveStandard")]
        [HttpPost]
        public OperationResult<bool> RemoveStandard(long StandardID, string lastupdatedby)
        {
            var data = this._standardService.RemoveStandard(StandardID,lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }
    }
}
