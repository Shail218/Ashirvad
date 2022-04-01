using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area
{
   public interface IBranchRightsService
    {
        Task<ResponseModel> BranchRightsMaintenance(BranchWiseRightEntity BranchRightsInfo);
        ResponseModel RemoveBranchRights(long BranchRightsID, string lastupdatedby);
        Task<List<BranchWiseRightEntity>> GetAllBranchRightss();
        Task<List<BranchWiseRightEntity>> GetAllBranchRightsUniqData(long PackageRightID);
        Task<BranchWiseRightEntity> GetBranchRightsByID(long standardID);
        Task<List<BranchWiseRightEntity>> GetBranchRightsByBranchID(long PackageRightID);
        Task<List<BranchWiseRightEntity>> GetAllCustomRights(DataTableAjaxPostModel model);
    }
}
