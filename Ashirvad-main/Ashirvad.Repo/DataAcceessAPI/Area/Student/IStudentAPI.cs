using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Student
{
    public interface IStudentAPI
    {
        Task<long> StudentMaintenance(StudentEntity studentInfo);
        Task<List<StudentEntity>> GetAllStudent(long branchID, int status);
        Task<List<StudentEntity>> GetAllStudentByStd(long Std, long Branch,long BatchTime);
        Task<List<StudentEntity>> GetAllStudentWithoutContent(long branchID, int status = 0);
        bool RemoveStudent(long StudentID, string lastupdatedby);
        Task<StudentEntity> GetStudentByID(long studenID);
        Task<List<StudentEntity>> GetAllStudent(string studName, string contactNo);
    }
}
