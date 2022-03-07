using Ashirvad.Data.Model;
using Ashirvad.Repo.DataAcceessAPI.Area.DashboardChart;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class ProgressReportChartController : Controller
    {
        private readonly IChartService _chartService = null;
        private readonly IDashboardChartAPI _dashboardService;

        public ProgressReportChartController(IChartService chartService, IDashboardChartAPI dashboardService)
        {
            _chartService = chartService;
            _dashboardService = dashboardService;
        }

        // GET: ProgressReportChart
        public ActionResult Index(long StudentID)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetStudentContentByID(long StudentID)
        {
            var standardData = await _chartService.GetStudentContentByID(StudentID);
            return View("~/Views/ProgressReportChart/FilteredData.cshtml", standardData);
        }

        [HttpPost]
        public async Task<ActionResult> GetTotalCountList(long StudentID)
        {
            var standardData = await _dashboardService.GetTotalCountList(StudentID);
            return View("~/Views/ProgressReportChart/Report.cshtml", standardData);
        }

        [HttpPost]
        public async Task<JsonResult> GetStudentAttendanceDetails(long StudentID)
        {
            CommonChartModel model = new CommonChartModel();
            var result = await _dashboardService.GetStudentAttendanceDetails(StudentID);
            model.dataPoints = result;
            return Json(model);
        }

        [HttpPost]
        public async Task<JsonResult> GetHomeworkByStudent(long branchID, long StudentID)
        {
            if (branchID == 0)
                branchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
            CommonChartModel model = new CommonChartModel();
            var result = await _dashboardService.GetHomeworkByStudent(branchID, StudentID);
            model.dataPoints = result;
            return Json(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetHomeworkByStudent2(long branchID, long StudentID)
        {
            if (branchID == 0)
                branchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
            var standardData = await _dashboardService.GetHomeworkByStudent(branchID, StudentID);
            if(standardData.Count > 0)
            {
                return View("~/Views/ProgressReportChart/Homework.cshtml", standardData);
            }
            else
            {
                return View("~/Views/ProgressReportChart/Empty.cshtml");
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetTestByStudent(long branchID=0, long StudentID=0)
        {
            if (branchID == 0)
                branchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
            CommonChartModel model = new CommonChartModel();
            var result = await _dashboardService.GetTestdetailsByStudent(branchID, StudentID);            
            model.testdataPoints = (List<TestDataPoints>)result.Data;
            return Json(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetTestByStudent2(long branchID=0, long StudentID=0)
        {
            List<TestDataPoints> testdataPoints  = new List<TestDataPoints>();
            if (branchID == 0)
                branchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
            var standardData = await _dashboardService.GetTestdetailsByStudent(branchID, StudentID);
            testdataPoints = (List<TestDataPoints>)standardData.Data;
            ViewBag.OverallTest = standardData.Overall;
            return View("~/Views/ProgressReportChart/Test.cshtml", testdataPoints);
        }
    }
}