using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Announcement;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.Announcement
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IAnnouncementAPI _announcementContext;
        public AnnouncementService(IAnnouncementAPI announcementContext)
        {
            this._announcementContext = announcementContext;
        }

        public async Task<ResponseModel> AnnouncementMaintenance(AnnouncementEntity annInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            AnnouncementEntity ann = new AnnouncementEntity();
            try
            {
               responseModel = await _announcementContext.AnnouncementMaintenance(annInfo);
                //ann.AnnouncementID = announceID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<OperationResult<List<AnnouncementEntity>>> GetAllAnnouncement(long branchID)
        {
            try
            {
                OperationResult<List<AnnouncementEntity>> announcements = new OperationResult<List<AnnouncementEntity>>();
                announcements.Data = await _announcementContext.GetAllAnnouncement(branchID);
                announcements.Completed = true;
                return announcements;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<AnnouncementEntity>> GetNotificationByAnnouncementID(long announceID)
        {
            try
            {
                OperationResult<AnnouncementEntity> announcements = new OperationResult<AnnouncementEntity>();
                announcements.Data = await _announcementContext.GetAnnouncementsByAnnouncementID(announceID);
                announcements.Completed = true;
                return announcements;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveAnnouncement(long annID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                return this._announcementContext.RemoveAnnouncement(annID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

    }
}
