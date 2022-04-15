using Ashirvad.Common;
using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Role
{
   public interface IRoleService
    {
        Task<ResponseModel> RoleMaintenance(RoleEntity RoleInfo);
        Task<List<RoleEntity>> GetAllRoles(long branchID);
        ResponseModel RemoveRole(long RoleID, string lastupdatedby);
        Task<List<RoleEntity>> GetAllRoles();
        Task<RoleEntity> GetRoleByID(long RoleID);
        Task<List<RoleEntity>> GetAllCustomRole(Common.Common.DataTableAjaxPostModel model, long branchID);
    }
}
