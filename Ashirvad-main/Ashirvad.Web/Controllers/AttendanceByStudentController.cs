using Ashirvad.ServiceAPI.ServiceAPI.Area.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class AttendanceByStudentController : Controller
    {
        private readonly IChartService _chartService = null;

        public AttendanceByStudentController(IChartService chartService)
        {
            _chartService = chartService;
        }

        // GET: AttendanceByStudent
        public ActionResult Index(long studentID, long type)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetStudentContentByID(long StudentID)
        {
            var standardData = await _chartService.GetStudentContentByID(StudentID);
            return View("~/Views/AttendanceByStudent/PersonalInfo.cshtml", standardData);
        }

        [HttpPost]
        public async Task<ActionResult> GetStudentAttendanceDetails(long StudentID, long type)
        {
            var result = await _chartService.GetStudentAttendanceDetails(StudentID, type);
            return View("~/Views/AttendanceByStudent/FilteredData.cshtml", result);
        }
    }
}