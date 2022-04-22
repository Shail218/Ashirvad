using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.RoleRights
{
    public interface IRoleRightsAPI
    {
        Task<ResponseModel> RightsMaintenance(RoleRightsEntity RightsInfo);
        Task<List<RoleRightsEntity>> GetAllRights();
        Task<List<RoleRightsEntity>> GetRightsByRightsID(long RightsID, long branchId);
        ResponseModel RemoveRights(long RightsID, string lastupdatedby);
        Task<RoleRightsEntity> GetRolebyID(long RightsID);
        Task<List<RoleRightsEntity>> GetAllCustomRights(DataTableAjaxPostModel model, long branchId);
        Task<List<RoleRightsEntity>> GetAllRightsbyBranch(long branchId);
        Task<List<RoleRightsEntity>> GetPageListbyBranchID(long branchId);
    }
}
