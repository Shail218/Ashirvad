using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Student;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Test;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [System.Web.Http.RoutePrefix("api/Marks/v1")]
    [AshirvadAuthorization]
    public class MarksController : ApiController
    {
        private readonly IMarksService _marksService = null;
        private readonly ITestService _testService = null;
        private readonly IStudentService _studentService = null;
        public MarksController(IMarksService marksService, ITestService testService, IStudentService studentService)
        {
            this._marksService = marksService;
            this._testService = testService;
            this._studentService = studentService;
        }

        [Route("GetTestDatesByBatch")]
        [HttpPost]
        public OperationResult<List<TestEntity>> GetTestDatesByBatch(long BranchID, long stdID, int BatchType)
        {
            var data = this._testService.GetAllTestByBranchAndStandard(BranchID, stdID, BatchType).Result;
            OperationResult<List<TestEntity>> result = new OperationResult<List<TestEntity>>();
            result.Completed = true;
            result.Data = data.Data;
            return result;
        }

        [Route("GetTestDetails")]
        [HttpPost]
        public OperationResult<TestEntity> GetTestDetails(long TestID, long SubjectID)
        {
            var data = this._testService.GetTestDetails(TestID, SubjectID);
            OperationResult<TestEntity> result = new OperationResult<TestEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetStudentByStd")]
        [HttpPost]
        public OperationResult<List<StudentEntity>> GetStudentByStd(long Std, long Branch, long BatchTime)
        {
            var data = this._studentService.GetStudentByStd(Std, Branch, BatchTime);
            OperationResult<List<StudentEntity>> result = new OperationResult<List<StudentEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("MarksMaintenance/{MarksID}/{Marks_Date}/{TestID}/{BranchID}/{StudentID}/{Achieve_Marks}/{CreateId}/{CreateBy}/{TransactionId}/{FileName}/{Extension}/{HasFile}")]
        [HttpPost]
        public OperationResult<MarksEntity> MarksMaintenance(long MarksID, DateTime Marks_Date, long TestID, long BranchID, string StudentID, string Achieve_Marks, long CreateId, string CreateBy, long TransactionId, string FileName, string Extension, bool HasFile)
        {
            OperationResult<MarksEntity> result = new OperationResult<MarksEntity>();
            var httpRequest = HttpContext.Current.Request;
            MarksEntity data = new MarksEntity();
            string[] studet = StudentID.Split(',');
            string[] marks = Achieve_Marks.Split(',');
            for (int i = 0; i < studet.Length; i++)
            {
                MarksEntity marksEntity = new MarksEntity();
                marksEntity.BranchInfo = new BranchEntity();
                marksEntity.testEntityInfo = new TestEntity();
                marksEntity.student = new StudentEntity();
                marksEntity.student.StudentID = Convert.ToInt64(studet[i]);
                marksEntity.AchieveMarks = marks[i];
                marksEntity.MarksID = MarksID;
                marksEntity.MarksDate = Marks_Date;
                marksEntity.testEntityInfo.TestID = TestID;
                marksEntity.BranchInfo.BranchID = BranchID;
                marksEntity.MarksContentFileName = FileName;
                marksEntity.MarksFilepath = "/MarksImage/" + FileName + "." + Extension;
                marksEntity.RowStatus = new RowStatusEntity()
                {
                    RowStatusId = (int)Enums.RowStatus.Active
                };
                marksEntity.Transaction = new TransactionEntity()
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
                                //string UpdatedPath = currentDir.Replace("AshirvadAPI", "ashivadproduct");
                                // for local server
                                string UpdatedPath = currentDir.Replace("Ashirvad.API", "Ashirvad.Web");
                                var postedFile = httpRequest.Files[file];
                                string randomfilename = Common.Common.RandomString(20);
                                extension = Path.GetExtension(postedFile.FileName);
                                fileName = Path.GetFileName(postedFile.FileName);
                                string _Filepath = "/MarksImage/" + randomfilename + extension;
                                string _Filepath1 = "MarksImage/" + randomfilename + extension;
                                var filePath = HttpContext.Current.Server.MapPath("~/MarksImage/" + randomfilename + extension);
                                string _path = UpdatedPath + _Filepath1;
                                postedFile.SaveAs(_path);
                                marksEntity.MarksContentFileName = fileName;
                                marksEntity.MarksFilepath = _Filepath;
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
                data = this._marksService.MarksMaintenance(marksEntity).Result;                
            }
            result.Completed = false;
            result.Data = null;
            if (data.MarksID > 0)
            {
                result.Completed = true;
                result.Data = data;
                result.Message = "Marks Added Successfully.";
            }
            else
            {
                result.Message = "Marks Already Added!!";
            }
            return result;
        }
    }
}
