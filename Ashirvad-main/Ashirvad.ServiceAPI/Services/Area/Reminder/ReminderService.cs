using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Reminder;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Reminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.Reminder
{
    public class ReminderService : IReminderService
    {
        private readonly IReminderAPI _reminderContext;
        public ReminderService(IReminderAPI reminderContext)
        {
            this._reminderContext = reminderContext;
        }

        public async Task<ReminderEntity> ReminderMaintenance(ReminderEntity reminderInfo)
        {
            ReminderEntity notif = new ReminderEntity();
            try
            {
                long reminderID = await _reminderContext.ReminderMaintenance(reminderInfo);
                if (reminderID > 0)
                {
                    notif.ReminderID = reminderID;
                }
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return notif;
        }

        public async Task<OperationResult<List<ReminderEntity>>> GetAllReminderByBranch(long branchID, long userID = 0)
        {
            try
            {
                OperationResult<List<ReminderEntity>> banner = new OperationResult<List<ReminderEntity>>();
                banner.Data = await _reminderContext.GetAllRemindersByBranch(branchID, userID);
                banner.Completed = true;
                return banner;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<ReminderEntity>> GetReminderByReminderID(long reminderID)
        {
            try
            {
                OperationResult<ReminderEntity> banner = new OperationResult<ReminderEntity>();
                banner.Data = await _reminderContext.GetReminderByID(reminderID);
                banner.Completed = true;
                return banner;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemoveReminder(long reminderID, string lastupdatedby)
        {
            try
            {
                return this._reminderContext.RemoveReminder(reminderID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }
    }
}
