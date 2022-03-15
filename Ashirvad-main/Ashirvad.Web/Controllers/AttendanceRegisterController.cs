using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

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
            List<AttendanceEntity> entity = new List<AttendanceEntity>();
            return View("~/Views/AttendanceRegister/Manage.cshtml", entity);
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

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            List<string> columns = new List<string>();
            columns.Add("AttendanceDate");
            foreach (var item in model.order)
            {
                item.name = columns[item.column];
            }
            var branchData = await _attendanceService.GetAllCustomAttendanceRegister(model, SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            long total = 0;
            if (branchData.Count > 0)
            {
                total = branchData[0].Count;
            }
            return Json(new
            {
                draw = model.draw,
                iTotalRecords = total,
                iTotalDisplayRecords = total,
                data = branchData
            });

        }

    }
}