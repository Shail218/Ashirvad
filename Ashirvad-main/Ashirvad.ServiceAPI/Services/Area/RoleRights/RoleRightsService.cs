using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.RoleRights;
using Ashirvad.ServiceAPI.ServiceAPI.Area.RoleRights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area.RoleRights
{
    public class RoleRightsService : IRoleRightsService
    {
        private readonly IRoleRightsAPI _RoleRightsContext;
        public RoleRightsService(IRoleRightsAPI RoleRightsContext)
        {
            this._RoleRightsContext = RoleRightsContext;
        }

        public async Task<ResponseModel> RoleRightsMaintenance(RoleRightsEntity RoleRightsInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            RoleRightsEntity RoleRights = new RoleRightsEntity();
            try
            {
                //long RoleRightsID = await _RoleRightsContext.RightsMaintenance(RoleRightsInfo);
                responseModel = await _RoleRightsContext.RightsMaintenance(RoleRightsInfo);
                //RoleRights.RoleRightsId = RoleRightsID;

            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            //return RoleRights;
            return responseModel;
        }



        public async Task<List<RoleRightsEntity>> GetRoleRightsByRoleRightsID(long RoleRightsID, long branchId)
        {
            try
            {
                List<RoleRightsEntity> RoleRights = new List<RoleRightsEntity>();
                RoleRights = await _RoleRightsContext.GetRightsByRightsID(RoleRightsID,branchId);
                return RoleRights;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }


        public async Task<RoleRightsEntity> GetRolerightsByID(long RoleRightsID)
        {
            try
            {
                RoleRightsEntity RoleRights = new RoleRightsEntity();
                RoleRights = await _RoleRightsContext.GetRolebyID(RoleRightsID);
                return RoleRights;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<RoleRightsEntity>> GetAllRoleRights()
        {
            try
            {
                return await this._RoleRightsContext.GetAllRights();
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<RoleRightsEntity>> GetAllCustomRights(DataTableAjaxPostModel model, long branchId)
        {
            try
            {
                return await this._RoleRightsContext.GetAllCustomRights(model,branchId);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }

        public ResponseModel RemoveRoleRights(long RoleRightsID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                return this._RoleRightsContext.RemoveRights(RoleRightsID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return responseModel;
            // return false;
        }
    }
}
