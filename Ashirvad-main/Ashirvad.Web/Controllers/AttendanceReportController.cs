using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Attendance;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

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
        public async Task<ActionResult> SaveReport(DateTime FromDate, DateTime ToDate, long StandardId,long courseid, int BatchTime,long studentid)
        {
            var data = await this._attendanceService.GetAllAttendanceByFilter(FromDate, ToDate,SessionContext.Instance.LoginUser.BranchInfo.BranchID,StandardId,courseid,BatchTime,studentid);
            return View("~/Views/AttendanceReport/Manage.cshtml", data.Data);
        }

        public async Task<JsonResult> StudentData(long std,long courseid, int BatchTime)
        {
            var studentData = await _studentService.GetAllStudentsName(SessionContext.Instance.LoginUser.BranchInfo.BranchID,std,courseid, BatchTime);
            return Json(studentData);
        }

        [HttpPost]
        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model, DateTime FromDate, DateTime ToDate, long StandardId=0,long courseid=0, int BatchTime=0, long studentid=0)
        {
            // action inside a standard controller
            try
            {
                var branchData = await _attendanceService.GetAllAttendanceByCustom(model, FromDate, ToDate, SessionContext.Instance.LoginUser.BranchInfo.BranchID, StandardId,courseid, BatchTime, studentid);
                long total = 0;
                if (branchData.Count > 0)
                {
                    total = branchData[0].Count;
                }
                return Json(new
                {
                    // this is what datatables wants sending back
                    draw = model.draw,
                    iTotalRecords = total,
                    iTotalDisplayRecords = total,
                    data = branchData
                });
            }
            catch (Exception ex)
            {
                throw;
            }


        }
    }
}