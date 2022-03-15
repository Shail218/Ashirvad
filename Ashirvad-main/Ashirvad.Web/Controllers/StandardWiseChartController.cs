using Ashirvad.Common;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class StandardWiseChartController : BaseController
    {
        private readonly IChartService _chartService = null;

        public StandardWiseChartController(IChartService chartService)
        {
            _chartService = chartService;
        }
        // GET: StandardWiseChart
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> StudentData(long branchID)
        {
            if (branchID == 0)
                branchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
            var studentData = await _chartService.GetAllStudentsName(branchID);
            return Json(studentData);
        }

        public async Task<JsonResult> StudentDataByStandard(long StdID, long courseid)
        {
            var studentData = await _chartService.GetAllStudentsNameByStandard(StdID,courseid);
            return Json(studentData);
        }

        [HttpPost]
        public async Task<ActionResult> GetAllClassDDL(long branchID)
        {
            if (branchID == 0)
                branchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
            var standardData = await _chartService.GetAllClassDDL(branchID);
            return View("~/Views/StandardWiseChart/FilteredData.cshtml", standardData);
        }
    }
}