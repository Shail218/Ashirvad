using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Notification;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class NotificationController : BaseController
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // GET: Notification
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> NotificationMaintenance(long notificationID)
        {
            NotificationMaintenanceModel branch = new NotificationMaintenanceModel();
            if (notificationID > 0)
            {
                var result = await _notificationService.GetNotificationByNotificationID(notificationID);
                branch.NotificationInfo = result.Data;
            }
            else
            {
                branch.NotificationInfo = new NotificationEntity();
            }

            var branchData = await _notificationService.GetAllNotification(0, 0);
            //var branchData = await _notificationService.GetAllNotification(SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID, SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : (int)SessionContext.Instance.LoginUser.UserType);
            branch.NotificationData = branchData.Data;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveNotification(NotificationEntity notification)
        {
            if(notification.Branch.BranchID == 1)
            {
                notification.Branch.BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
            }
            var nt = JsonConvert.DeserializeObject<List<NotificationTypeEntity>>(notification.JSONData);
            notification.NotificationType = nt;
            notification.Transaction = GetTransactionData(notification.NotificationID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            notification.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _notificationService.NotificationMaintenance(notification);
            if (data != null)
            {
                return Json(true);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult RemoveNotification(long notificationID)
        {
            var result = _notificationService.RemoveNotification(notificationID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

    }
}