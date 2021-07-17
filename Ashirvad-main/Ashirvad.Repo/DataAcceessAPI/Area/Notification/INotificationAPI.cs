using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Notification
{
    public interface INotificationAPI
    {
        Task<long> NotificationMaintenance(NotificationEntity NotificationInfo);
        Task<List<NotificationEntity>> GetAllNotification(long branchID);
        Task<NotificationEntity> GetNotificationByNotificationID(long NotificationID);
        bool RemoveNotification(long NotificationID, string lastupdatedby);
        Task<List<NotificationEntity>> GetAllNotification(long branchID, int typeID);
    }
}
