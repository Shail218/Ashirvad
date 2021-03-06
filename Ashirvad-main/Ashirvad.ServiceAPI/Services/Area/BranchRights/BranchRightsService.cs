using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area
{
   public class BranchRightsService : IBranchRightsService
    {
        private readonly IBranchRightsAPI _BranchRightsContext;
        public BranchRightsService(IBranchRightsAPI BranchRightsContext)
        {
            this._BranchRightsContext = BranchRightsContext;
        }

        public async Task<ResponseModel> BranchRightsMaintenance(BranchWiseRightEntity BranchRightsInfo)
        {
            BranchWiseRightEntity standard = new BranchWiseRightEntity();
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _BranchRightsContext.RightsMaintenance(BranchRightsInfo);
              //  standard.BranchWiseRightsID = BranchRightsID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return responseModel;
        }

        

        public async Task<List<BranchWiseRightEntity>> GetAllBranchRightss()
        {
            try
            {
                return await this._BranchRightsContext.GetAllRights();
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<BranchWiseRightEntity>> GetAllCustomRights(DataTableAjaxPostModel model)
        {
            try
            {
                return await this._BranchRightsContext.GetAllCustomRights(model);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }

        public async Task<List<BranchWiseRightEntity>> GetAllBranchRightsUniqData(long PackageRightID)
        {
            try
            {
                return await this._BranchRightsContext.GetAllRightsUniqData(PackageRightID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveBranchRights(long BranchRightsID, string lastupdatedby)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                return this._BranchRightsContext.RemoveRights(BranchRightsID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return model;
        }

        public async Task<BranchWiseRightEntity> GetBranchRightsByID(long BranchRightsID)
        {
            try
            {
                return await this._BranchRightsContext.GetRightsByRightsID(BranchRightsID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<BranchWiseRightEntity>> GetBranchRightsByBranchID(long PackageRightID)
        {
            try
            {
                return await this._BranchRightsContext.GetAllRightsByBranch(PackageRightID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
    }
}
