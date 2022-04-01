using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.Repo.DataAcceessAPI.Area.Fees;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area
{
    public class FeesService : IFeesService
    {
        private readonly IFeesAPI _FeesContext;
        public FeesService(IFeesAPI FeesContext)
        {
            this._FeesContext = FeesContext;
        }

        public async Task<ResponseModel> FeesMaintenance(FeesEntity FeesInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            FeesEntity Fees = new FeesEntity();
            try
            {
                responseModel = await _FeesContext.FeesMaintenance(FeesInfo);
              //  Fees.FeesID = FeesID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<OperationResult<List<FeesEntity>>> GetAllFeesWithoutImage()
        {
            try
            {
                OperationResult<List<FeesEntity>> Fees = new OperationResult<List<FeesEntity>>();
                Fees.Data = await _FeesContext.GetAllFeesWithoutImage();
                Fees.Completed = true;
                return Fees;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<FeesEntity> GetFeesByFeesID(long FeesID)
        {
            try
            {
                FeesEntity Fees = new FeesEntity();
                Fees = await _FeesContext.GetFeesByFeesID(FeesID);                
                return Fees;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        public async Task<List<FeesEntity>> GetFeesByBranchID(long BranchID,long courseid, long STDID)
        {
            try
            {
                List<FeesEntity> Fees = new List<FeesEntity>();
                Fees = await _FeesContext.GetAllFeesByBranchID(BranchID,courseid, STDID);
                return Fees;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
       
        public async Task<List<FeesEntity>> GetAllFees(long BranchID)
        {
            try
            {
                return await this._FeesContext.GetAllFees(BranchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<FeesEntity>> GetAllCustomFees(DataTableAjaxPostModel model, long branchID)
        {
            try
            {
                return await this._FeesContext.GetAllCustomFees(model, branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveFees(long FeesID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                return this._FeesContext.RemoveFees(FeesID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        Task<List<FeesEntity>> IFeesService.GetAllFeesWithoutImage()
        {
            throw new NotImplementedException();
        }
    }
}
