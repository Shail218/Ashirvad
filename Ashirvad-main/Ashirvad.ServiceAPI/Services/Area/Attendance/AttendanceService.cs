﻿using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Attendance;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.Attendance
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceAPI _attendanceContext;
        public AttendanceService(IAttendanceAPI attendanceContent)
        {
            _attendanceContext = attendanceContent;
        }

        public async Task<AttendanceEntity> AttendanceMaintenance(AttendanceEntity attendanceInfo)
        {
            AttendanceEntity attendance = new AttendanceEntity();
            try
            {
                long attendanceID = await _attendanceContext.AttendanceMaintenance(attendanceInfo);
                attendanceInfo.AttendanceID = attendanceID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return attendance;
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

        public async Task<List<StudentEntity>> GetAllStudentByBranchStdBatch(long branchID, long stdID, int batchID)
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

        public bool RemoveAttendance(long attendanceID, string lastupdatedby)
        {
            try
            {
                return this._attendanceContext.RemoveAttendance(attendanceID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }
        public async Task<OperationResult<List<AttendanceEntity>>> GetAllAttendanceByFilter(DateTime fromDate, DateTime toDate, long branchID, long stdID, int batchTimeID)
        {
            try
            {
                OperationResult<List<AttendanceEntity>> attendance = new OperationResult<List<AttendanceEntity>>();
                attendance.Data = await _attendanceContext.GetAllAttendanceByFilter(fromDate, toDate, branchID, stdID, batchTimeID);
                attendance.Completed = true;
                return attendance;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
    }
}
