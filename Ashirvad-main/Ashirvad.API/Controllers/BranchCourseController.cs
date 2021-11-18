using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Class;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Course;
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

        [Route("BranchCourseMaintenance")]
        [HttpPost]
        public OperationResult<BranchCourseEntity> BranchCourseMaintenance(BranchCourseEntity branchClassEntity)
        {
            var data = this._branchcourseService.BranchCourseMaintenance(branchClassEntity);
            OperationResult<BranchCourseEntity> result = new OperationResult<BranchCourseEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetBranchCourseByBranchCourseID")]
        [HttpPost]
        public OperationResult<List<BranchCourseEntity>> GetBranchCourseByBranchCourseID(long ClassID)
        {
            var data = this._branchcourseService.GetBranchCourseByBranchCourseID(ClassID);
            OperationResult<List<BranchCourseEntity>> result = new OperationResult<List<BranchCourseEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllBranchCourse")]
        [HttpPost]
        public OperationResult<List<BranchCourseEntity>> GetAllBranchCourse(long branchID)
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

        [Route("RemoveClassDetail")]
        [HttpPost]
        public OperationResult<bool> RemoveClassDetail(long BranchCourseID, string lastupdatedby)
        {
            var data = _branchcourseService.RemoveBranchCourse(BranchCourseID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetAllBranchCourse")]
        [HttpPost]
        public OperationResult<List<BranchCourseEntity>> GetAllBranchCourse(long BranchID, long CourseID)
        {
            var data = _branchcourseService.GetAllBranchCourse(BranchID);
            OperationResult<List<BranchCourseEntity>> result = new OperationResult<List<BranchCourseEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }
    }
}