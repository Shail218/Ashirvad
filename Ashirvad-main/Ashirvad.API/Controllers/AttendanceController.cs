using Ashirvad.API.Filter;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/attendance/v1")]
    [AshirvadAuthorization]
    public class AttendanceController : ApiController
    {
        private readonly IAttendanceService _attendanceService = null;
        public AttendanceController(IAttendanceService attendanceService)
        {
            this._attendanceService = attendanceService;
        }


        [Route("AttendanceMaintenance")]
        [HttpPost]
        public OperationResult<AttendanceEntity> AttendanceMaintenance(AttendanceEntity attendanceInfo)
        {
            var data = this._attendanceService.AttendanceMaintenance(attendanceInfo);
            OperationResult<AttendanceEntity> result = new OperationResult<AttendanceEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllAttendanceByBranch")]
        [HttpGet]
        public OperationResult<List<AttendanceEntity>> GetAllAttendanceByBranch(long branchID)
        {
            var data = this._attendanceService.GetAllAttendanceByBranch(branchID);
            OperationResult<List<AttendanceEntity>> result = new OperationResult<List<AttendanceEntity>>();
            result = data.Result;
            return result;
        }

        [Route("GetAttendanceByID")]
        [HttpGet]
        public OperationResult<AttendanceEntity> GetAttendanceByID(long attendanceID)
        {
            var data = this._attendanceService.GetAttendanceByAttendanceID(attendanceID);
            OperationResult<AttendanceEntity> result = new OperationResult<AttendanceEntity>();
            result = data.Result;
            return result;
        }

        [Route("GetAttendanceByStudentID")]
        [HttpGet]
        public OperationResult<List<AttendanceEntity>> GetAttendanceByStudentID(long studentID)
        {
            var data = this._attendanceService.GetAttendanceByStudentID(studentID);
            OperationResult<List<AttendanceEntity>> result = new OperationResult<List<AttendanceEntity>>();
            result = data.Result;
            return result;
        }

        [Route("GetAttendanceByFilter")]
        [HttpGet]
        public OperationResult<List<AttendanceEntity>> GetAttendanceByFilter(DateTime fromDate, DateTime toDate, long branchID, long stdID, int batchTimeID, long studentid)
        {
            var data = this._attendanceService.GetAllAttendanceByFilter(fromDate, toDate, branchID, stdID, batchTimeID,studentid);
            OperationResult<List<AttendanceEntity>> result = new OperationResult<List<AttendanceEntity>>();
            result = data.Result;
            return result;
        }

        [Route("GetAllStudentForAttendance")]
        [HttpGet]
        public async Task<OperationResult<List<StudentEntity>>> GetAllStudentForAttendanceAsync(long branchID, long stdID, int batchID,DateTime attendanceDate)
        {
            OperationResult<List<StudentEntity>> result = new OperationResult<List<StudentEntity>>();
            var res = await this._attendanceService.VerifyAttendanceRegister(branchID, stdID, batchID, attendanceDate);
            if (res.Status)
            {
                var data =  this._attendanceService.GetAllStudentByBranchStdBatch(branchID, stdID, batchID);
                result.Data = data.Result;
                result.Completed = true;
                result.Message = "Success";
            }
            else
            {
                result.Completed = false;
                result.Message = res.Message;
            }
            
            return result;
        }


        [Route("RemoveAttendance")]
        [HttpPost]
        public OperationResult<bool> RemoveAttendance(long attendanceID, string lastupdatedby)
        {
            var data = this._attendanceService.RemoveAttendance(attendanceID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }
    }
}