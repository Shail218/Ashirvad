using Ashirvad.Data;
using Ashirvad.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Charts
{
    public interface IChartAPI
    {
        Task<List<StudentEntity>> GetAllStudentsName(long branchID);
        Task<List<StandardEntity>> GetAllClassDDL(long BranchID);
        Task<StudentEntity> GetStudentContentByID(long studentID);
        Task<List<StudentEntity>> GetStudentContent(long stdID, long branchID, long batchID);
        Task<List<BranchStandardEntity>> AllBranchStandardWithCountByBranch(long branchid);
        Task<List<AttendanceEntity>> GetStudentAttendanceDetails(long studentID, long type);
        Task<List<StudentEntity>> GetAllStudentsNameByStandard(long StdID, long courseid);
    }
}
