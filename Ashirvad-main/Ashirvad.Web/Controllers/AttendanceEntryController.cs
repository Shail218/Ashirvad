using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Repo.DataAcceessAPI.Area.Attendance;
using Newtonsoft.Json;
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
                var line = JsonConvert.DeserializeObject<List<AttendanceDetailEntity>>(aInfo.JsonData);
                aInfo.AttendanceDetail = line;
                long attendanceID = await _attendanceContext.AttendanceMaintenance(aInfo);
                if (attendanceID > 0)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
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

            list= await this._attendanceContext.GetAllStudentByBranchStdBatch(attendanceInfo.Branch.BranchID, attendanceInfo.Standard.StandardID, attendanceInfo.BatchTypeID);
            return View("~/Views/AttendanceEntry/Manage.cshtml", list);
        }
        [HttpPost]
        public async Task<JsonResult> VerifyAttendanceRegister(AttendanceEntity attendanceInfo)
        {
            ResponseModel res = new ResponseModel();
            res= await this._attendanceContext.VerifyAttendanceRegister(attendanceInfo.Branch.BranchID, attendanceInfo.Standard.StandardID, attendanceInfo.BatchTypeID,attendanceInfo.AttendanceDate);
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