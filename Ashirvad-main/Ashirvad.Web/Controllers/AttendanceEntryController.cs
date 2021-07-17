using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Repo.DataAcceessAPI.Area.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class AttendanceEntryController : BaseController
    {
        private readonly IAttendanceAPI _attendanceContext;
        public AttendanceEntryController(IAttendanceAPI attendanceContent)
        {
            _attendanceContext = attendanceContent;
        }

        // GET: AttendanceEntry
        public ActionResult Index()
        {
            AttendanceMaintenanceModel model = new AttendanceMaintenanceModel();
            return View("Index", model);
        }

        [HttpPost]
        public async Task<ActionResult> AttendanceMaintenance(AttendanceEntity aInfo)
        {
            AttendanceEntity attendance = new AttendanceEntity();
            //aInfo.AttendanceDetail = attendanceDetailEntities;
            long attendanceID = await _attendanceContext.AttendanceMaintenance(aInfo);
            aInfo.AttendanceID = attendanceID;
            return View("Index", attendance);
        }

        [HttpPost]
        public async Task<ActionResult> GetAllStudentByBranchStdBatch(AttendanceEntity attendanceInfo)
        {
            return View("~/Views/AttendanceEntry/Manage.cshtml", await this._attendanceContext.GetAllStudentByBranchStdBatch(attendanceInfo.Branch.BranchID, attendanceInfo.Standard.StandardID, attendanceInfo.BatchTypeID));
        }

        [HttpPost]
        public JsonResult RemoveAttendance(long attendanceID, string lastupdatedby)
        {
            var result = _attendanceContext.RemoveAttendance(attendanceID, lastupdatedby);
            return Json(result);
        }

    }
}