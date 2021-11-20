using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area
{
    public class BranchClassService : IBranchClassService
    {
        private readonly IBranchClassAPI _BranchClassContext;        

        public BranchClassService(IBranchClassAPI BranchClassContext)
        {
            this._BranchClassContext = BranchClassContext;
        }

        public async Task<BranchClassEntity> BranchClassMaintenance(BranchClassEntity BranchClassInfo)
        {
            BranchClassEntity BranchClass = new BranchClassEntity();
            try
            {
                long BranchClassID = await _BranchClassContext.ClassMaintenance(BranchClassInfo);
                BranchClass.Data = BranchClassID;

            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return BranchClass;
        }

        

        public async Task<List<BranchClassEntity>> GetBranchClassByBranchClassID(long BranchClassID, long BranchID)
        {
            try
            {
                List<BranchClassEntity> BranchClass = new List<BranchClassEntity>();
                BranchClass = await _BranchClassContext.GetClassByClassID(BranchClassID,BranchID);                
                return BranchClass;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }


        public async Task<BranchClassEntity> GetPackaegBranchClassByID(long BranchClassID)
        {
            try
            {
               BranchClassEntity BranchClass = new BranchClassEntity();
                BranchClass = await _BranchClassContext.GetClassbyID(BranchClassID);
                return BranchClass;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<BranchClassEntity>> GetAllBranchClass(long BranchID=0,long ClassID=0)
        {
            try
            {
                return await this._BranchClassContext.GetAllClass(BranchID, ClassID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<BranchClassEntity>> GetMobileAllBranchClass(long BranchID = 0, long ClassID = 0)
        {
            try
            {
                return await this._BranchClassContext.GetMobileAllClass(BranchID, ClassID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemoveBranchClass(long BranchClassID,long BranchID, string lastupdatedby)
        {
            try
            {
                return this._BranchClassContext.RemoveClass(BranchClassID, BranchID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

        
    }
}
