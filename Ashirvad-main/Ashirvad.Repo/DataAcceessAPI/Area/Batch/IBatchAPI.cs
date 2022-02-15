using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Batch
{
    public interface IBatchAPI
    {
        Task<long> BatchMaintenance(BatchEntity batchInfo);
        Task<List<BatchEntity>> GetAllBatches(long branchID,long STDID = 0);
        Task<List<BatchEntity>> GetAllBatchesBySTD(long branchID, long courseid, long STDID = 0);
        bool RemoveBatch(long BatchID, string lastupdatedby);
        Task<BatchEntity> GetBatchByID(long branchID);
        Task<List<BatchEntity>> GetAllBatches();
        Task<List<BatchEntity>> GetAllCustomBatch(DataTableAjaxPostModel model, long branchID);
    }
}
