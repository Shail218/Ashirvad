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
        Task<AnnouncementEntity> AnnouncementMaintenance(AnnouncementEntity annInfo);
        Task<OperationResult<List<AnnouncementEntity>>> GetAllAnnouncement(long branchID, string financialyear);
        Task<OperationResult<AnnouncementEntity>> GetNotificationByAnnouncementID(long announceID);
        bool RemoveAnnouncement(long annID, string lastupdatedby);
    }
}
