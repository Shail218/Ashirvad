using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class AdminReportController : BaseController
    {
        private readonly IAdminDataService _adminDataService = null;

        public AdminReportController(IAdminDataService adminDataService)
        {
            this._adminDataService = adminDataService;
        }

        // GET: AdminReport
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllReportData()
        {
            AdminDataModel model = new AdminDataModel();
            var data = this._adminDataService.GetDataUsage(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            model.dataUsageEntitiesInfo = data;
            return View("Index", model);
        }
        public async Task<ActionResult> GetReportDataBranchWise(long branchId)
        {
            AdminDataModel model = new AdminDataModel();
            var data = this._adminDataService.GetDataUsage(branchId);
            model.dataUsageEntitiesInfo = data;
            return View("~/Views/AdminReport/Manage.cshtml", model.dataUsageEntitiesInfo);
        }
    }
}