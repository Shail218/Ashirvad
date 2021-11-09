using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.SuperAdminSubject;
using Ashirvad.ServiceAPI.ServiceAPI.Area.SuperAdminSubject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.SuperAdminSubject
{
    public class SuperAdminSubjectService : ISuperAdminSubjectService
    {
        private readonly ISuperAdminSubjectAPI _subjectService;

        public SuperAdminSubjectService(ISuperAdminSubjectAPI subjectService)
        {
            this._subjectService = subjectService;
        }

        public async Task<SuperAdminSubjectEntity> SubjectMaintenance(SuperAdminSubjectEntity subjectEntity)
        {
            SuperAdminSubjectEntity cl = new SuperAdminSubjectEntity();
            try
            {
                long subjectID = await _subjectService.SubjectMaintenance(subjectEntity);
                cl.SubjectID = subjectID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return cl;
        }

        public async Task<OperationResult<SuperAdminSubjectEntity>> GetSubjectBySubjectID(long subjectID)
        {
            try
            {
                OperationResult<SuperAdminSubjectEntity> course = new OperationResult<SuperAdminSubjectEntity>();
                course.Data = await _subjectService.GetSubjectBySubjectID(subjectID);
                course.Completed = true;
                return course;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<List<SuperAdminSubjectEntity>>> GetAllSubject()
        {
            try
            {
                OperationResult<List<SuperAdminSubjectEntity>> course = new OperationResult<List<SuperAdminSubjectEntity>>();
                course.Data = await _subjectService.GetAllSubject();
                course.Completed = true;
                return course;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemoveSubject(long subjectID, string lastupdatedby)
        {
            try
            {
                return this._subjectService.RemoveSubject(subjectID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }
    }
}
