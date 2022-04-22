using Ashirvad.API.Filter;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/Role/v1")]
    [AshirvadAuthorization]
    public class RoleController : ApiController
    {
        private readonly IRoleService _RoleService;
        public RoleController(IRoleService RoleService)
        {
            _RoleService = RoleService;
        }

        [Route("RoleMaintenance")]
        [HttpPost]
        public OperationResult<RoleEntity> RoleMaintenance(RoleEntity roleInfo)
        {
            var data = this._RoleService.RoleMaintenance(roleInfo);
            OperationResult<RoleEntity> result = new OperationResult<RoleEntity>();
            result.Completed = data.Result.Status;
            if (data.Result.Status && data.Result.Data != null)
            {
                result.Data = (RoleEntity)data.Result.Data;
            }
            result.Message = data.Result.Message;
            return result;
        }

        [Route("GetAllRolesByBranch")]
        [HttpGet]
        public OperationResult<List<RoleEntity>> GetAllRoles(long branchID)
        {
            var data = this._RoleService.GetAllRoles(branchID);
            OperationResult<List<RoleEntity>> result = new OperationResult<List<RoleEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetRolesByRoleId")]
        [HttpGet]
        public OperationResult<RoleEntity> GetRolesByRoleId(long roleId)
        {
            var data = this._RoleService.GetRoleByID(roleId);
            OperationResult<RoleEntity> result = new OperationResult<RoleEntity>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }
       
        [Route("RemoveRole")]
        [HttpPost]
        public OperationResult<bool> RemoveRole(long roleId, string lastupdatedby)
        {
            var data = this._RoleService.RemoveRole(roleId, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = data.Status;
            result.Data = data.Status;
            result.Message = data.Message;
            return result;
        }


    }
}
