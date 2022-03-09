﻿using Ashirvad.Data;
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

        public async Task<FeesEntity> FeesMaintenance(FeesEntity FeesInfo)
        {
            FeesEntity Fees = new FeesEntity();
            try
            {
                long FeesID = await _FeesContext.FeesMaintenance(FeesInfo);
                Fees.FeesID = FeesID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return Fees;
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

        public async Task<FeesEntity> GetFeesByFeesID(long FeesID, string financialyear)
        {
            try
            {
                FeesEntity Fees = new FeesEntity();
                Fees = await _FeesContext.GetFeesByFeesID(FeesID,financialyear);                
                return Fees;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        public async Task<List<FeesEntity>> GetFeesByBranchID(long BranchID,long courseid, long STDID, string financialyear)
        {
            try
            {
                List<FeesEntity> Fees = new List<FeesEntity>();
                Fees = await _FeesContext.GetAllFeesByBranchID(BranchID,courseid, STDID,financialyear);
                return Fees;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
       
        public async Task<List<FeesEntity>> GetAllFees(long BranchID, string financialyear)
        {
            try
            {
                return await this._FeesContext.GetAllFees(BranchID,financialyear);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<FeesEntity>> GetAllCustomFees(DataTableAjaxPostModel model, long branchID, string financialyear)
        {
            try
            {
                return await this._FeesContext.GetAllCustomFees(model, branchID,financialyear);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemoveFees(long FeesID, string lastupdatedby)
        {
            try
            {
                return this._FeesContext.RemoveFees(FeesID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

        Task<List<FeesEntity>> IFeesService.GetAllFeesWithoutImage()
        {
            throw new NotImplementedException();
        }
    }
}
