using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class AttendanceRegisterController : BaseController
    {
        private readonly IAttendanceService _attendanceService = null;
        public AttendanceRegisterController(IAttendanceService attendanceService)
        {
            this._attendanceService = attendanceService;
        }

        // GET: AttendanceRegister
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetAllAttendanceByBranch(long branchID)
        {
            var data = await this._attendanceService.GetAllAttendanceByBranch(branchID);
            return View("~/Views/AttendanceRegister/Manage.cshtml", data.Data);
        }

        public async Task<ActionResult> GetAttendanceByID(long attendanceID)
        {
            var data = await this._attendanceService.GetAttendanceByAttendanceID(attendanceID);
            return View(data.Data);
        }

        [HttpPost]
        public JsonResult RemoveAttendance(long attendanceID)
        {
            var data = this._attendanceService.RemoveAttendance(attendanceID, SessionContext.Instance.LoginUser.Username);
            return Json(data);
        }

    }
}