using Ashirvad.API.Filter;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Homework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/homework/v1")]
    [AshirvadAuthorization]
    public class HomeworkController : ApiController
    {
        private readonly IHomeworkService _homeworkService = null;
        private readonly IHomeworkDetailService _homeworkdetailService = null;
        public HomeworkController(IHomeworkService homeworkService ,IHomeworkDetailService homeworkdetailService)
        {
            _homeworkService = homeworkService;
            _homeworkdetailService = homeworkdetailService;
        }


        [Route("HomeworkMaintenance")]
        [HttpPost]
        public OperationResult<HomeworkEntity> HomeworkMaintenance(HomeworkEntity homework)
        {
            OperationResult<HomeworkEntity> result = new OperationResult<HomeworkEntity>();

            var data = this._homeworkService.HomeworkMaintenance(homework);
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllHomeworkByBranch")]
        [HttpGet]
        public OperationResult<List<HomeworkEntity>> GetAllHomeworkByBranch(long branchID)
        {
            var data = this._homeworkService.GetAllHomeworkByBranch(branchID);
            OperationResult<List<HomeworkEntity>> result = new OperationResult<List<HomeworkEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetAllHomeworkByBranchAndStd")]
        [HttpGet]
        public OperationResult<List<HomeworkEntity>> GetAllHomeworkByBranch(long branchID, long stdID, int batchTime)
        {
            var data = this._homeworkService.GetAllHomeworkByBranch(branchID, stdID, batchTime);
            OperationResult<List<HomeworkEntity>> result = new OperationResult<List<HomeworkEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetAllHomeworkWithoutContentByBranch")]
        [HttpGet]
        public OperationResult<List<HomeworkEntity>> GetAllHomeworkWithoutContentByBranch(long branchID)
        {
            var data = this._homeworkService.GetAllHomeworkWithoutContentByBranch(branchID);
            OperationResult<List<HomeworkEntity>> result = new OperationResult<List<HomeworkEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetAllHomeworkWithoutContentByBranchSTD")]
        [HttpGet]
        public OperationResult<List<HomeworkEntity>> GetAllHomeworkWithoutContentByBranch(long branchID, long stdID)
        {
            var data = this._homeworkService.GetAllHomeworkWithoutContentByBranch(branchID, stdID);
            OperationResult<List<HomeworkEntity>> result = new OperationResult<List<HomeworkEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetAllHomeworks")]
        [HttpGet]
        public OperationResult<List<HomeworkEntity>> GetAllHomeworks(DateTime hwDate, string searchParam)
        {
            var data = this._homeworkService.GetAllHomeworks(hwDate, searchParam);
            OperationResult<List<HomeworkEntity>> result = new OperationResult<List<HomeworkEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetHomeworkByHWID")]
        [HttpGet]
        public OperationResult<HomeworkEntity> GetHomeworkByHWID(long hwID)
        {
            var data = this._homeworkService.GetHomeworkByHomeworkID(hwID);
            OperationResult<HomeworkEntity> result = new OperationResult<HomeworkEntity>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }


        [Route("RemoveHomework")]
        [HttpPost]
        public OperationResult<bool> RemoveHomework(long hwID, string lastupdatedby)
        {
            var data = this._homeworkService.RemoveHomework(hwID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetAllHomeworks")]
        [HttpGet]
        public OperationResult<List<HomeworkDetailEntity>> GetAllHomeworks(long StudID)
        {
            var data = this._homeworkdetailService.GetAllHomeworkdetailByHomeWork(StudID);
            OperationResult<List<HomeworkDetailEntity>> result = new OperationResult<List<HomeworkDetailEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }
        
    }
}
