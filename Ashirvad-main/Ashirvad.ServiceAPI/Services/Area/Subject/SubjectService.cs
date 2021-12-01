using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Subject;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.Subject
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectAPI _subjectContext;
        public SubjectService(ISubjectAPI subjectContext)
        {
            this._subjectContext = subjectContext;
        }

        public async Task<SubjectEntity> SubjectMaintenance(SubjectEntity subjectInfo)
        {
            SubjectEntity standard = new SubjectEntity();
            try
            {
                long subjectID = await _subjectContext.SubjectMaintenance(subjectInfo);
                standard.SubjectID = subjectID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return standard;
        }

        public async Task<List<SubjectEntity>> GetAllSubjects(long branchID)
        {
            try
            {
                return await this._subjectContext.GetAllSubjects(branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<SubjectEntity>> GetAllSubjectsName(long branchid)
        {
            try
            {
                return await this._subjectContext.GetAllSubjectsName(branchid);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<SubjectEntity>> GetAllSubjectsID(string subjectName, long branchid)
        {
            try
            {
                return await this._subjectContext.GetAllSubjectsID(subjectName,branchid);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<SubjectEntity>> GetAllSubjectsByTestDate(string TestDate)
        {
            try
            {
                return await this._subjectContext.GetAllSubjectsByTestDate(TestDate);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        public async Task<List<SubjectEntity>> GetAllSubjects()
        {
            try
            {
                return await this._subjectContext.GetAllSubjects();
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemoveSubject(long SubjectID, string lastupdatedby)
        {
            try
            {
                return this._subjectContext.RemoveSubject(SubjectID,lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

        public async Task<SubjectEntity> GetSubjectByIDAsync(long subjectID)
        {
            try
            {
                return await this._subjectContext.GetSubjectByID(subjectID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
    }
}
