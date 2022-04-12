using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Repo.DataAcceessAPI.Area.Attendance;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

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
        public async Task<JsonResult> AttendanceMaintenance(AttendanceEntity aInfo)
        {
            try
            {
                aInfo.Transaction = GetTransactionData(aInfo.AttendanceID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
                aInfo.RowStatus = new RowStatusEntity()
                {
                    RowStatusId = (int)Enums.RowStatus.Active
                };
                //aInfo.AttendanceDetail = attendanceDetailEntities;
                //var Test= DateTime.ParseExact(aInfo.AttendanceDatetxt, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                //var Test= DateTime.ParseExact(aInfo.AttendanceDatetxt, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                var line = JsonConvert.DeserializeObject<List<AttendanceDetailEntity>>(aInfo.JsonData);
                aInfo.AttendanceDetail = line;
                var attendanceID = await _attendanceContext.AttendanceMaintenance(aInfo);
                
                    return Json(attendanceID);
                
            }
            catch(Exception ex)
            {
                throw;
            }
           
        }

        [HttpPost]
        public async Task<ActionResult> GetAllStudentByBranchStdBatch(AttendanceEntity attendanceInfo)
        {
            List<StudentEntity> list = new List<StudentEntity>();

            list= await this._attendanceContext.GetAllStudentByBranchStdBatch(attendanceInfo.Branch.BranchID, attendanceInfo.BranchClass.Class_dtl_id, attendanceInfo.BatchTypeID);
            return View("~/Views/AttendanceEntry/Manage.cshtml", list);
        }

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model, long STD,long courseid, long BatchTime)
        {
            List<string> columns = new List<string>();
            var branchData = await _attendanceContext.GetAllCustomAttendance(model, STD,courseid, SessionContext.Instance.LoginUser.BranchInfo.BranchID, BatchTime);
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

        [HttpPost]
        public async Task<JsonResult> VerifyAttendanceRegister(AttendanceEntity attendanceInfo)
        {
            ResponseModel res = new ResponseModel();
            res= await this._attendanceContext.VerifyAttendanceRegister(attendanceInfo.Branch.BranchID, attendanceInfo.BranchClass.Class_dtl_id,attendanceInfo.BranchCourse.course_dtl_id, attendanceInfo.BatchTypeID,attendanceInfo.AttendanceDate,attendanceInfo.AttendanceRemarks);
            return Json(res);
        }

        [HttpPost]
        public JsonResult RemoveAttendance(long attendanceID, string lastupdatedby)
        {
            var result = _attendanceContext.RemoveAttendance(attendanceID, lastupdatedby);
            return Json(result);
        }

    }
}