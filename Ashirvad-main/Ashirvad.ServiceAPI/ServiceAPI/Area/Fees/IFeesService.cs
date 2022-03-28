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
        Task<ResponseModel> FeesMaintenance(FeesEntity FeesInfo);
        Task<List<FeesEntity>> GetAllFees(long BranchID);
    
        Task<List<FeesEntity>> GetAllFeesWithoutImage();
        Task<FeesEntity> GetFeesByFeesID(long FeesID);
        Task<List<FeesEntity>> GetFeesByBranchID(long BranchID,long courseid, long STDID);
        ResponseModel RemoveFees(long FeesID, string lastupdatedby);
        Task<List<FeesEntity>> GetAllCustomFees(DataTableAjaxPostModel model, long branchID);
    }
}
