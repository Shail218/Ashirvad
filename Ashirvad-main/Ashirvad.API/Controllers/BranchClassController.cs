using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Repo.Services.Area.Branch;
using Ashirvad.Repo.Services.Area.Class;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Class;
using Ashirvad.ServiceAPI.Services.Area;
using Ashirvad.ServiceAPI.Services.Area.Class;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Ashirvad.Web.Controllers
{
    [RoutePrefix("api/branchclass/v1")]
    public class BranchClassController : ApiController
    {
        private readonly IClassService _ClassService;
        private readonly IBranchClassService _branchClassService;
        public BranchClassController(IClassService ClassService, IBranchClassService branchClassService)
        {
            _ClassService = ClassService;
            _branchClassService = branchClassService;
        }

        public BranchClassController()
        {
            _ClassService = new ClassService(new Class(new BranchClass()));
            _branchClassService = new BranchClassService(new BranchClass());
        }

        [Route("BranchClassMaintenance")]
        [HttpPost]
        public OperationResult<BranchClassEntity> BranchClassMaintenance(BranchClassEntity branchClassEntity)
        {
            var data = this._branchClassService.BranchClassMaintenance(branchClassEntity);
            OperationResult<BranchClassEntity> result = new OperationResult<BranchClassEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetBranchClassByBranchClassID")]
        [HttpPost]
        public OperationResult<List<BranchClassEntity>> GetBranchClassByBranchClassID(long ClassID, long branchID)
        {
            var data = this._branchClassService.GetBranchClassByBranchClassID(ClassID, branchID);
            OperationResult<List<BranchClassEntity>> result = new OperationResult<List<BranchClassEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllBranchClassByBranchID")]
        [HttpPost]
        public OperationResult<List<BranchClassEntity>> GetAllBranchClassByBranchID(long BranchID, long CourseID = 0)
        {
            var data = this._branchClassService.GetMobileAllBranchClass(BranchID, CourseID);
            OperationResult<List<BranchClassEntity>> result = new OperationResult<List<BranchClassEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllClass")]
        [HttpGet]
        public OperationResult<List<ClassEntity>> GetAllClass()
        {
            var data = this._ClassService.GetAllClass();
            OperationResult<List<ClassEntity>> result = new OperationResult<List<ClassEntity>>();
            result.Completed = true;
            result.Data = data.Result.Data;
            return result;
        }

        [Route("RemoveClassDetail")]
        [HttpPost]
        public OperationResult<bool> RemoveClassDetail(long ClassID, long BranchID, string lastupdatedby)
        {
            var data = _branchClassService.RemoveBranchClass(ClassID, BranchID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

    }
}