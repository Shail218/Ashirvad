using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Attendance;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area.Attendance
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceAPI _attendanceContext;
        public AttendanceService(IAttendanceAPI attendanceContent)
        {
            _attendanceContext = attendanceContent;
        }

        public async Task<ResponseModel> AttendanceMaintenance(AttendanceEntity attendanceInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            AttendanceEntity attendance = new AttendanceEntity();
            try
            {
                responseModel = await _attendanceContext.AttendanceMaintenance(attendanceInfo);
                //attendanceInfo.AttendanceID = attendanceID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<OperationResult<List<AttendanceEntity>>> GetAllAttendanceByBranch(long branchID)
        {
            try
            {
                OperationResult<List<AttendanceEntity>> attendance = new OperationResult<List<AttendanceEntity>>();
                attendance.Data = await _attendanceContext.GetAllAttendanceByBranch(branchID);
                attendance.Completed = true;
                return attendance;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<AttendanceEntity>> GetAllCustomAttendanceRegister(DataTableAjaxPostModel model, long branchID)
        {
            try
            {
                return await this._attendanceContext.GetAllCustomAttendanceRegister(model, branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<AttendanceEntity>> GetAttendanceByAttendanceID(long attendanceID)
        {
            try
            {
                OperationResult<AttendanceEntity> attendance = new OperationResult<AttendanceEntity>();
                attendance.Data = await _attendanceContext.GetAttendanceByID(attendanceID);
                attendance.Completed = true;
                return attendance;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<List<AttendanceEntity>>> GetAttendanceByStudentID(long studentID)
        {
            try
            {
                OperationResult<List<AttendanceEntity>> attendance = new OperationResult<List<AttendanceEntity>>();
                attendance.Data = await _attendanceContext.GetAttendanceByStudentID(studentID);
                attendance.Completed = true;
                return attendance;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<StudentEntity>> GetAllStudentByBranchStdBatch(long branchID, long stdID, int batchID )
        {
            try
            {
                return await this._attendanceContext.GetAllStudentByBranchStdBatch(branchID, stdID, batchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        public async Task<ResponseModel> VerifyAttendanceRegister(long branchID, long stdID,long courseid, int batchID, DateTime attendanceDate, string attendanceRemarks)
        {
            try
            {
                return await this._attendanceContext.VerifyAttendanceRegister(branchID, stdID,courseid, batchID, attendanceDate,attendanceRemarks);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveAttendance(long attendanceID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                return this._attendanceContext.RemoveAttendance(attendanceID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }
        public async Task<OperationResult<List<AttendanceEntity>>> GetAllAttendanceByFilter(DateTime fromDate, DateTime toDate, long branchID, long stdID,long courseid, int batchTimeID, long studentid )
        {
            try
            {
                OperationResult<List<AttendanceEntity>> attendance = new OperationResult<List<AttendanceEntity>>();
                attendance.Data = await _attendanceContext.GetAllAttendanceByFilter(fromDate, toDate, branchID, stdID,courseid, batchTimeID, studentid);
                attendance.Completed = true;
                return attendance;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        public async Task<List<AttendanceEntity>> GetAllAttendanceByCustom(DataTableAjaxPostModel model, DateTime fromDate, DateTime toDate, long branchID, long stdID,long courseid, int batchTimeID, long studentid )
        {
            try
            {

                return  await _attendanceContext.GetAllAttendanceByCustom(model,fromDate, toDate, branchID, stdID,courseid, batchTimeID, studentid);
                
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        
    }
}
