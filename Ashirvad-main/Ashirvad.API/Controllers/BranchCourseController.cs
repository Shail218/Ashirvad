using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Repo.Services.Area.Branch;
using Ashirvad.Repo.Services.Area.Course;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Class;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Course;
using Ashirvad.ServiceAPI.Services.Area;
using Ashirvad.ServiceAPI.Services.Area.Course;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Ashirvad.Web.Controllers
{
    [RoutePrefix("api/branchcource/v1")]
    public class BranchCourseController : ApiController
    {
        private readonly ICourseService _courseService;
        private readonly IBranchCourseService _branchcourseService;
        public BranchCourseController(ICourseService courseService, IBranchCourseService branchCourseService)
        {
            _courseService = courseService;
            _branchcourseService = branchCourseService;
        }
        public BranchCourseController()
        {
            _courseService = new CourseService(new Course(new BranchCourse()));
            _branchcourseService = new BranchCourseService(new BranchCourse()); ;
        }

        [Route("BranchCourseMaintenance")]
        [HttpPost]
        public OperationResult<BranchCourseEntity> BranchCourseMaintenance(BranchCourseEntity branchClassEntity)
        {
            Task<BranchCourseEntity> data = null;
            Task<ResponseModel> responseModel = null;
            foreach (var item in branchClassEntity.BranchCourseData)
            {
                responseModel = this._branchcourseService.BranchCourseMaintenance(new BranchCourseEntity()
                {
                    branch = item.branch,
                    course = new CourseEntity()
                    {
                        CourseID = item.course.CourseID
                    },
                    course_dtl_id = item.course_dtl_id,
                    iscourse = item.iscourse,
                    RowStatus = new RowStatusEntity
                    {
                        RowStatusId = 1,
                        RowStatus = Enums.RowStatus.Active
                    },
                    Transaction = item.Transaction
                });
            }
             OperationResult<BranchCourseEntity> result = new OperationResult<BranchCourseEntity>();
            result.Completed = responseModel.Result.Status;
            result.Message = responseModel.Result.Message;
            if (responseModel.Result.Status && responseModel.Result.Data != null)
            {
                result.Data = (BranchCourseEntity)responseModel.Result.Data;
            }
            return result;
        }

        [Route("GetBranchCourseByBranchCourseID")]
        [HttpPost]
        public OperationResult<List<BranchCourseEntity>> GetBranchCourseByBranchCourseID(long BranchCourseID)
        {
            var data = this._branchcourseService.GetBranchCourseByBranchCourseID(BranchCourseID);
            OperationResult<List<BranchCourseEntity>> result = new OperationResult<List<BranchCourseEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllBranchCourseByBranchID")]
        [HttpPost]
        public OperationResult<List<BranchCourseEntity>> GetAllBranchCourseByBranchID(long branchID)
        {
            var data = this._branchcourseService.GetAllBranchCourse(branchID);
            OperationResult<List<BranchCourseEntity>> result = new OperationResult<List<BranchCourseEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllCourse")]
        [HttpGet]
        public OperationResult<List<CourseEntity>> GetAllCourse()
        {
            var data = this._courseService.GetAllCourse();
            OperationResult<List<CourseEntity>> result = new OperationResult<List<CourseEntity>>();
            result.Completed = true;
            result.Data = data.Result.Data;
            return result;
        }

        [Route("RemoveBranchCourse")]
        [HttpPost]
        public OperationResult<ResponseModel> RemoveBranchCourse(long BranchCourseID, string lastupdatedby)
        {
            var data = _branchcourseService.RemoveBranchCourse(BranchCourseID, lastupdatedby);
            OperationResult<ResponseModel> result = new OperationResult<ResponseModel>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetAllCourseDDL")]
        [HttpGet]
        public OperationResult<List<BranchCourseEntity>> GetAllCourseDDL(long BranchID)
        {
            var data = this._branchcourseService.GetAllBranchCourse(BranchID);
            OperationResult<List<BranchCourseEntity>> result = new OperationResult<List<BranchCourseEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

    }
}