using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Student
{
    public interface IStudentService
    {
        Task<ResponseModel> CheckPackage(long BranchId);
        Task<ResponseModel> StudentMaintenance(StudentEntity studentInfo);
        Task<ResponseModel> StudentTransferMaintenance(StudentEntity studentInfo);
        Task<List<StudentEntity>> GetAllStudent(long branchID, int status = 0);
        Task<List<StudentEntity>> GetAllStudentWithoutContent(long branchID, int status =0);
        ResponseModel RemoveStudent(long StudentID, string lastupdatedby);
        Task<StudentEntity> GetStudentByID(long studenID);
        Task<List<StudentEntity>> GetAllStudent(string studName, string contactNo);
        Task<List<StudentEntity>> GetAllCustomStudentMarks(DataTableAjaxPostModel model, long Std, long courseid, long Branch, long BatchTime);
        Task<List<StudentEntity>> GetStudentByStd(long Std, long BranchID,long BatchTime);
        Task<List<StudentEntity>> GetAllCustomStudent(DataTableAjaxPostModel model, long branchID, int status = 0);
        Task<List<StudentEntity>> GetAllStudentsName(long branchID, long stdid,long courseid, int batchtime);
        Task<List<StudentEntity>> GetFilterStudent(long course, long classname, string finalyear, long branchID);
    }
}
