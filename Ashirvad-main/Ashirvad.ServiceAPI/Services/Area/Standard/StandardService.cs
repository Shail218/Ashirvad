using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Standard;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.Standard
{
    public class StandardService : IStandardService
    {
        private readonly IStandardAPI _standardContext;
        public StandardService(IStandardAPI standardContext)
        {
            this._standardContext = standardContext;
        }
        public async Task<StandardEntity> StandardMaintenance(StandardEntity standardInfo)
        {
            StandardEntity standard = new StandardEntity();
            try
            {
                long StandardID = await _standardContext.StandardMaintenance(standardInfo);
                standard.StandardID = StandardID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return standard;
        }

        public async Task<List<StandardEntity>> GetAllStandards(long branchID)
        {
            try
            {
                return await this._standardContext.GetAllStandards(branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<StandardEntity>> GetAllStandardsName(long branchid)
        {
            try
            {
                return await this._standardContext.GetAllStandardsName(branchid);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<StandardEntity>> GetAllStandardsID(string standardname, long branchid)
        {
            try
            {
                return await this._standardContext.GetAllStandardsID(standardname,branchid);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<StandardEntity> GetStandardsByID(long standardInfo)
        {
            try
            {
                return await _standardContext.GetStandardsByID(standardInfo);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemoveStandard(long StandardID, string lastupdatedby)
        {
            try
            {
                return this._standardContext.RemoveStandard(StandardID,lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }
    }
}
