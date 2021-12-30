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
        Task<List<FeesEntity>> GetAllFees(long branchID);
        Task<List<FeesEntity>> GetAllFeesWithoutImage();
        Task<FeesEntity> GetFeesByFeesID(long FeesID);
        Task<List<FeesEntity>> GetAllCustomFees(DataTableAjaxPostModel model, long branchID);
        Task<List<FeesEntity>> GetAllFeesByBranchID(long BranchID,long STDID);
        bool RemoveFees(long FeesID, string lastupdatedby);
       
    }
}
