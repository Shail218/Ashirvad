﻿
using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Reminder
{
    public interface IReminderService
    {
        Task<ReminderEntity> ReminderMaintenance(ReminderEntity reminderInfo);
        Task<OperationResult<List<ReminderEntity>>> GetAllReminderByBranch(long branchID, long userID = 0);
        Task<OperationResult<ReminderEntity>> GetReminderByReminderID(long reminderID);
        bool RemoveReminder(long reminderID, string lastupdatedby);
        Task<List<ReminderEntity>> GetAllCustomReminder(DataTableAjaxPostModel model, long branchID);
    }
}
