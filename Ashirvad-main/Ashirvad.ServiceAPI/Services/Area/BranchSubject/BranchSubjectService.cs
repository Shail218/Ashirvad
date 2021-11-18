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
    public class BranchSubjectService : IBranchSubjectService
    {
        private readonly IBranchSubjectAPI _BranchSubjectContext;        

        public BranchSubjectService(IBranchSubjectAPI BranchSubjectContext)
        {
            this._BranchSubjectContext = BranchSubjectContext;
        }

        public async Task<BranchSubjectEntity> BranchSubjectMaintenance(BranchSubjectEntity BranchSubjectInfo)
        {
            BranchSubjectEntity BranchSubject = new BranchSubjectEntity();
            try
            {
                long BranchSubjectID = await _BranchSubjectContext.SubjectMaintenance(BranchSubjectInfo);
                BranchSubject.Data = BranchSubjectID;

            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return BranchSubject;
        }

        

        public async Task<List<BranchSubjectEntity>> GetBranchSubjectByBranchSubjectID(long BranchSubjectID, long BranchID,long ClassID)
        {
            try
            {
                List<BranchSubjectEntity> BranchSubject = new List<BranchSubjectEntity>();
                BranchSubject = await _BranchSubjectContext.GetSubjectBySubjectID(BranchSubjectID,BranchID, ClassID);                
                return BranchSubject;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }


        public async Task<BranchSubjectEntity> GetPackaegBranchSubjectByID(long BranchSubjectID)
        {
            try
            {
               BranchSubjectEntity BranchSubject = new BranchSubjectEntity();
                BranchSubject = await _BranchSubjectContext.GetSubjectbyID(BranchSubjectID);
                return BranchSubject;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<BranchSubjectEntity>> GetAllBranchSubject(long BranchID=0)
        {
            try
            {
                return await this._BranchSubjectContext.GetAllSubject(BranchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemoveBranchSubject(long CourseID, long ClassID, long BranchID, string lastupdatedby)
        {
            try
            {
                return this._BranchSubjectContext.RemoveSubject(CourseID, ClassID, BranchID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

        
    }
}
