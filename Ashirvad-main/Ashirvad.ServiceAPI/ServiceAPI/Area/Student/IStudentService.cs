using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Student
{
    public interface IStudentService
    {
        Task<StudentEntity> StudentMaintenance(StudentEntity studentInfo);
        Task<List<StudentEntity>> GetAllStudent(long branchID, int status = 0);
        Task<List<StudentEntity>> GetAllStudentWithoutContent(long branchID, int status = 0);
        bool RemoveStudent(long StudentID, string lastupdatedby);
        Task<StudentEntity> GetStudentByID(long studenID);
        Task<List<StudentEntity>> GetAllStudent(string studName, string contactNo);

        Task<List<StudentEntity>> GetStudentByStd(long Std, long BranchID,long BatchTime);
    }
}
