using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Circular;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Circular;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area.Circular
{
    public class CircularService : ICircularService
    {
        private readonly ICircularAPI _circularContext;
        public CircularService(ICircularAPI circularContext)
        {
            this._circularContext = circularContext;
        }

        public async Task<CircularEntity> CircularMaintenance(CircularEntity circularInfo)
        {
            CircularEntity branch = new CircularEntity();
            try
            {
                long circularID = await _circularContext.CircularMaintenance(circularInfo);
                if (circularID > 0)
                {
                    branch.CircularId = circularID;
                }

            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return branch;
        }

        public async Task<List<CircularEntity>> GetAllCircular()
        {
            try
            {
                return await this._circularContext.GetAllCircular();
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<CircularEntity>> GetAllCustomCircular(DataTableAjaxPostModel model)
        {
            try
            {
                return await this._circularContext.GetAllCustomCircular(model);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<CircularEntity> GetCircularById(long circularID)
        {
            try
            {
                return await this._circularContext.GetCircularById(circularID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemoveCircular(long CircularId, string lastupdatedby)
        {
            try
            {
                return this._circularContext.RemoveCircular(CircularId, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }
    }
}
