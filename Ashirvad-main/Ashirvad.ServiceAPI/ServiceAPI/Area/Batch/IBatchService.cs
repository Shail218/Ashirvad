using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Batch
{
    public interface IBatchService
    {
        Task<BatchEntity> BatchMaintenance(BatchEntity batchInfo);
        Task<List<BatchEntity>> GetAllBatches(long branchID);
        bool RemoveBatch(long BatchID, string lastupdatedby);
        Task<BatchEntity> GetBatchByID(long schoolID);
        Task<List<BatchEntity>> GetAllBatches();
    }
}
