using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.Services.Area.Branch;
using Ashirvad.Repo.Services.Area.Staff;
using Ashirvad.Repo.Services.Area.User;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Staff;
using Ashirvad.ServiceAPI.ServiceAPI.Area.User;
using Ashirvad.ServiceAPI.Services.Area.Staff;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ashirvad.Web.Controllers
{
    [RoutePrefix("api/profile/v1")]
    public class ProfileController : ApiController
    {
        private readonly IStaffService _staffService;
        public ResponseModel res = new ResponseModel();
        public ProfileController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        public ProfileController()
        {
            _staffService = new StaffService(new Staff(), new User(), new Branch());
        }

        [Route("GetStaffByID")]
        [HttpPost]
        public OperationResult<StaffEntity> GetStaffByID(long StaffID)
        {
            var data = _staffService.GetStaffByID(StaffID);
            OperationResult<StaffEntity> result = new OperationResult<StaffEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("UpdateProfile")]
        [HttpPost]
        public OperationResult<ResponseModel> UpdateProfile(StaffEntity branch)
        {
            var data = _staffService.UpdateProfile(branch);
            OperationResult<ResponseModel> result = new OperationResult<ResponseModel>();
            result.Completed = data.Result.Status;
            result.Message = data.Result.Message;
            result.Data = data.Result;
            return result;
        }
    }
}