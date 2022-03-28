using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Batch;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Batch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area.Batch
{
    public class BatchService:IBatchService
    {
        private readonly IBatchAPI _batchContext;
        public BatchService(IBatchAPI batchContext)
        {
            this._batchContext = batchContext;
        }
        public async Task<ResponseModel> BatchMaintenance(BatchEntity batchInfo)
        {
            ResponseModel responseModel = new ResponseModel();
               BatchEntity batch = new BatchEntity();
            try
            {
                responseModel= await _batchContext.BatchMaintenance(batchInfo);
                //batch.BatchID = batchID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<List<BatchEntity>> GetAllBatches(long branchID,long STDID=0)
        {
            try
            {
                return await this._batchContext.GetAllBatches(branchID, STDID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<BatchEntity>> GetAllBatchesBySTD(long branchID, long courseid,long STDID = 0)
        {
            try
            {
                return await this._batchContext.GetAllBatchesBySTD(branchID, courseid,STDID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<BatchEntity>> GetAllCustomBatch(DataTableAjaxPostModel model, long branchID)
        {
            try
            {
                return await this._batchContext.GetAllCustomBatch(model, branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<BatchEntity>> GetAllBatches()
        {
            try
            {
                return await this._batchContext.GetAllBatches();
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }


        public async Task<BatchEntity> GetBatchByID(long branchID)
        {
            try
            {
                return await this._batchContext.GetBatchByID(branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveBatch(long BatchID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                return this._batchContext.RemoveBatch(BatchID,lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }
    }
}
