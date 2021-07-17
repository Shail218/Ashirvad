using Ashirvad.API.Filter;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/notification/v1")]
    [AshirvadAuthorization]
    public class NotificationController : ApiController
    {
        private readonly INotificationService _notificationService = null;
        public NotificationController(INotificationService notifService)
        {
            this._notificationService = notifService;
        }

        [Route("NotificationMaintenance")]
        [HttpPost]
        public OperationResult<NotificationEntity> NotificationMaintenance(NotificationEntity notifInfo)
        {
            var data = this._notificationService.NotificationMaintenance(notifInfo);
            OperationResult<NotificationEntity> result = new OperationResult<NotificationEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllNotification")]
        [HttpGet]
        public OperationResult<List<NotificationEntity>> GetAllNotification()
        {
            var data = this._notificationService.GetAllNotification();
            OperationResult<List<NotificationEntity>> result = new OperationResult<List<NotificationEntity>>();
            result = data.Result;
            return result;
        }

        [Route("GetAllNotificationByBranch")]
        [HttpGet]
        public OperationResult<List<NotificationEntity>> GetAllNotification(long branchID)
        {
            var data = this._notificationService.GetAllNotification(branchID);
            OperationResult<List<NotificationEntity>> result = new OperationResult<List<NotificationEntity>>();
            result = data.Result;
            return result;
        }

        [Route("GetAllNotificationByBranchAndType")]
        [HttpGet]
        public OperationResult<List<NotificationEntity>> GetAllNotification(long branchID, int typeID)
        {
            var data = this._notificationService.GetAllNotification(branchID, typeID);
            OperationResult<List<NotificationEntity>> result = new OperationResult<List<NotificationEntity>>();
            result = data.Result;
            return result;
        }


        [Route("GetNotificationByID")]
        [HttpPost]
        public OperationResult<NotificationEntity> GetNotificationByID(long notifID)
        {
            var data = this._notificationService.GetNotificationByNotificationID(notifID);
            OperationResult<NotificationEntity> result = new OperationResult<NotificationEntity>();
            result = data.Result;
            return result;
        }

        [Route("RemoveNotification")]
        [HttpPost]
        public OperationResult<bool> RemoveNotification(long notifID, string lastupdatedby)
        {
            var data = this._notificationService.RemoveNotification(notifID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }
    }
}
