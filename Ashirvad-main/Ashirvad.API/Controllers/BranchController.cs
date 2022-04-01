using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
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
    [RoutePrefix("api/branch/v1")]
    [AshirvadAuthorization]
    public class BranchController : ApiController
    {
        private readonly IBranchService _branchService = null;
        public BranchController(IBranchService branchService)
        {
            this._branchService = branchService;
        }

        [Route("TestMethod")]
        [HttpGet]
        public OperationResult<string> TestMethod()
        {
            OperationResult<string> result = new OperationResult<string>();
            result.Completed = true;
            result.Data = Security.GenerateToken("Akash");
            return result;
        }

        [Route("Validate")]
        [HttpGet]
        public OperationResult<string> Validate(string token)
        {
            OperationResult<string> result = new OperationResult<string>();
            result.Completed = true;
            result.Data = Security.ValidateToken(token);
            return result;
        }

        [Route("BranchMaintenance")]
        [HttpPost]
        public OperationResult<BranchEntity> BranchMaintenance(BranchEntity branchInfo)
        {
            var data = this._branchService.BranchMaintenance(branchInfo);
            OperationResult<BranchEntity> result = new OperationResult<BranchEntity>();
            result.Completed = data.Result.Status;
            if (data.Result.Status)
            {
                result.Data = (BranchEntity)data.Result.Data;
            }
            result.Message = data.Result.Message;
  
            return result;
        }

        [Route("GetAllBranch")]
        [HttpGet]
        public async Task<OperationResult<List<BranchEntity>>> GetAllBranch()
        {
            var data = await this._branchService.GetAllBranch();
            OperationResult<List<BranchEntity>> result = new OperationResult<List<BranchEntity>>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetBranchWithoutContent")]
        [HttpGet]
        public async Task<OperationResult<List<BranchEntity>>> GetBranchWithoutContent()
        {
            var data = await this._branchService.GetAllBranchWithoutImage();
            OperationResult<List<BranchEntity>> result = new OperationResult<List<BranchEntity>>();
            result = data;
            return result;
        }


        [Route("RemoveBranch")]
        [HttpPost]
        public OperationResult<bool> RemoveBranch(long BranchID, string lastupdatedby)
        {
            var data = this._branchService.RemoveBranch(BranchID,lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = data.Status;
            result.Data = data.Status;
            result.Message = data.Message;
            return result;
        }
    }
}
