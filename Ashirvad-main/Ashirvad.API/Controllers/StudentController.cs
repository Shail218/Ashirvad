using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.Services.Area.Student;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Student;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/student/v1")]
    [AshirvadAuthorization]
    public class StudentController : ApiController
    {
        private readonly IStudentService _studentService = null;
        public StudentController(IStudentService studentService)
        {
            this._studentService = studentService;
        }

        [Route("StudentMaintenance")]
        [HttpPost]
        public OperationResult<StudentEntity> StudentMaintenance(StudentEntity studentInfo)
        {
            var data = this._studentService.StudentMaintenance(studentInfo);
            OperationResult<StudentEntity> result = new OperationResult<StudentEntity>();
            result.Completed = data.Result.Status;
            if (data.Result.Status)
            {
                result.Data = (StudentEntity)data.Result.Data;
            }
            result.Message = data.Result.Message;
            return result;
        }

        [Route("GetStudentByID")]
        [HttpPost]
        public OperationResult<StudentEntity> GetStudentByID(long studenID)
        {
            var data = this._studentService.GetStudentByID(studenID);
            OperationResult<StudentEntity> result = new OperationResult<StudentEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllStudent")]
        [HttpPost]
        public async Task<OperationResult<List<StudentEntity>>> GetAllStudent(long branchID)
        {
            var data = await this._studentService.GetAllStudent(branchID);
            OperationResult<List<StudentEntity>> result = new OperationResult<List<StudentEntity>>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetAllStudentWithoutContent")]
        [HttpPost]
        public async Task<OperationResult<List<StudentEntity>>> GetAllStudentWithoutContent(long branchID)
        {
            var data = await this._studentService.GetAllStudentWithoutContent(branchID);
            OperationResult<List<StudentEntity>> result = new OperationResult<List<StudentEntity>>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetAllStudentWithoutContentByRange")]
        [HttpGet]
        public async Task<OperationResult<List<StudentEntity>>> GetAllStudentWithoutContentByRange(long branchID, int page, int limit)
        {
            Student s = new Student();
            var data = s.GetAllStudentWithoutContentByRange(branchID, page, limit);
            OperationResult<List<StudentEntity>> result = new OperationResult<List<StudentEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllActiveStudent")]
        [HttpPost]
        public async Task<OperationResult<List<StudentEntity>>> GetAllActiveStudent(long branchID)
        {
            var data = await this._studentService.GetAllStudent(branchID, (int)Enums.RowStatus.Active);
            OperationResult<List<StudentEntity>> result = new OperationResult<List<StudentEntity>>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetAllActiveStudentWithoutContent")]
        [HttpPost]
        public async Task<OperationResult<List<StudentEntity>>> GetAllActiveStudentWithoutContent(long branchID)
        {
            var data = await this._studentService.GetAllStudentWithoutContent(branchID, (int)Enums.RowStatus.Active);
            OperationResult<List<StudentEntity>> result = new OperationResult<List<StudentEntity>>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetAllInActiveStudent")]
        [HttpPost]
        public async Task<OperationResult<List<StudentEntity>>> GetAllInActiveStudent(long branchID)
        {
            var data = await this._studentService.GetAllStudent(branchID, (int)Enums.RowStatus.Inactive);
            OperationResult<List<StudentEntity>> result = new OperationResult<List<StudentEntity>>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetAllInActiveStudentWithoutContent")]
        [HttpPost]
        public async Task<OperationResult<List<StudentEntity>>> GetAllInActiveStudentWithoutContent(long branchID)
        {
            var data = await this._studentService.GetAllStudentWithoutContent(branchID, (int)Enums.RowStatus.Inactive);
            OperationResult<List<StudentEntity>> result = new OperationResult<List<StudentEntity>>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetAllStudentByNameAndContact")]
        [HttpPost]
        public async Task<OperationResult<List<StudentEntity>>> GetAllStudent(string studName, string contactNo)
        {
            var data = await this._studentService.GetAllStudent(studName, contactNo);
            OperationResult<List<StudentEntity>> result = new OperationResult<List<StudentEntity>>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("RemoveStudent")]
        [HttpPost]
        public OperationResult<bool> RemoveStudent(long StudentID, string lastupdatedby)
        {
            var data = this._studentService.RemoveStudent(StudentID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = data.Status;
            result.Data = data.Status;
            result.Message = data.Message;
            return result;
        }

        [Route("CheckPackage")]
        [HttpGet]
        public async Task<OperationResult<ResponseModel>> CheckPackage(long branchID)
        {
            var data = await this._studentService.CheckPackage(branchID);
            OperationResult<ResponseModel> result = new OperationResult<ResponseModel>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("StudentMaintenance")]
        [HttpPost]
        public OperationResult<StudentEntity> StudentMaintenance(string model, bool HasFile)
        {
            OperationResult<StudentEntity> result = new OperationResult<StudentEntity>();
            var httpRequest = HttpContext.Current.Request;
            StudentEntity studentEntity = new StudentEntity();
            studentEntity.BranchInfo = new BranchEntity();
            studentEntity.SchoolInfo = new SchoolEntity();
            studentEntity.BatchInfo = new BatchEntity();
            studentEntity.BranchClass = new BranchClassEntity();
            studentEntity.BranchCourse = new BranchCourseEntity();
            var entity = JsonConvert.DeserializeObject<StudentEntity>(model);
            studentEntity.StudentID = entity.StudentID;
            studentEntity.StudentMaint = new StudentMaint()
            {
                ParentID = entity.StudentMaint.ParentID,
                ParentName = entity.StudentMaint.ParentName,
                FatherOccupation = entity.StudentMaint.FatherOccupation,
                MotherOccupation = entity.StudentMaint.MotherOccupation,
                ContactNo = entity.StudentMaint.ContactNo,
                ParentPassword = entity.StudentPassword
            };
            studentEntity.GrNo = "1";
            studentEntity.FirstName = entity.FirstName;
            studentEntity.MiddleName = entity.MiddleName;
            studentEntity.LastName = entity.LastName;
            studentEntity.DOB = entity.DOB;
            studentEntity.Address = entity.Address;
            studentEntity.BranchInfo.BranchID = entity.BranchInfo.BranchID;
            studentEntity.BranchCourse.course_dtl_id = entity.BranchCourse.course_dtl_id;
            studentEntity.BranchClass.Class_dtl_id = entity.BranchClass.Class_dtl_id;
            studentEntity.SchoolInfo.SchoolID = entity.SchoolInfo.SchoolID;
            studentEntity.SchoolTime = entity.SchoolTime;
            studentEntity.BatchInfo.BatchType = (Enums.BatchType)entity.BatchInfo.BatchTime;
            studentEntity.LastYearResult = entity.LastYearResult;
            studentEntity.Grade = entity.Grade;
            studentEntity.LastYearClassName = entity.LastYearClassName;
            studentEntity.ContactNo = entity.ContactNo;
            DateTime dateTime = DateTime.Now;
            studentEntity.Final_Year = dateTime.Month >= 4 ? dateTime.Year.ToString() + "_" + dateTime.Year + 1.ToString("yyyy") : (dateTime.Year - 1).ToString() + "-" + dateTime.Year.ToString();
            studentEntity.AdmissionDate = entity.AdmissionDate;
            studentEntity.StudentPassword = entity.StudentPassword;
            studentEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = entity.RowStatus.RowStatusId
            };
            studentEntity.Transaction = new TransactionEntity()
            {
                TransactionId = entity.Transaction.TransactionId,
                LastUpdateBy = entity.Transaction.LastUpdateBy,
                LastUpdateId = entity.Transaction.LastUpdateId,
                CreatedBy = entity.Transaction.CreatedBy,
                CreatedId = entity.Transaction.CreatedId
            };
            if (HasFile)
            {
                try
                {
                    if (httpRequest.Files.Count > 0)
                    {
                        foreach (string file in httpRequest.Files)
                        {
                            string fileName;
                            string extension;
                            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                            string UpdatedPath = currentDir.Replace("WebAPI", "wwwroot");
                            var postedFile = httpRequest.Files[file];
                            string randomfilename = Common.Common.RandomString(20);
                            extension = Path.GetExtension(postedFile.FileName);
                            fileName = Path.GetFileName(postedFile.FileName);
                            string _Filepath = "/StudentImage/" + randomfilename + extension;
                            string _Filepath1 = "StudentImage/" + randomfilename + extension;
                            var filePath = HttpContext.Current.Server.MapPath("~/StudentImage/" + randomfilename + extension);
                            string _path = UpdatedPath + _Filepath1;
                            postedFile.SaveAs(_path);
                            studentEntity.FileName = fileName;
                            studentEntity.FilePath = _Filepath;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Completed = false;
                    result.Data = null;
                    result.Message = ex.ToString();
                }
            }
            else
            {
                studentEntity.FileName = entity.FileName;
                studentEntity.FilePath = entity.FilePath;
            }
            var data = this._studentService.StudentMaintenance(studentEntity).Result;
            result.Completed = data.Status;
            if (data.Status)
            {
                result.Data = (StudentEntity)data.Data; 
            }
            result.Message = data.Message;

            return result;
        }
    }

}
