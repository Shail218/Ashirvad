using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.AboutUs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class AboutUsController : BaseController
    {
        private readonly IAboutUsService _aboutUsService;

        public AboutUsController(IAboutUsService aboutUsService)
        {
            _aboutUsService = aboutUsService;
        }
        // GET: AboutUs
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> AboutUsMaintenance(long aboutID)
        {
            AboutUsMaintenanceModel model = new AboutUsMaintenanceModel();
            if (aboutID > 0)
            {
                var about = await _aboutUsService.GetAboutUsByUniqueID(aboutID);
                model.AboutusInfo = about.Data;
            }

            var list = await _aboutUsService.GetAllAboutUs(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            model.AboutusData = list;

            return View("Index", model);
        }

        [HttpPost]
        public async Task<ActionResult> ManageMaintenance(long aboutID)
        {
            AboutUsMaintenanceModel model = new AboutUsMaintenanceModel();
    
            var list = await _aboutUsService.GetAllAboutUs(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            model.AboutusData = list;

            return View("~/Views/AboutUs/Manage.cshtml", model.AboutusData);
        }

        [HttpPost]
        public async Task<ActionResult> DetailMaintenance(long aboutID)
        {
            AboutUsMaintenanceModel model = new AboutUsMaintenanceModel();
            model.detailInfo = new AboutUsDetailEntity();
            if (aboutID > 0)
            {
                var about = await _aboutUsService.GetAboutUsByUniqueID(aboutID);
                model.AboutusInfo = about.Data;
            }

            return View("~/Views/AboutUs/DetailCreate.cshtml", model.detailInfo);
        }

        [HttpPost]
        public async Task<JsonResult> SaveAboutus(AboutUsEntity about)
        {
            about.TransactionInfo = GetTransactionData(about.AboutUsID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            about.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            about.BranchInfo.BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
            var data = await _aboutUsService.AboutUsMaintenance(about);
            if (data != null)
            {
                return Json(true);
            }
            return Json(false);
        }

        [HttpPost]
        public async Task<string> GetHeaderImage(long aboutID)
        {
            var data = await _aboutUsService.GetAboutUsByUniqueID(aboutID);
            var result = data.Data;
            return "data:image/jpg;base64, " + result.HeaderImageText;
        }
    }
}