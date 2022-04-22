using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.UserRights;
using Ashirvad.ServiceAPI.ServiceAPI.Area.UserRights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area.UserRights
{
    public class UserRightsService : IUserRightsService
    {
        private readonly IUserRightsAPI _UserRightsContext;
        public UserRightsService(IUserRightsAPI UserRightsContext)
        {
            this._UserRightsContext = UserRightsContext;
        }
        public async Task<ResponseModel> UserRightsMaintenance(UserWiseRightsEntity UserRightsInfo)
        {
            UserWiseRightsEntity standard = new UserWiseRightsEntity();
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _UserRightsContext.RightsMaintenance(UserRightsInfo);
                //  standard.UserWiseRightsID = UserRightsID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return responseModel;
        }

        public async Task<List<UserWiseRightsEntity>> GetAllUserRightss()
        {
            try
            {
                return await this._UserRightsContext.GetAllRights();
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<UserWiseRightsEntity>> GetAllCustomRights(DataTableAjaxPostModel model,long branchId)
        {
            try
            {
                return await this._UserRightsContext.GetAllCustomRights(model,branchId);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }
        
        public async Task<List<UserWiseRightsEntity>> GetAllUserRightsbyBranchId(long branchId)
        {
            try
            {
                return await this._UserRightsContext.GetAllUserRightsbyBranchId(branchId);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }

        public async Task<List<UserWiseRightsEntity>> GetAllUserRightsUniqData(long PackageRightID)
        {
            try
            {
                return await this._UserRightsContext.GetAllRightsUniqData(PackageRightID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveUserRights(long UserRightsID, string lastupdatedby)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                return this._UserRightsContext.RemoveRights(UserRightsID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return model;
        }

        public async Task<UserWiseRightsEntity> GetUserRightsByID(long UserRightsID)
        {
            try
            {
                return await this._UserRightsContext.GetRightsByRightsID(UserRightsID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<UserWiseRightsEntity>> GetUserRightsByUserID(long UserID)
        {
            try
            {
                return await this._UserRightsContext.GetAllRightsByUser(UserID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
    }
}
