using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area
{
    public interface IBranchRightsAPI
    {
        Task<ResponseModel> RightsMaintenance(BranchWiseRightEntity RightsInfo);
        Task<List<BranchWiseRightEntity>> GetAllRights();
        Task<List<BranchWiseRightEntity>> GetAllRightsUniqData(long PackageRightID);
        Task<BranchWiseRightEntity> GetRightsByRightsID(long RightsID);
        ResponseModel RemoveRights(long RightsID, string lastupdatedby);
        Task<List<BranchWiseRightEntity>> GetAllRightsByBranch(long PackageRightID);
        Task<List<BranchWiseRightEntity>> GetAllCustomRights(DataTableAjaxPostModel model);
    }
}
