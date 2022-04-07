using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [System.Web.Http.RoutePrefix("api/subject/v1")]
    [AshirvadAuthorization]
    public class SubjectController : ApiController
    {
        private readonly ISubjectService _subjectService = null;
        public SubjectController(ISubjectService subjectService)
        {
            this._subjectService = subjectService;
        }

        [Route("SubjectMaintenance")]
        [HttpPost]
        public OperationResult<SubjectEntity> SubjectMaintenance(SubjectEntity subjectInfo)
        {
            var data = this._subjectService.SubjectMaintenance(subjectInfo);
            OperationResult<SubjectEntity> result = new OperationResult<SubjectEntity>();
            result.Completed = data.Result.Status;
            if (data.Result.Status)
            {
                result.Data = (SubjectEntity)data.Result.Data;
            }
            result.Message = data.Result.Message;
            return result;
        }

        [Route("GetAllSubjects")]
        [HttpPost]
        public OperationResult<List<SubjectEntity>> GetAllSubjects(long branchID)
        {
            var data = this._subjectService.GetAllSubjects(branchID);
            OperationResult<List<SubjectEntity>> result = new OperationResult<List<SubjectEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("RemoveSubject")]
        [HttpPost]
        public OperationResult<bool> RemoveSubject(long SubjectID, string lastupdatedby)
        {
            var data = this._subjectService.RemoveSubject(SubjectID,lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = data.Status;
            result.Data = data.Status;
            result.Message = data.Message;
            return result;
        }

        [Route("GetAllSubjectsByTestDate")]
        [HttpGet]
        public OperationResult<List<SubjectEntity>> GetAllSubjectsByTestDate(string TestDate, long BranchID)
        {
            var data = this._subjectService.GetAllSubjectsByTestDate(TestDate,BranchID);
            OperationResult<List<SubjectEntity>> result = new OperationResult<List<SubjectEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }
    }
}
