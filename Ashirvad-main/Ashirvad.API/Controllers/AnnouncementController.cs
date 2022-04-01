using Ashirvad.API.Filter;
using Ashirvad.Data;
using Ashirvad.Repo.Services.Area.Announcement;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Announcement;
using Ashirvad.ServiceAPI.Services.Area.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/announcement/v1")]
    [AshirvadAuthorization]
    public class AnnouncementController : ApiController
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        public AnnouncementController()
        {
            _announcementService = new AnnouncementService(new Announcement());
        }

        [Route("AnnouncementMaintenance")]
        [HttpPost]
        public OperationResult<AnnouncementEntity> AnnouncementMaintenance(AnnouncementEntity announcementEntity)
        {
            var data = _announcementService.AnnouncementMaintenance(announcementEntity);
            OperationResult<AnnouncementEntity> result = new OperationResult<AnnouncementEntity>();
            result.Completed = data.Result.Status;
            if (data.Result.Status)
            {
                result.Data = (AnnouncementEntity)data.Result.Data;
            }
            result.Message = data.Result.Message;
      
            return result;
        }

        [Route("GetAllAnnouncement")]
        [HttpGet]
        public OperationResult<List<AnnouncementEntity>> GetAllAnnouncement(long branchID)
        {
            var data = _announcementService.GetAllAnnouncement(branchID);
            OperationResult<List<AnnouncementEntity>> result = new OperationResult<List<AnnouncementEntity>>();
            result.Completed = true;
            result.Data = data.Result.Data;
            return result;
        }

        [Route("RemoveAnnouncement")]
        [HttpPost]
        public OperationResult<bool> RemoveAnnouncement(long annoID, string lastupdatedby)
        {
            var data = _announcementService.RemoveAnnouncement(annoID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = data.Status;
            result.Data = data.Status;
            result.Message = data.Message;
            return result;
        }
    }
}
