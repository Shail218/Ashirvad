using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Announcement
{
    public interface IAnnouncementService
    {
        Task<ResponseModel> AnnouncementMaintenance(AnnouncementEntity annInfo);
        Task<OperationResult<List<AnnouncementEntity>>> GetAllAnnouncement(long branchID);
        Task<OperationResult<AnnouncementEntity>> GetNotificationByAnnouncementID(long announceID);
        ResponseModel RemoveAnnouncement(long annID, string lastupdatedby);
    }
}
