using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Role
{
    public interface IRoleAPI
    {
        Task<ResponseModel> RoleMaintenance(RoleEntity roleInfo);
        Task<List<RoleEntity>> GetAllRoles(long branchID);
        ResponseModel RemoveRole(long RoleID, string lastupdatedby);
        Task<List<RoleEntity>> GetAllRoles();
        Task<RoleEntity> GetRoleByID(long RoleID);
        Task<List<RoleEntity>> GetAllCustomRole(Common.Common.DataTableAjaxPostModel model, long branchID);
    }
}
