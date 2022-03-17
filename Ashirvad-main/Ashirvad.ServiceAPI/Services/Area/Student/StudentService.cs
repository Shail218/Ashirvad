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
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area.Student
{
    public class StudentService : IStudentService
    {
        ResponseModel responseModel = new ResponseModel();
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
                    var info = await _studentContext.GetStudentByID(studentID);
                    if (info != null)
                    {
                        studentInfo.StudentID = info.StudentID;
                        studentInfo.UserID = info.UserID;
                        studentInfo.StudentPassword2 = info.StudentPassword2;
                        studentInfo.StudentMaint.UserID = info.StudentMaint.UserID;
                        studentInfo.StudentMaint.ParentID = 0;
                        var user = await _userContext.UserMaintenance(await this.GetUserData(studentInfo, studentID, Enums.UserType.Student));
                        studentInfo.StudentMaint.ParentID = info.StudentMaint.ParentID;
                        studentInfo.StudentPassword2 = info.StudentPassword2;
                        //studentInfo.StudentPassword2 = info.StudentMaint.ParentPassword2;
                        long parentId = info.StudentMaint.ParentID;
                        var user2 = await _userContext.UserMaintenance(await this.GetUserData(studentInfo, parentId, Enums.UserType.Parent));
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return student;
        }
        public async Task<ResponseModel> CheckPackage(long BranchId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                response = await _studentContext.CheckPackage(BranchId);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return response;
        }

        private async Task<UserEntity> GetUserData(StudentEntity studentInfo, long studentID, Enums.UserType userType)
        {
            long? a = null;
            var result = await _branchContext.GetAllBranch();
            //studentInfo.StudentMaint.ParentPassword = studentInfo.StudentMaint.ParentPassword == "" || studentInfo.StudentMaint.ParentPassword == null ? studentInfo.StudentMaint.ParentPassword2 : studentInfo.StudentMaint.ParentPassword;
            studentInfo.StudentMaint.ParentPassword = studentInfo.StudentPassword == "" || studentInfo.StudentPassword == null ? studentInfo.StudentPassword2 : studentInfo.StudentPassword;
            studentInfo.StudentPassword = studentInfo.StudentPassword == "" || studentInfo.StudentPassword == null ? studentInfo.StudentPassword2 : studentInfo.StudentPassword;
            UserEntity user = new UserEntity()
            {
                BranchInfo = result.Where(x => x.BranchID == studentInfo.BranchInfo.BranchID).FirstOrDefault(),
                ClientSecret = "TESTGUID",
                ParentID = userType == Enums.UserType.Parent ? studentInfo.StudentMaint.ParentID : a,
                Password = userType == Enums.UserType.Parent ? studentInfo.StudentPassword : studentInfo.StudentPassword,
                //Password = userType == Enums.UserType.Parent ? studentInfo.StudentMaint.ContactNo : studentInfo.ContactNo,
                RowStatus = studentInfo.RowStatus,
                StudentID = studentInfo.StudentID,
                Transaction = studentInfo.Transaction,
                Username = userType == Enums.UserType.Parent ? studentInfo.StudentMaint.ContactNo : studentInfo.StudentMaint.ContactNo,
                UserType = userType,
                UserID = userType == Enums.UserType.Parent ? (long)studentInfo.StudentMaint.UserID : (long)studentInfo.UserID
            };
            return user;
        }

        public async Task<List<StudentEntity>> GetAllStudent(long branchID,int status = 0)
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
        public async Task<List<StudentEntity>> GetStudentByStd(long Std, long Branch, long BatchTime)
        {
            try
            {
                return await this._studentContext.GetAllStudentByStd(Std, Branch, BatchTime);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<StudentEntity>> GetAllCustomStudentMarks(DataTableAjaxPostModel model, long Std, long courseid, long Branch, long BatchTime)
        {
            try
            {
                return await this._studentContext.GetAllCustomStudentMarks(model, Std, courseid, Branch, BatchTime);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<StudentEntity>> GetAllStudentWithoutContent(long branchID, int status = 0)
        {
            try
            {
                return await this._studentContext.GetAllStudentWithoutContent(branchID,status);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<StudentEntity>> GetAllStudentsName(long branchID, long stdid, long courseid, int batchtime)
        {
            try
            {
                return await this._studentContext.GetAllStudentsName(branchID, stdid, courseid, batchtime);
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

        public async Task<List<StudentEntity>> GetAllCustomStudent(DataTableAjaxPostModel model, long branchID, int status = 0)
        {
            try
            {
                return await this._studentContext.GetAllCustomStudent(model, branchID, status);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<StudentEntity>> GetFilterStudent(long course, long classname, string finalyear, long branchID)
        {
            try
            {
                return await this._studentContext.GetFilterStudent(course,classname,finalyear,branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }


        public async Task<ResponseModel> StudentTransferMaintenance(StudentEntity studentInfo)
        {            
            try
            {
                responseModel = await _studentContext.StudentTransferMaintenance(studentInfo);
                if (responseModel.Status)
                {
                    long StudentID =(long)responseModel.Data;
                    var info = await _studentContext.GetStudentByID(StudentID);
                    if (info != null)
                    {
                        studentInfo.StudentID = info.StudentID;
                        studentInfo.UserID = info.UserID;
                        studentInfo.StudentPassword2 = info.StudentPassword2;
                        studentInfo.StudentMaint.UserID = info.StudentMaint.UserID;
                        studentInfo.StudentMaint.ParentID = 0;
                        var user = await _userContext.StudentUserMaintenance(await this.GetUserData(studentInfo, StudentID, Enums.UserType.Student));
                        studentInfo.StudentMaint.ParentID = info.StudentMaint.ParentID;
                        studentInfo.StudentPassword2 = info.StudentPassword2;
                        //studentInfo.StudentPassword2 = info.StudentMaint.ParentPassword2;
                        long parentId = info.StudentMaint.ParentID;
                        var user2 = await _userContext.StudentUserMaintenance(await this.GetUserData(studentInfo, parentId, Enums.UserType.Parent));
                    }
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message;
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

    }
}
