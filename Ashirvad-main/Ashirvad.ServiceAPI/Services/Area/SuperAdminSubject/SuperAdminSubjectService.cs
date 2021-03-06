using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.SuperAdminSubject;
using Ashirvad.ServiceAPI.ServiceAPI.Area.SuperAdminSubject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area.SuperAdminSubject
{
    public class SuperAdminSubjectService : ISuperAdminSubjectService
    {
        private readonly ISuperAdminSubjectAPI _subjectService;

        public SuperAdminSubjectService(ISuperAdminSubjectAPI subjectService)
        {
            this._subjectService = subjectService;
        }

        public async Task<ResponseModel> SubjectMaintenance(SuperAdminSubjectEntity subjectEntity)
        {
            ResponseModel responseModel = new ResponseModel();
            SuperAdminSubjectEntity cl = new SuperAdminSubjectEntity();
            try
            {
                responseModel = await _subjectService.SubjectMaintenance(subjectEntity);
                //long subjectID = await _subjectService.SubjectMaintenance(subjectEntity);
                //cl.SubjectID = subjectID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
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

        public async Task<OperationResult<List<SuperAdminSubjectEntity>>> GetAllSubjectByCourseClass(long courseid, long classid)
        {
            try
            {
                OperationResult<List<SuperAdminSubjectEntity>> course = new OperationResult<List<SuperAdminSubjectEntity>>();
                course.Data = await _subjectService.GetAllSubjectByCourseClass(courseid,classid);
                course.Completed = true;
                return course;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<List<SuperAdminSubjectEntity>>> GetAllCustomSubject(DataTableAjaxPostModel model)
        {
            try
            {
                OperationResult<List<SuperAdminSubjectEntity>> course = new OperationResult<List<SuperAdminSubjectEntity>>();
                course.Data = await _subjectService.GetAllCustomSubject(model);
                course.Completed = true;
                return course;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveSubject(long subjectID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            { 
                responseModel = this._subjectService.RemoveSubject(subjectID, lastupdatedby);
                //return this._subjectService.RemoveSubject(subjectID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<List<BranchSubjectEntity>> GetAllSubjectByCourseClassddl(long courseid, long classid, bool Isupdate = false)
        {
            try
            {
                List< BranchSubjectEntity> course = new List<BranchSubjectEntity>();
                course = await _subjectService.GetAllSubjectByCourseClassddl(courseid, classid);                
                return course;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
    }
}
