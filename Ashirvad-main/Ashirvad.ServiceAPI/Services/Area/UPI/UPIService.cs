using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.UPI;
using Ashirvad.ServiceAPI.ServiceAPI.Area.UPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.UPI
{
    public class UPIService : IUPIService
    {
        private readonly IUPIAPI _upiContext;
        public UPIService(IUPIAPI upiContext)
        {
            this._upiContext = upiContext;
        }

        public async Task<ResponseModel> UPIMaintenance(UPIEntity upiInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            UPIEntity upi = new UPIEntity();
            try
            {
                responseModel = await _upiContext.UPIMaintenance(upiInfo);
                //long upiID = await _upiContext.UPIMaintenance(upiInfo);
                //upi.UPIId = upiID;
                //if (upiID > 0)
                //{
                    
                //}
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<OperationResult<List<UPIEntity>>> GetAllUPIs(long branchID)
        {
            try
            {
                OperationResult<List<UPIEntity>> upis = new OperationResult<List<UPIEntity>>();
                upis.Data = await _upiContext.GetAllUPIs(branchID);
                upis.Completed = true;
                return upis;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<UPIEntity>> GetUPIByUPIID(long upiID)
        {
            try
            {
                OperationResult<UPIEntity> upi = new OperationResult<UPIEntity>();
                upi.Data = await _upiContext.GetUPIByID(upiID);
                upi.Completed = true;
                return upi;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveUPI(long upiID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                return this._upiContext.RemoveUPI(upiID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }
    }
}
