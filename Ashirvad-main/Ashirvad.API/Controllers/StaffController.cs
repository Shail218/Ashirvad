using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [System.Web.Http.RoutePrefix("api/staff/v1")]
    [AshirvadAuthorization]
    public class StaffController : ApiController
    {
        private readonly IStaffService _staffService = null;
        public StaffController(IStaffService staffService)
        {
            this._staffService = staffService;
        }
        
        [Route("StaffMaintenance")]
        [HttpPost]
        public OperationResult<StaffEntity> StaffMaintenance(StaffEntity staffInfo)
        {
            staffInfo.Userrole = Enums.UserType.Staff;
            var data = this._staffService.StaffMaintenance(staffInfo);
            OperationResult<StaffEntity> result = new OperationResult<StaffEntity>();
            result.Completed = data.Result.Status;
            result.Message = data.Result.Message;
            if (data.Result.Status && data.Result.Data != null)
            {
                result.Data = (StaffEntity)data.Result.Data;
            }

            return result;
        }

        [Route("GetAllStaff")]
        [HttpPost]
        public OperationResult<List<StaffEntity>> GetAllStaff(long branchID)
        {
            var data = this._staffService.GetAllStaff(branchID);
            OperationResult<List<StaffEntity>> result = new OperationResult<List<StaffEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }


        [Route("RemoveStaff")]
        [HttpPost]
        public OperationResult<bool> RemoveStaff(long StaffID, string lastupdatedby)
        {
            var data = this._staffService.RemoveStaff(StaffID,lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = data.Status;
            result.Data = data.Status;
            result.Message = data.Message;
            return result;
        }
    }
}