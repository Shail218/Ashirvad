using Ashirvad.API.Filter;
using Ashirvad.Data;
using Ashirvad.Repo.Services.Area.Announcement;
using Ashirvad.Repo.Services.Area.Notification;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Announcement;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Notification;
using Ashirvad.ServiceAPI.Services.Area.Announcement;
using Ashirvad.ServiceAPI.Services.Area.Notification;
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
        private readonly IAnnouncementService _announcementService = null;
        public NotificationController(INotificationService notifService, IAnnouncementService announcementService)
        {
            this._notificationService = notifService;
            this._announcementService = announcementService;
        }

        public NotificationController()
        {
            this._notificationService = new NotificationService(new Notification());
            this._announcementService = new AnnouncementService(new Announcement());
        }

        [Route("NotificationMaintenance")]
        [HttpPost]
        public OperationResult<NotificationEntity> NotificationMaintenance(NotificationEntity notifInfo)
        {
            if (notifInfo.stdID != "")
            {
                string[] stdname = notifInfo.stdID.Split(',');
                for (int i = 0; i < stdname.Length; i++)
                {
                    notifInfo.Standardlist.Add(new StandardEntity()
                    {
                        StandardID = long.Parse(stdname[i])
                    });
                }
            }
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
        [Route("GetMobileNotification")]
        [HttpGet]
        public OperationResult<List<NotificationEntity>> GetMobileNotification(long branchID)
        {
            var data = this._notificationService.GetMobileNotification(branchID);
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

        [Route("GetAllAnnouncement")]
        [HttpPost]
        public OperationResult<List<AnnouncementEntity>> GetAllAnnouncement(long branchID)
        {
            var branchData = this._announcementService.GetAllAnnouncement(branchID);
            OperationResult<List<AnnouncementEntity>> result = new OperationResult<List<AnnouncementEntity>>();
            result.Completed = true;
            result = branchData.Result;
            return result;
        }
    }
}
