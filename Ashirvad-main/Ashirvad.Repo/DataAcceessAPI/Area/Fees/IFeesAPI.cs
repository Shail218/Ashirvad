using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Fees
{
    public interface IFeesAPI
    {
        Task<long> FeesMaintenance(FeesEntity branchInfo);
        Task<List<FeesEntity>> GetAllFees(long branchID, string financialyear);
        Task<List<FeesEntity>> GetAllFeesWithoutImage();
        Task<FeesEntity> GetFeesByFeesID(long FeesID, string financialyear);
        Task<List<FeesEntity>> GetAllCustomFees(DataTableAjaxPostModel model, long branchID, string financialyear);
        Task<List<FeesEntity>> GetAllFeesByBranchID(long BranchID,long courseid, long STDID, string financialyear);
        bool RemoveFees(long FeesID, string lastupdatedby);
       
    }
}
