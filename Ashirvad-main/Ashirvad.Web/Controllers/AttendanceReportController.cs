using Ashirvad.Data.Model;
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
        public async Task<ActionResult> Index()
        {
            AttendanceMaintenanceModel attendance = new AttendanceMaintenanceModel();
            //var data = await this._attendanceService.GetAllAttendanceByFilter(Convert.ToDateTime("2021-08-26 00:00:00.000"), Convert.ToDateTime("2021-09-26 00:00:00.000"), 20, 20, 1);
            //attendance.AttendanceData = data.Data;
            return View(attendance);
        }

        [HttpPost]
        public async Task<ActionResult> SaveReport(DateTime FromDate, DateTime ToDate, long BranchId, long StandardId, int BatchTime)
        {
            var data = await this._attendanceService.GetAllAttendanceByFilter(FromDate, ToDate,BranchId,StandardId,BatchTime);
            return View("~/Views/AttendanceReport/Manage.cshtml", data.Data);
        }
    }
}