using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Batch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/batch/v1")]
    [AshirvadAuthorization]
    public class BatchController : ApiController
    {
        private readonly IBatchService _batchService = null;
        public BatchController(IBatchService batchService)
        {
            this._batchService = batchService;
        }

        [Route("BatchMaintenance")]
        [HttpPost]
        public OperationResult<BatchEntity> BatchMaintenance(BatchEntity batchInfo)
        {
            var data = this._batchService.BatchMaintenance(batchInfo);
            OperationResult<BatchEntity> result = new OperationResult<BatchEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllBatches")]
        [HttpPost]
        public OperationResult<List<BatchEntity>> GetAllBatches(long branchID)
        {
            var data = this._batchService.GetAllBatches(branchID);
            OperationResult<List<BatchEntity>> result = new OperationResult<List<BatchEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }
        
        [Route("RemoveBatch")]
        [HttpPost]
        public OperationResult<bool> RemoveBatch(long BatchID, string lastupdatedby)
        {
            var data = this._batchService.RemoveBatch(BatchID,lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetAllBatchesByStd")]
        [HttpGet]
        public OperationResult<List<BatchEntity>> GetAllBatchesByStd(long branchID,long courseid,long StdID)
        {
            var data = this._batchService.GetAllBatchesBySTD(branchID,courseid,StdID);
            OperationResult<List<BatchEntity>> result = new OperationResult<List<BatchEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }
    }
}
