using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Branch;
using Ashirvad.Repo.DataAcceessAPI.Area.Student;
using Ashirvad.Repo.DataAcceessAPI.Area.User;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Staff;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.Student
{
    public class StudentService : IStudentService
    {

        private readonly IStudentAPI _studentContext;
        private readonly IBranchAPI _branchContext;
        private readonly IUserAPI _userContext;
        public StudentService(IStudentAPI studentContext, IBranchAPI branchContext, IUserAPI userContext)
        {
            this._studentContext = studentContext;
            this._branchContext = branchContext;
            this._userContext = userContext;
        }
        public async Task<StudentEntity> StudentMaintenance(StudentEntity studentInfo)
        {
            StudentEntity student = new StudentEntity();
            try
            {
                long studentID = await _studentContext.StudentMaintenance(studentInfo);
                if (studentID > 0)
                {
                    student.StudentID = studentID;
                    var user = await _userContext.UserMaintenance(await this.GetUserData(studentInfo, studentID, Enums.UserType.Student));
                    user = await _userContext.UserMaintenance(await this.GetUserData(studentInfo, studentID, Enums.UserType.Parent));
                }
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return student;
        }

        private async Task<UserEntity> GetUserData(StudentEntity studentInfo, long studentID, Enums.UserType userType)
        {
            var result = await _branchContext.GetAllBranch();
            UserEntity user = new UserEntity()
            {
                BranchInfo = result.Where(x => x.BranchID == studentInfo.BranchInfo.BranchID).FirstOrDefault(),
                ClientSecret = "TESTGUID",
                ParentID = studentInfo.StudentMaint.ParentID,
                Password = userType == Enums.UserType.Parent ? studentInfo.StudentMaint.ContactNo : studentInfo.ContactNo,
                RowStatus = studentInfo.RowStatus,
                StudentID = studentID,
                Transaction = studentInfo.Transaction,
                Username = userType == Enums.UserType.Parent ? studentInfo.StudentMaint.ContactNo : studentInfo.ContactNo,
                UserType = userType
            };

            return user;
        }

        public async Task<List<StudentEntity>> GetAllStudent(long branchID, int status = 0)
        {
            try
            {
                return await this._studentContext.GetAllStudent(branchID, status);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemoveStudent(long StudentID, string lastupdatedby)
        {
            try
            {
                return this._studentContext.RemoveStudent(StudentID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

        public async Task<StudentEntity> GetStudentByID(long studenID)
        {
            try
            {
                return await this._studentContext.GetStudentByID(studenID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<StudentEntity>> GetAllStudent(string studName, string contactNo)
        {
            try
            {
                return await this._studentContext.GetAllStudent(studName, contactNo);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
    }
}
