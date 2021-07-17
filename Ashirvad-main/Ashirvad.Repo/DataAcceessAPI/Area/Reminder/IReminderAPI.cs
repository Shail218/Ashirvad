using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Reminder
{
    public interface IReminderAPI 
    {
        Task<long> ReminderMaintenance(ReminderEntity reminderInfo);
        Task<List<ReminderEntity>> GetAllRemindersByBranch(long branchID, long userID);
        Task<ReminderEntity> GetReminderByID(long reminderID);
        bool RemoveReminder(long reminderID, string lastupdatedby);

    }
}
