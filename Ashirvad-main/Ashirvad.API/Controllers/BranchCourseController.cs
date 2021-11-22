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
            foreach (var item in branchClassEntity.CourseData)
            {
                data = this._branchcourseService.BranchCourseMaintenance(new BranchCourseEntity()
                {
                    branch = branchClassEntity.branch,
                    course = new CourseEntity()
                    {
                        CourseID = item.CourseID
                    },
                    course_dtl_id = branchClassEntity.course_dtl_id,
                    iscourse = branchClassEntity.iscourse,
                    RowStatus = branchClassEntity.RowStatus,
                    Transaction = branchClassEntity.Transaction
                });
            }
            OperationResult<BranchCourseEntity> result = new OperationResult<BranchCourseEntity>();
            result.Completed = true;
            result.Data = data.Result;
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
        public OperationResult<bool> RemoveBranchCourse(long BranchCourseID, string lastupdatedby)
        {
            var data = _branchcourseService.RemoveBranchCourse(BranchCourseID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

    }
}