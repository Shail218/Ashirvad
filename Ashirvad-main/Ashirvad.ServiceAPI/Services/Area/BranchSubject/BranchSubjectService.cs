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

        public async Task<List<BranchSubjectEntity>> GetSubjectDDL(long courseid, long branchid,string std)
        {
            try
            {
                List<BranchSubjectEntity> BranchSubject = new List<BranchSubjectEntity>();
                BranchSubject = await _BranchSubjectContext.GetSubjectDDL(courseid,branchid,std);
                return BranchSubject;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<BranchSubjectEntity>> GetMobileBranchSubjectByBranchSubjectID(long BranchSubjectID, long BranchID,long ClassID)
        {
            try
            {
                List<BranchSubjectEntity> BranchSubject = new List<BranchSubjectEntity>();
                BranchSubject = await _BranchSubjectContext.GetMobileSubjectBySubjectID(BranchSubjectID,BranchID, ClassID);                
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

        public async Task<List<BranchSubjectEntity>> GetAllSubjects(DataTableAjaxPostModel model, long BranchID)
        {
            try
            {
                return await this._BranchSubjectContext.GetAllSubjects(model, BranchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }

        public async Task<List<BranchSubjectEntity>> GetMobileAllBranchSubject(long BranchID=0)
        {
            try
            {
                return await this._BranchSubjectContext.GetMobileAllSubject(BranchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveBranchSubject(long CourseID, long ClassID, long BranchID, string lastupdatedby)
        {
            try
            {
                return this._BranchSubjectContext.RemoveSubject(CourseID, ClassID, BranchID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public Task<List<BranchSubjectEntity>> GetMobileAllSubject(long BranchID = 0)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BranchSubjectEntity>> GetSubjectByclasscourseid(long SubjectID, long BranchID, long CourseID)
        {
            try
            {
                List<BranchSubjectEntity> BranchSubject = new List<BranchSubjectEntity>();
                BranchSubject = await _BranchSubjectContext.GetSubjectByclasscourseid(SubjectID, BranchID, CourseID);
                return BranchSubject;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
    }
}
