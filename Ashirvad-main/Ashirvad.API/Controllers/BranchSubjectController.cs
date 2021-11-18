using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Subject;
using Ashirvad.ServiceAPI.ServiceAPI.Area.SuperAdminSubject;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Ashirvad.Web.Controllers
{
    [RoutePrefix("api/branchsubject/v1")]
    public class BranchSubjectController : ApiController
    {
        BranchSubjectMaintenanceModel branchSubject = new BranchSubjectMaintenanceModel();

        private readonly ISuperAdminSubjectService _subjectService;
        private readonly IBranchSubjectService _branchSubjectService;
        public BranchSubjectController(ISuperAdminSubjectService SubjectService, IBranchSubjectService branchSubjectService)
        {
            _subjectService = SubjectService;
            _branchSubjectService = branchSubjectService;
        }

        [Route("BranchSubjectMaintenance")]
        [HttpPost]
        public OperationResult<BranchSubjectEntity> BranchSubjectMaintenance(BranchSubjectEntity branchClassEntity)
        {
            var data = this._branchSubjectService.BranchSubjectMaintenance(branchClassEntity);
            OperationResult<BranchSubjectEntity> result = new OperationResult<BranchSubjectEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetBranchSubjectByBranchSubjectID")]
        [HttpPost]
        public OperationResult<List<BranchSubjectEntity>> GetBranchSubjectByBranchSubjectID(long SubjectID, long BranchID, long ClassID)
        {
            var data = this._branchSubjectService.GetBranchSubjectByBranchSubjectID(SubjectID, BranchID, ClassID);
            OperationResult<List<BranchSubjectEntity>> result = new OperationResult<List<BranchSubjectEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllBranchSubject")]
        [HttpPost]
        public OperationResult<List<BranchSubjectEntity>> GetAllBranchSubject(long branchID)
        {
            var data = this._branchSubjectService.GetAllBranchSubject(branchID);
            OperationResult<List<BranchSubjectEntity>> result = new OperationResult<List<BranchSubjectEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllSubject")]
        [HttpGet]
        public OperationResult<List<SuperAdminSubjectEntity>> GetAllSubject()
        {
            var data = this._subjectService.GetAllSubject();
            OperationResult<List<SuperAdminSubjectEntity>> result = new OperationResult<List<SuperAdminSubjectEntity>>();
            result.Completed = true;
            result.Data = data.Result.Data;
            return result;
        }

        [Route("RemoveSubjectDetail")]
        [HttpPost]
        public OperationResult<bool> RemoveSubjectDetail(long CourseID, long ClassID, long branchID, string lastupdatedby)
        {
            var data = _branchSubjectService.RemoveBranchSubject(CourseID, ClassID, branchID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

    }
}