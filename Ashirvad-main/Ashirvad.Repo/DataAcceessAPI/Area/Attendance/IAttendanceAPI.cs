using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Attendance
{
    public interface IAttendanceAPI
    {
        Task<long> AttendanceMaintenance(AttendanceEntity attendanceInfo);
        Task<List<StudentEntity>> GetAllStudentByBranchStdBatch(long branchID, long stdID, int batchID);
        Task<List<AttendanceEntity>> GetAllAttendanceByBranch(long branchID);
        Task<List<AttendanceEntity>> GetAttendanceByStudentID(long studentID);
        Task<AttendanceEntity> GetAttendanceByID(long attendanceID);
        bool RemoveAttendance(long attendanceID, string lastupdatedby);
        Task<List<AttendanceEntity>> GetAllAttendanceByFilter(DateTime fromDate, DateTime toDate, long branchID, long stdID, int batchTimeID, long studentid);
        Task<List<AttendanceEntity>> GetAllAttendanceByCustom(DataTableAjaxPostModel model, DateTime fromDate, DateTime toDate, long branchID, long stdID, int batchTimeID, long studentid);
        Task<ResponseModel> VerifyAttendanceRegister(long branchID, long stdID, int batchID, DateTime attendanceDate);
        Task<List<AttendanceEntity>> GetAllCustomAttendanceRegister(DataTableAjaxPostModel model, long branchID);
        Task<List<StudentEntity>> GetAllCustomAttendance(DataTableAjaxPostModel model, long Std, long Branch, long Batch);
    }
}
