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
using static Ashirvad.Common.Common;

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
            //var data = await _notificationService.GetAllNotification(SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : 
            //    SessionContext.Instance.LoginUser.BranchInfo.BranchID, SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : 
            //    (int)SessionContext.Instance.LoginUser.UserType);
            branch.NotificationData = new List<NotificationEntity>();
            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveNotification(NotificationEntity notification)
        {
            if (SessionContext.Instance.LoginUser.UserType == Enums.UserType.Admin)
            {
                notification.Branch.BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
            }
            else
            {
                if (notification.BranchType == 1 && notification.NotificationID == 0)
                {
                    notification.Branch = new BranchEntity()
                    {
                        BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID
                    };
                }
                else if (notification.BranchType == 0)
                {
                    notification.Branch = new BranchEntity()
                    {
                        BranchID = 0
                    };
                }
                else if (notification.BranchType == 1 && notification.Branch.BranchID == 0)
                {
                    notification.Branch = new BranchEntity()
                    {
                        BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID
                    };
                }
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

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            List<string> columns = new List<string>();
            columns.Add("Notification_Date");
            columns.Add("NotificationMessage");
            foreach (var item in model.order)
            {
                item.name = columns[item.column];
            }
            var branchData = await _notificationService.GetAllCustomNotification(model, SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 :
                SessionContext.Instance.LoginUser.BranchInfo.BranchID, SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 :
                (int)SessionContext.Instance.LoginUser.UserType);
            long total = 0;
            if (branchData.Count > 0)
            {
                total = branchData[0].Count;
            }
            return Json(new
            {
                draw = model.draw,
                iTotalRecords = total,
                iTotalDisplayRecords = total,
                data = branchData
            });

        }
        [HttpPost]
        public async Task<ActionResult> GetExportData(string Search)
        {
            var branchData = await _notificationService.GetAllNotificationforexcel(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            return View("~/Views/Notification/_Export_Notification.cshtml", branchData.Data);
        }
    }
}