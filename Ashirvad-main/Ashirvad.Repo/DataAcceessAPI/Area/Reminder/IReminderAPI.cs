using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Reminder
{
    public interface IReminderAPI 
    {
        Task<ResponseModel> ReminderMaintenance(ReminderEntity reminderInfo);
        Task<List<ReminderEntity>> GetAllRemindersByBranch(long branchID, long userID);
        Task<ReminderEntity> GetReminderByID(long reminderID);
        ResponseModel RemoveReminder(long reminderID, string lastupdatedby);
        Task<List<ReminderEntity>> GetAllCustomReminder(DataTableAjaxPostModel model, long branchID);
        Task<List<ReminderEntity>> GetAllReminderList(long branchid);
    }
}
