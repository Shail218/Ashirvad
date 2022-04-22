using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.RoleRights
{
    public interface IRoleRightsService
    {
        Task<ResponseModel> RoleRightsMaintenance(RoleRightsEntity RoleRightsInfo);
        Task<List<RoleRightsEntity>> GetAllRoleRights();
        Task<List<RoleRightsEntity>> GetAllCustomRights(DataTableAjaxPostModel model,long branchId);
        Task<List<RoleRightsEntity>> GetRoleRightsByRoleRightsID(long RoleRightsID, long branchId);
        Task<RoleRightsEntity> GetRolerightsByID(long RoleRightsID);
        ResponseModel RemoveRoleRights(long RoleRightsID, string lastupdatedby);
        Task<List<RoleRightsEntity>> GetAllRightsbyBranch(long branchId);
    }
}
