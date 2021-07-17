using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Attendance
{
    public interface IAttendanceService
    {
        Task<AttendanceEntity> AttendanceMaintenance(AttendanceEntity attendanceInfo);
        Task<OperationResult<List<AttendanceEntity>>> GetAllAttendanceByBranch(long branchID);

        Task<OperationResult<AttendanceEntity>> GetAttendanceByAttendanceID(long attendanceID);
        Task<OperationResult<List<AttendanceEntity>>> GetAttendanceByStudentID(long studentID);
        Task<List<StudentEntity>> GetAllStudentByBranchStdBatch(long branchID, long stdID, int batchID);
        bool RemoveAttendance(long attendanceID, string lastupdatedby);

        Task<OperationResult<List<AttendanceEntity>>> GetAllAttendanceByFilter(DateTime fromDate, DateTime toDate, long branchID, long stdID, int batchTimeID);

    }
}
