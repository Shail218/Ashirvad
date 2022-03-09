using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area
{
    public interface IFeesService
    {
        Task<FeesEntity> FeesMaintenance(FeesEntity FeesInfo);
        Task<List<FeesEntity>> GetAllFees(long BranchID, string financialyear);
    
        Task<List<FeesEntity>> GetAllFeesWithoutImage();
        Task<FeesEntity> GetFeesByFeesID(long FeesID, string financialyear);
        Task<List<FeesEntity>> GetFeesByBranchID(long BranchID,long courseid, long STDID, string financialyear);
        bool RemoveFees(long FeesID, string lastupdatedby);
        Task<List<FeesEntity>> GetAllCustomFees(DataTableAjaxPostModel model, long branchID, string financialyear);
    }
}
