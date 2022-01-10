using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Repo.DataAcceessAPI.Area.DashboardChart;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

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
        public async Task<ActionResult> HomeworkchartMaintenance(long StudentID)
        {
            HomeworkchartModel model = new HomeworkchartModel();
            if (StudentID > 0)
            {
                var result = await _chartService.GetStudentContentByID(StudentID);
                model.studentlist = result;
            }
            model.homeworklist = new List<HomeworkDetailEntity>();

            return View("Index", model);
        }
        [HttpPost]
        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model,long StudentID=0,long SubjectID=0)
        {
            var Data = await _dashboardService.GetHomeworkDetailsByStudent(model,StudentID,SubjectID);
            long total = 0;
            if (Data.Count > 0)
            {
                total = Data[0].Count;
            }
            return Json(new
            {
                draw = model.draw,
                iTotalRecords = total,
                iTotalDisplayRecords = total,
                data = Data
            });

        }
    }
}