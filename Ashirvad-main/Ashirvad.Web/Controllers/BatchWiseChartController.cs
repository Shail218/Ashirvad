using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class BatchWiseChartController : BaseController
    {
        private readonly IChartService _chartService = null;

        public BatchWiseChartController(IChartService chartService)
        {
            _chartService = chartService;
        }
        // GET: BatchWiseChart
        public ActionResult Index(long StandardID)
        {
            return View();
        }

        public async Task<JsonResult> GetAllStandard(long branchID)
        {
            branchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
            var standardData = await _chartService.GetAllClassDDL(branchID);
            return Json(standardData);
        }

        public async Task<JsonResult> GetAllStandardByTime(long branchID)
        {
            if (branchID == 0)
                branchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
            CommonChartModel model = new CommonChartModel();
            var result = await _chartService.AllBranchStandardWithCountByBranch(branchID);
            model.branchstandardlist = result;
            return Json(model);
            //var standardData = await _chartService.AllBranchStandardWithCountByBranch(branchID);
            //return Json(standardData);
        }
    }
}