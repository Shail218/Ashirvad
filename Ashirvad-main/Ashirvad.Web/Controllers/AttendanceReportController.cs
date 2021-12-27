using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Attendance;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Student;
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
        private readonly IStudentService _studentService;
        // GET: AttendanceReport

        public AttendanceReportController(IAttendanceService attendanceService, IStudentService studentService)
        {
            _attendanceService = attendanceService;
            _studentService = studentService;
        }
        public async Task<ActionResult> Index()
        {
            AttendanceMaintenanceModel attendance = new AttendanceMaintenanceModel();
            //var data = await this._attendanceService.GetAllAttendanceByFilter(Convert.ToDateTime("2021-08-26 00:00:00.000"), Convert.ToDateTime("2021-09-26 00:00:00.000"), 20, 20, 1);
            //attendance.AttendanceData = data.Data;
            return View(attendance);
        }

        [HttpPost]
        public async Task<ActionResult> SaveReport(DateTime FromDate, DateTime ToDate, long StandardId, int BatchTime,long studentid)
        {
            var data = await this._attendanceService.GetAllAttendanceByFilter(FromDate, ToDate,SessionContext.Instance.LoginUser.BranchInfo.BranchID,StandardId,BatchTime,studentid);
            return View("~/Views/AttendanceReport/Manage.cshtml", data.Data);
        }

        public async Task<JsonResult> StudentData(int BatchTime, long std)
        {
            var studentData = await _studentService.GetAllStudentsName(SessionContext.Instance.LoginUser.BranchInfo.BranchID,BatchTime,std);
            return Json(studentData);
        }
    }
}