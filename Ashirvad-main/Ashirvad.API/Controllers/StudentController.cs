using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Student;
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
            result.Completed = true;
            result.Data = data.Result;
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
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("StudentMaintenance/{StudentID}/{ParentID}/{Gr_No}/{Name}" +
            "/{Birth_Date}/{Address}/{BranchID}/{StandardID}/{SchoolID}/{School_TimeID}/{Batch_TimeID}" +
            "/{Last_Year_Result}/{Grade}/{Class_Name}/{Student_Contact_No}/{Admission_Date}/{Parent_Name}" +
            "/{Father_Occupation}/{Mother_Occupation}/{Parent_Contact_No}/{CreateId}/{CreateBy}/{TransactionId}" +
            "/{std_pwd}/{parent_pwd}/{FileName}/{Extension}/{HasFile}")]
        [HttpPost]
        public OperationResult<StudentEntity> StudentMaintenance(long StudentID, long ParentID, string Gr_No, string Name, DateTime Birth_Date, string Address, long BranchID, string StandardID,
            long SchoolID, int School_TimeID, int Batch_TimeID, string Last_Year_Result, string Grade, string Class_Name,
            string Student_Contact_No, DateTime Admission_Date, string Parent_Name, string Father_Occupation,
            string Mother_Occupation, string Parent_Contact_No, long CreateId, string CreateBy, long TransactionId, string std_pwd,
            string parent_pwd, string FileName, string Extension, bool HasFile)
        {
            OperationResult<StudentEntity> result = new OperationResult<StudentEntity>();
            var httpRequest = HttpContext.Current.Request;
            string[] name = Name.Split(',');
            string[] result_status = Last_Year_Result.Split(',');
            string[] course_standard = StandardID.Split(',');
            StudentEntity studentEntity = new StudentEntity();
            StudentEntity data = new StudentEntity();
            studentEntity.BranchInfo = new BranchEntity();
            studentEntity.StandardInfo = new StandardEntity();
            studentEntity.SchoolInfo = new SchoolEntity();
            studentEntity.StudentMaint = new StudentMaint();
            studentEntity.BatchInfo = new BatchEntity();
            studentEntity.BranchClass = new BranchClassEntity();
            studentEntity.BranchCourse = new BranchCourseEntity();
            studentEntity.StudentID = StudentID;
            studentEntity.StudentMaint.ParentID = ParentID;
            studentEntity.GrNo = Gr_No;
            studentEntity.FirstName = name[0];
            studentEntity.MiddleName = name[1];
            studentEntity.LastName = name[2];
            if (Birth_Date.Equals("01-01-0001"))
            {
                studentEntity.DOB = null;
            }
            else
            {
                studentEntity.DOB = Birth_Date;
            }
            studentEntity.Address = Decode(Address);
            studentEntity.BranchInfo.BranchID = BranchID;
            studentEntity.BranchCourse.course_dtl_id = Convert.ToInt64(course_standard[0]);
            studentEntity.BranchClass.Class_dtl_id = Convert.ToInt64(course_standard[1]);
            studentEntity.SchoolInfo.SchoolID = SchoolID == -1 ? 0 : SchoolID;
            studentEntity.SchoolTime = School_TimeID == -1 ? 0 : School_TimeID;
            studentEntity.BatchInfo.BatchType = (Enums.BatchType)Batch_TimeID;
            studentEntity.LastYearResult = Convert.ToInt32(result_status[0]) == -1 ? 0 : Convert.ToInt32(result_status[0]);
            studentEntity.Grade = Grade == "none" ? "" : Grade;
            studentEntity.LastYearClassName = Class_Name == "none" ? null : Decode(Class_Name);
            studentEntity.ContactNo = Student_Contact_No == "none" ? null : Student_Contact_No;
            DateTime dateTime = DateTime.Now;
            studentEntity.Final_Year = dateTime.Month >= 4 ? dateTime.Year.ToString() + "_" + dateTime.Year + 1.ToString("yyyy") : (dateTime.Year - 1).ToString() + "-" + dateTime.Year.ToString();
            if (Admission_Date.Equals("01-01-0001"))
            {
                studentEntity.AdmissionDate = null;
            }
            else
            {
                studentEntity.AdmissionDate = Admission_Date;
            }
            studentEntity.StudentMaint.ParentName = Decode(Parent_Name);
            studentEntity.StudentMaint.FatherOccupation = Father_Occupation == "none" ? null : Decode(Father_Occupation);
            studentEntity.StudentMaint.MotherOccupation = Mother_Occupation == "none" ? null : Decode(Mother_Occupation);
            studentEntity.StudentMaint.ContactNo = Parent_Contact_No;
            studentEntity.StudentPassword = std_pwd;
            studentEntity.StudentMaint.ParentPassword = parent_pwd;
            studentEntity.FileName = FileName == "none" ? null : FileName;
            if (Extension == "none")
            {
                studentEntity.FilePath = null;
            }
            else
            {
                studentEntity.FilePath = "/StudentImage/" + FileName + "." + Extension;
            }
            if (studentEntity.StudentID > 0 && FileName != "none" && HasFile == false)
            {
                string[] filename = FileName.Split(',');
                studentEntity.FileName = filename[0];
                studentEntity.FilePath = "/StudentImage/" + filename[1] + "." + Extension;
            }
            studentEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = Convert.ToInt32(result_status[1])
            };
            studentEntity.Transaction = new TransactionEntity()
            {
                TransactionId = TransactionId,
                LastUpdateBy = CreateBy,
                LastUpdateId = CreateId,
                CreatedBy = CreateBy,
                CreatedId = CreateId,
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
                            // for live server
                            //string UpdatedPath = currentDir.Replace("mastermindapi", "mastermind");
                            // for local server
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
            data = this._studentService.StudentMaintenance(studentEntity).Result;
            result.Completed = false;
            result.Data = null;
            if (data.StudentID > 0)
            {
                result.Completed = true;
                result.Data = data;
                if (StudentID > 0)
                {
                    result.Message = "Student Updated Successfully";
                }
                else
                {
                    result.Message = "Student Created Successfully";
                }
            }
            return result;
        }

        public static string Decode(string Path)
        {
            byte[] mybyte = Convert.FromBase64String(Path);
            string returntext = Encoding.UTF8.GetString(mybyte);
            return returntext;
        }

    }

}
