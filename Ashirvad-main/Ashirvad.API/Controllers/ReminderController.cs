using Ashirvad.API.Filter;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Reminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/reminder/v1")]
    [AshirvadAuthorization]
    public class ReminderController : ApiController
    {
        private readonly IReminderService _reminderService = null;
        public ReminderController(IReminderService reminderService)
        {
            this._reminderService = reminderService;
        }

        [Route("ReminderMaintenance")]
        [HttpPost]
        public OperationResult<ReminderEntity> ReminderMaintenance(ReminderEntity reminderInfo)
        {
            var data = this._reminderService.ReminderMaintenance(reminderInfo);
            OperationResult<ReminderEntity> result = new OperationResult<ReminderEntity>();
            result.Completed = data.Result.Status;
            if (data.Result.Status)
            {
                result.Data = (ReminderEntity)data.Result.Data;
            }
            result.Message = data.Result.Message;
            return result;
        }

        [Route("GetAllReminderByBranch")]
        [HttpGet]
        public OperationResult<List<ReminderEntity>> GetAllReminder(long branchID)
        {
            var data = this._reminderService.GetAllReminderByBranch(branchID);
            OperationResult<List<ReminderEntity>> result = new OperationResult<List<ReminderEntity>>();
            result = data.Result;
            return result;
        }

        [Route("GetAllReminderByBranchAndUser")]
        [HttpGet]
        public OperationResult<List<ReminderEntity>> GetAllReminder(long branchID, long userID)
        {
            var data = this._reminderService.GetAllReminderByBranch(branchID, userID);
            OperationResult<List<ReminderEntity>> result = new OperationResult<List<ReminderEntity>>();
            result = data.Result;
            return result;
        }


        [Route("GetReminderByID")]
        [HttpPost]
        public OperationResult<ReminderEntity> GetReminderByID(long reminderID)
        {
            var data = this._reminderService.GetReminderByReminderID(reminderID);
            OperationResult<ReminderEntity> result = new OperationResult<ReminderEntity>();
            result = data.Result;
            return result;
        }

        [Route("RemoveReminder")]
        [HttpPost]
        public OperationResult<bool> RemoveReminder(long reminderID, string lastupdatedby)
        {
            var data = this._reminderService.RemoveReminder(reminderID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = data.Status;
            result.Data = data.Status;
            result.Message = data.Message;
            return result;
        }
    }
}
