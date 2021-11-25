using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Repo.Services.Area.Branch;
using Ashirvad.Repo.Services.Area.SuperAdminSubject;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Subject;
using Ashirvad.ServiceAPI.ServiceAPI.Area.SuperAdminSubject;
using Ashirvad.ServiceAPI.Services.Area;
using Ashirvad.ServiceAPI.Services.Area.SuperAdminSubject;
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

        public BranchSubjectController()
        {
            _subjectService = new SuperAdminSubjectService(new SuperAdminSubject(new BranchSubject()));
            _branchSubjectService = new BranchSubjectService(new BranchSubject());
        }

        [Route("BranchSubjectMaintenance")]
        [HttpPost]
        public OperationResult<BranchSubjectEntity> BranchSubjectMaintenance(BranchSubjectEntity branchClassEntity)
        {
            Task<BranchSubjectEntity> data = null;
            foreach (var item in branchClassEntity.BranchSubjectData)
            {
                data = this._branchSubjectService.BranchSubjectMaintenance(new BranchSubjectEntity()
                {
                    branch = item.branch,
                    BranchCourse = item.BranchCourse,
                    BranchClass = item.BranchClass,
                    Subject = item.Subject,
                    isSubject = item.isSubject,
                    RowStatus = item.RowStatus,
                    Transaction = item.Transaction
                });
            }
            OperationResult<BranchSubjectEntity> result = new OperationResult<BranchSubjectEntity>();
            result.Completed = false;
            result.Data = null;
            if ((long)data.Result.Data > 0)
            {
                result.Completed = true;
                result.Data = data.Result;
                if (branchClassEntity.BranchSubjectData[0].Subject_dtl_id > 0)
                {
                    result.Message = "Branch Subject Updated Successfully";
                }
                else
                {
                    result.Message = "Branch Subject Created Successfully";
                }
            }
            else
            {
                result.Message = "Branch Course Already Exists!!";
            }
            return result;
        }

        [Route("GetBranchSubjectByBranchSubjectID")]
        [HttpPost]
        public OperationResult<List<BranchSubjectEntity>> GetBranchSubjectByBranchSubjectID(long SubjectID, long BranchID, long ClassID)
        {
            var data = this._branchSubjectService.GetMobileBranchSubjectByBranchSubjectID(SubjectID, BranchID, ClassID);
            OperationResult<List<BranchSubjectEntity>> result = new OperationResult<List<BranchSubjectEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllBranchSubjectByBranchID")]
        [HttpPost]
        public OperationResult<List<BranchSubjectEntity>> GetAllBranchSubjectByBranchID(long branchID)
        {
            var data = this._branchSubjectService.GetMobileAllBranchSubject(branchID);
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