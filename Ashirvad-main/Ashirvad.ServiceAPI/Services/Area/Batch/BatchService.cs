using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Batch;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Batch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.Batch
{
    public class BatchService:IBatchService
    {
        private readonly IBatchAPI _batchContext;
        public BatchService(IBatchAPI batchContext)
        {
            this._batchContext = batchContext;
        }
        public async Task<BatchEntity> BatchMaintenance(BatchEntity batchInfo)
        {
            BatchEntity batch = new BatchEntity();
            try
            {
                long batchID = await _batchContext.BatchMaintenance(batchInfo);
                if (batchID > 0)
                {
                    batch.BatchID = batchID;
                }
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return batch;
        }

        public async Task<List<BatchEntity>> GetAllBatches(long branchID)
        {
            try
            {
                return await this._batchContext.GetAllBatches(branchID);
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

        public bool RemoveBatch(long BatchID, string lastupdatedby)
        {
            try
            {
                return this._batchContext.RemoveBatch(BatchID,lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }
    }
}
