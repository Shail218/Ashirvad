using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Announcement
{
    public interface IAnnouncementAPI
    {
        Task<ResponseModel> AnnouncementMaintenance(AnnouncementEntity annInfo);
        Task<List<AnnouncementEntity>> GetAllAnnouncement(long branchID);
        Task<AnnouncementEntity> GetAnnouncementsByAnnouncementID(long annID);
        ResponseModel RemoveAnnouncement(long annID, string lastupdatedby);
    }
}
