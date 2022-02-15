using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Repo.Services.Area;
using Ashirvad.Repo.Services.Area.Branch;
using Ashirvad.Repo.Services.Area.Subject;
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
            _subjectService = new SuperAdminSubjectService(new SuperAdminSubject(new BranchSubject(),new Subject()));
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
                    Subject_dtl_id = item.Subject_dtl_id,
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
                result.Message = "Branch Subject Already Exists!!";
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
        public OperationResult<ResponseModel> RemoveSubjectDetail(long CourseID, long ClassID, long branchID, string lastupdatedby)
        {
            var data = _branchSubjectService.RemoveBranchSubject(CourseID, ClassID, branchID, lastupdatedby);
            if (data.Status == false)
            {
                data.Message = data.Message.Replace("<br />", "\n");
            }
            OperationResult<ResponseModel> result = new OperationResult<ResponseModel>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetAllSubjectDDL")]
        [HttpGet]
        public OperationResult<List<BranchSubjectEntity>> GetAllSubjectDDL(long ClassID,long BranchID, long CourseID)
        {
            var data = this._branchSubjectService.GetBranchSubjectByBranchSubjectID(ClassID,BranchID, CourseID);
            OperationResult<List<BranchSubjectEntity>> result = new OperationResult<List<BranchSubjectEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetLibrarySubjectDDL")]
        [HttpGet]
        public OperationResult<List<BranchSubjectEntity>> GetLibrarySubjectDDL(long CourseID,long BranchID,string ClassID)
        {
            var data = this._branchSubjectService.GetSubjectDDL(CourseID,BranchID,ClassID);
            OperationResult<List<BranchSubjectEntity>> result = new OperationResult<List<BranchSubjectEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllSubjectByCourse")]
        [HttpGet]
        public OperationResult<List<BranchSubjectEntity>> GetAllSubjectByCourse(long courseid,long classid)
        {
            var data = this._subjectService.GetAllSubjectByCourseClassddl(courseid,classid);
            OperationResult<List<BranchSubjectEntity>> result = new OperationResult<List<BranchSubjectEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("Check_SubjectDetail")]
        [HttpGet]
        public OperationResult<ResponseModel> Check_SubjectDetail(long Branchid, long subjectdetailis)
        {
            Check_Delete subject = new Check_Delete();
            var data = subject.check_delete_subject(Branchid, subjectdetailis);
            if (data.Result.Status == false)
            {
                data.Result.Message = data.Result.Message.Replace("<br />", "\n");
            }
            OperationResult<ResponseModel> result = new OperationResult<ResponseModel>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }
    }
}