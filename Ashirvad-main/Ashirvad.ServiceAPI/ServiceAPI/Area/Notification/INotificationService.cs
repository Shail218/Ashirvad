using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Notification
{
    public interface INotificationService
    {
        Task<NotificationEntity> NotificationMaintenance(NotificationEntity notifInfo);
        Task<OperationResult<List<NotificationEntity>>> GetAllNotification(long branchID = 0);
        Task<OperationResult<NotificationEntity>> GetNotificationByNotificationID(long notifID);

        bool RemoveNotification(long notifID, string lastupdatedby);
        Task<OperationResult<List<NotificationEntity>>> GetAllNotification(long branchID, int typeID);
    }
}
