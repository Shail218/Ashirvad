using Ashirvad.ServiceAPI.ServiceAPI.Area.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class AttendanceReportController : BaseController
    {

        private readonly IAttendanceService _attendanceService = null;
        // GET: AttendanceReport

        public AttendanceReportController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SaveReport(DateTime FromDate, DateTime ToDate, long BranchId, long StandardId, int BatchTime)
        {
            var data = await this._attendanceService.GetAllAttendanceByFilter(FromDate, ToDate,BranchId,StandardId,BatchTime);
            return View("~/Views/AttendanceReport/Manage.cshtml", data.Data);
        }
    }
}