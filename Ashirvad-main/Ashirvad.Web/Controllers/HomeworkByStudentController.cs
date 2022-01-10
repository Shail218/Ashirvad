using Ashirvad.Repo.DataAcceessAPI.Area.DashboardChart;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class HomeworkByStudentController : Controller
    {
        private readonly IChartService _chartService = null;
        private readonly IDashboardChartAPI _dashboardService;

        public HomeworkByStudentController(IChartService chartService, IDashboardChartAPI dashboardService)
        {
            _chartService = chartService;
            _dashboardService = dashboardService;
        }

        // GET: HomeworkByStudent
        public ActionResult Index(long studentID, long subjectID)
        {
            return View();
        }
    }
}