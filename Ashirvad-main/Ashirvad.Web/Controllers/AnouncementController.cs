using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class AnouncementController : BaseController
    {
        private readonly IAnnouncementService _announcementService;
        public ResponseModel res = new ResponseModel();

        public AnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }
        // GET: Anouncement
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> AnnouncementMaintenance(long annoID)
        {
            AnnouncementMaintenanceModel branch = new AnnouncementMaintenanceModel();
            if (annoID > 0)
            {
                var result = await _announcementService.GetNotificationByAnnouncementID(annoID);
                branch.AnnouncementInfo = result.Data;
            }
           
            var branchData = await _announcementService.GetAllAnnouncement(SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            //var branchData = await _announcementService.GetAllAnnouncement(0);
            branch.AnnouncementData = branchData.Data;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveAnnouncement(AnnouncementEntity announcement)
        {
            if(announcement.BranchData.BranchID == 1)
            {
                announcement.BranchData.BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
            }
            announcement.TransactionData = GetTransactionData(announcement.AnnouncementID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            announcement.RowStatusData = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _announcementService.AnnouncementMaintenance(announcement);
            if (data != null)
            {
                return Json(data);
            }
            return Json(0);
        }

        [HttpPost]
        public JsonResult RemoveAnnouncement(long annoID)
        {
            var result = _announcementService.RemoveAnnouncement(annoID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }
    }
}