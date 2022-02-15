using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Batch
{
    public interface IBatchService
    {
        Task<BatchEntity> BatchMaintenance(BatchEntity batchInfo);
        Task<List<BatchEntity>> GetAllBatches(long branchID, long StdID=0);
        Task<List<BatchEntity>> GetAllBatchesBySTD(long branchID, long courseid, long STDID = 0);
        bool RemoveBatch(long BatchID, string lastupdatedby);
        Task<BatchEntity> GetBatchByID(long schoolID);
        Task<List<BatchEntity>> GetAllBatches();
        Task<List<BatchEntity>> GetAllCustomBatch(DataTableAjaxPostModel model, long branchID);
    }
}
