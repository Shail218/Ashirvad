using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Reminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class ReminderController : BaseController
    {
        private readonly IReminderService _reminderService = null;
        public ReminderController(IReminderService reminderService)
        {
            _reminderService = reminderService;
        }

        // GET: ReminderEntry
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ReminderMaintenance(long reminderID)
        {
            ReminderMaintenanceModel branch = new ReminderMaintenanceModel();
            if (reminderID > 0)
            {
                var reminder = await _reminderService.GetReminderByReminderID(reminderID);
                branch.ReminderInfo = reminder.Data;
            }

            var reminderData = await _reminderService.GetAllReminderByBranch(SessionContext.Instance.LoginUser.BranchInfo.BranchID, SessionContext.Instance.LoginUser.UserID);
            branch.ReminderData = reminderData.Data;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveReminder(ReminderEntity reminderEntity)
        {
            reminderEntity.UserID = SessionContext.Instance.LoginUser.UserID;
            reminderEntity.Username = SessionContext.Instance.LoginUser.Username;
            reminderEntity.BranchInfo = new BranchEntity()
            {
                BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID
            };
            reminderEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            reminderEntity.Transaction = GetTransactionData(reminderEntity.ReminderID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            var data = await _reminderService.ReminderMaintenance(reminderEntity);
            if (data != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult RemoveReminder(long reminderID)
        {
            var result = _reminderService.RemoveReminder(reminderID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

    }
}