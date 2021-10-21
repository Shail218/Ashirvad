using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Batch
{
    public interface IBatchAPI
    {
        Task<long> BatchMaintenance(BatchEntity batchInfo);
        Task<List<BatchEntity>> GetAllBatches(long branchID, long STDID = 0);
        bool RemoveBatch(long BatchID, string lastupdatedby);
        Task<BatchEntity> GetBatchByID(long branchID);
        Task<List<BatchEntity>> GetAllBatches();
    }
}
