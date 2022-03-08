﻿using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Announcement
{
    public interface IAnnouncementAPI
    {
        Task<long> AnnouncementMaintenance(AnnouncementEntity annInfo);
        Task<List<AnnouncementEntity>> GetAllAnnouncement(long branchID, string financialyear);
        Task<AnnouncementEntity> GetAnnouncementsByAnnouncementID(long annID);
        bool RemoveAnnouncement(long annID, string lastupdatedby);
    }
}
