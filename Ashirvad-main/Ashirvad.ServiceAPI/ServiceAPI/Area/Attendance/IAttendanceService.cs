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
        Task<AttendanceEntity> AttendanceMaintenance(AttendanceEntity attendanceInfo);
        Task<OperationResult<List<AttendanceEntity>>> GetAllAttendanceByBranch(long branchID, string financialyear);

        Task<OperationResult<AttendanceEntity>> GetAttendanceByAttendanceID(long attendanceID);
        Task<OperationResult<List<AttendanceEntity>>> GetAttendanceByStudentID(long studentID);
        Task<List<StudentEntity>> GetAllStudentByBranchStdBatch(long branchID, long stdID, int batchID, string financialyear);
        bool RemoveAttendance(long attendanceID, string lastupdatedby);
        Task<List<AttendanceEntity>> GetAllCustomAttendanceRegister(DataTableAjaxPostModel model, long branchID, string financialyear);
        Task<OperationResult<List<AttendanceEntity>>> GetAllAttendanceByFilter(DateTime fromDate, DateTime toDate, long branchID, long stdID, long courseid, int batchTimeID, long studentid, string financialyear);
        Task<ResponseModel> VerifyAttendanceRegister(long branchID, long stdID, long courseid, int batchID, DateTime attendanceDate, string financialyear);
        Task<List<AttendanceEntity>> GetAllAttendanceByCustom(DataTableAjaxPostModel model, DateTime fromDate, DateTime toDate, long branchID, long stdID,long courseid, int batchTimeID, long studentid, string financialyear);

    }
}
