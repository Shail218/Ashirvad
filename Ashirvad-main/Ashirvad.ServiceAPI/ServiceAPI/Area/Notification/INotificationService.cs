using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Notification
{
    public interface INotificationService
    {
        Task<ResponseModel> NotificationMaintenance(NotificationEntity notifInfo);
        Task<OperationResult<List<NotificationEntity>>> GetAllNotification(long branchID = 0);
        Task<OperationResult<NotificationEntity>> GetNotificationByNotificationID(long notifID);
        Task<List<NotificationEntity>> GetAllCustomNotification(DataTableAjaxPostModel model, long branchID, int typeID);
        Task<List<NotificationEntity>> GetAllCustomNotification2(DataTableAjaxPostModel model, long branchID, int typeID);
        ResponseModel RemoveNotification(long notifID, string lastupdatedby);
        Task<OperationResult<List<NotificationEntity>>> GetAllNotification(long branchID, int typeID);
        Task<OperationResult<List<NotificationEntity>>> GetAllNotificationforexcel(long branchID = 0);
        Task<OperationResult<List<NotificationEntity>>> GetMobileNotification(long branchID);
        Task<OperationResult<List<NotificationEntity>>> GetAllStudentNotification(long branchID, int typeID, long CourseID, long ClassID);
    }
}
