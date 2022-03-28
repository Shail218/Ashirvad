using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Attendance
{
    public interface IAttendanceService
    {
        Task<ResponseModel> AttendanceMaintenance(AttendanceEntity attendanceInfo);
        Task<OperationResult<List<AttendanceEntity>>> GetAllAttendanceByBranch(long branchID);

        Task<OperationResult<AttendanceEntity>> GetAttendanceByAttendanceID(long attendanceID);
        Task<OperationResult<List<AttendanceEntity>>> GetAttendanceByStudentID(long studentID);
        Task<List<StudentEntity>> GetAllStudentByBranchStdBatch(long branchID, long stdID, int batchID);
        ResponseModel RemoveAttendance(long attendanceID, string lastupdatedby);
        Task<List<AttendanceEntity>> GetAllCustomAttendanceRegister(DataTableAjaxPostModel model, long branchID);
        Task<OperationResult<List<AttendanceEntity>>> GetAllAttendanceByFilter(DateTime fromDate, DateTime toDate, long branchID, long stdID, long courseid, int batchTimeID, long studentid);
        Task<ResponseModel> VerifyAttendanceRegister(long branchID, long stdID, long courseid, int batchID, DateTime attendanceDate);
        Task<List<AttendanceEntity>> GetAllAttendanceByCustom(DataTableAjaxPostModel model, DateTime fromDate, DateTime toDate, long branchID, long stdID,long courseid, int batchTimeID, long studentid);

    }
}
