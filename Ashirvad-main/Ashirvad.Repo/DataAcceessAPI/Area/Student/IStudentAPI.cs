using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Student
{
    public interface IStudentAPI
    {
        Task<ResponseModel> CheckPackage(long BranchId, string financialyear);
        Task<long> StudentMaintenance(StudentEntity studentInfo);
        Task<ResponseModel> StudentTransferMaintenance(StudentEntity studentInfo);
        Task<List<StudentEntity>> GetAllStudent(long branchID, int status,string financialyear);
        Task<List<StudentEntity>> GetAllStudentByStd(long Std, long Branch, long BatchTime, string financialyear);
        Task<List<StudentEntity>> GetAllStudentWithoutContent(long branchID, string financialyear, int status = 0);
        bool RemoveStudent(long StudentID, string lastupdatedby);
        Task<StudentEntity> GetStudentByID(long studenID);
        Task<List<StudentEntity>> GetAllStudent(string studName, string contactNo, string financialyear);
        Task<List<StudentEntity>> GetAllCustomStudent(DataTableAjaxPostModel model, long branchID, int status, string financialyear);
        Task<List<StudentEntity>> GetAllStudentsName(long branchID, long stdid, long courseid, int batchtime, string financialyear);
        Task<List<StudentEntity>> GetAllCustomStudentMarks(DataTableAjaxPostModel model, long Std, long courseid, long Branch, long Batch, string financialyear);
        Task<List<StudentEntity>> GetFilterStudent(long course, long classname, string finalyear, long branchID, string financialyear);
    }
}
