using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Notification;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationAPI _notificationContext;
        public NotificationService(INotificationAPI notifContext)
        {
            this._notificationContext = notifContext;
        }

        public async Task<OperationResult<List<NotificationEntity>>> GetAllNotification(long branchID, int typeID)
        {
            OperationResult<List<NotificationEntity>> notif = new OperationResult<List<NotificationEntity>>();
            notif.Data = await _notificationContext.GetAllNotification(branchID, typeID);
            notif.Completed = true;
            return notif;
        }
        
        public async Task<OperationResult<List<NotificationEntity>>> GetAllStudentNotification(long branchID, int typeID, long CourseID, long ClassID)
        {
            OperationResult<List<NotificationEntity>> notif = new OperationResult<List<NotificationEntity>>();
            notif.Data = await _notificationContext.GetAllStudentNotification(branchID, typeID,CourseID,ClassID);
            notif.Completed = true;
            return notif;
        }

        public async Task<List<NotificationEntity>> GetAllCustomNotification(DataTableAjaxPostModel model, long branchID, int typeID)
        {
            try
            {
                return await this._notificationContext.GetAllCustomNotification(model, branchID, typeID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        public async Task<List<NotificationEntity>> GetAllCustomNotification2(DataTableAjaxPostModel model, long branchID, int typeID)
        {
            try
            {
                return await this._notificationContext.GetAllCustomNotification2(model, branchID, typeID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<ResponseModel> NotificationMaintenance(NotificationEntity notifInfo)
        {
            ResponseModel notif = new ResponseModel();
            try
            {
                notif = await _notificationContext.NotificationMaintenance(notifInfo);
               
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
                notif.Status = false;
                notif.Message = ex.Message.ToString();
            }

            return notif;
        }

        public async Task<OperationResult<List<NotificationEntity>>> GetAllNotification(long branchID = 0)
        {
            try
            {
                OperationResult<List<NotificationEntity>> banner = new OperationResult<List<NotificationEntity>>();
                banner.Data = await _notificationContext.GetAllNotification(branchID);
                banner.Completed = true;
                return banner;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<NotificationEntity>> GetNotificationByNotificationID(long notifID)
        {
            try
            {
                OperationResult<NotificationEntity> banner = new OperationResult<NotificationEntity>();
                banner.Data = await _notificationContext.GetNotificationByNotificationID(notifID);
                banner.Completed = true;
                return banner;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        
        public ResponseModel RemoveNotification(long notifID, string lastupdatedby)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                model =  this._notificationContext.RemoveNotification(notifID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
                model.Status = false;
                model.Message = ex.Message.ToString();
            }

            return model;
        }
        public async Task<OperationResult<List<NotificationEntity>>> GetAllNotificationforexcel(long branchID = 0)
        {
            try
            {
                OperationResult<List<NotificationEntity>> banner = new OperationResult<List<NotificationEntity>>();
                banner.Data = await _notificationContext.GetAllNotificationforexcel(branchID);
                banner.Completed = true;
                return banner;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<List<NotificationEntity>>> GetMobileNotification(long branchID)
        {
            OperationResult<List<NotificationEntity>> notif = new OperationResult<List<NotificationEntity>>();
            notif.Data = await _notificationContext.GetMobileNotification(branchID);
            notif.Completed = true;
            return notif;
        }
    }
}
