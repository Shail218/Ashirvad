using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Role;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.Role
{
    public class RoleService : IRoleService
    {
        private readonly IRoleAPI _RoleContext;
        public RoleService(IRoleAPI RoleContext)
        {
            this._RoleContext = RoleContext;
        }

        public async Task<ResponseModel> RoleMaintenance(RoleEntity RoleInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            RoleEntity standard = new RoleEntity();
            try
            {
                //long RoleID = await _RoleContext.RoleMaintenance(RoleInfo);
                responseModel = await _RoleContext.RoleMaintenance(RoleInfo);
                //standard.RoleID = RoleID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            //return standard;
            return responseModel;
        }

        public async Task<List<RoleEntity>> GetAllRoles(long branchID)
        {
            try
            {
                return await this._RoleContext.GetAllRoles(branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<RoleEntity>> GetAllCustomRole(Common.Common.DataTableAjaxPostModel model, long branchID)
        {
            try
            {
                return await this._RoleContext.GetAllCustomRole(model, branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<RoleEntity>> GetAllRoles()
        {
            try
            {
                return await this._RoleContext.GetAllRoles();
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveRole(long RoleID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                return this._RoleContext.RemoveRole(RoleID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return responseModel;
            // return false;
        }

        public async Task<RoleEntity> GetRoleByID(long RoleID)
        {
            try
            {
                return await this._RoleContext.GetRoleByID(RoleID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
    }
}
