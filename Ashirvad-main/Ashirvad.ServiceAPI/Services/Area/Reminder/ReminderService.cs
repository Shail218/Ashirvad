using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Reminder;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Reminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area.Reminder
{
    public class ReminderService : IReminderService
    {
        private readonly IReminderAPI _reminderContext;
        public ReminderService(IReminderAPI reminderContext)
        {
            this._reminderContext = reminderContext;
        }

        public async Task<ResponseModel> ReminderMaintenance(ReminderEntity reminderInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            ReminderEntity notif = new ReminderEntity();
            try
            {
                //long reminderID = await _reminderContext.ReminderMaintenance(reminderInfo);
                responseModel = await _reminderContext.ReminderMaintenance(reminderInfo);
                //if (responseModel.Status)
                //{
                //    var da = (ReminderEntity)responseModel.Data;
                //    long reminderID = da.ReminderID;
                //    if (reminderID > 0)
                //    {
                //        notif.ReminderID = reminderID;
                //    }
                //}               
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            //return notif;
            return responseModel;
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

        public async Task<List<ReminderEntity>> GetAllCustomReminder(DataTableAjaxPostModel model, long branchID)
        {
            try
            {
                return await this._reminderContext.GetAllCustomReminder(model, branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<ReminderEntity>> GetAllReminderList(long branchID)
        {
            try
            {
                return await this._reminderContext.GetAllReminderList(branchID);
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

        public ResponseModel RemoveReminder(long reminderID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                return this._reminderContext.RemoveReminder(reminderID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }
    }
}
