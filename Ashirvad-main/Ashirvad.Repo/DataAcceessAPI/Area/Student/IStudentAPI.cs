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
        Task<ResponseModel> CheckPackage(long BranchId);
        Task<ResponseModel> StudentMaintenance(StudentEntity studentInfo);
        Task<ResponseModel> StudentTransferMaintenance(StudentEntity studentInfo);
        Task<List<StudentEntity>> GetAllStudent(long branchID, int status);
        Task<List<StudentEntity>> GetAllStudentByStd(long Std, long Branch, long BatchTime);
        Task<List<StudentEntity>> GetAllStudentByStd(long Std,long courseId, long Branch, long BatchTime);
        Task<List<StudentEntity>> GetAllStudentWithoutContent(long branchID, int status = 0);
        ResponseModel RemoveStudent(long StudentID, string lastupdatedby);
        Task<StudentEntity> GetStudentByID(long studenID);
        Task<List<StudentEntity>> GetAllStudent(string studName, string contactNo);
        Task<List<StudentEntity>> GetAllCustomStudent(DataTableAjaxPostModel model, long branchID, int status);
        Task<List<StudentEntity>> GetAllStudentsName(long branchID, long stdid, long courseid, int batchtime);
        Task<List<StudentEntity>> GetAllCustomStudentMarks(DataTableAjaxPostModel model, long Std, long courseid, long Branch, long Batch);
        Task<List<StudentEntity>> GetFilterStudent(long course, long classname, string finalyear, long branchID);

        Task<ResponseModel> ChangeStudentStatus(long StudentID, string lastupdatedby, int status);
        Task<List<StudentEntity>> GetFilterStudentStatusWise(long course, long classname, int status, long branchID);
    }
}
