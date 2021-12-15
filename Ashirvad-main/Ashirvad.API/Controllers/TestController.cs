using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Test;
using Ashirvad.ServiceAPI.Services.Area.Test;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/test/v1")]
    [AshirvadAuthorization]
    public class TestController : ApiController
    {
        private readonly ITestService _testService = null;
        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [Route("TestMaintenance")]
        [HttpPost]
        public OperationResult<TestEntity> TestMaintenance(TestEntity testInfo)
        {
            OperationResult<TestEntity> result = new OperationResult<TestEntity>();

            var data = this._testService.TestMaintenance(testInfo);
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllTestByBranch")]
        [HttpGet]
        public OperationResult<List<TestEntity>> GetAllTestByBranch(long branchID)
        {
            var data = this._testService.GetAllTestByBranch(branchID);
            return data.Result;
        }

        [Route("GetAllTestByBranchAPI")]
        [HttpGet]
        public OperationResult<List<TestEntity>> GetAllTestByBranchAPI(long branchID)
        {
            var data = this._testService.GetAllTestByBranchAPI(branchID);
            return data.Result;
        }

        [Route("GetAllTestPapaerWithoutContentByTest")]
        [HttpGet]
        public OperationResult<List<TestPaperEntity>> GetAllTestPapaerWithoutContentByTest(long testID)
        {
            var data = this._testService.GetAllTestPapaerWithoutContentByTest(testID);
            return data.Result;
        }

        [Route("GetAllTests")]
        [HttpGet]
        public OperationResult<List<TestEntity>> GetAllTests(DateTime testDate, string searchParam)
        {
            var data = this._testService.GetAllTest(testDate, searchParam);
            return data.Result;
        }

        [Route("GetAllTestByBranchAndSTD")]
        [HttpGet]
        public OperationResult<List<TestEntity>> GetAllTestByBranchAndSTD(long branchID, long stdID, int batchTime)
        {
            var data = this._testService.GetAllTestByBranchAndStandard(branchID, stdID, batchTime);
            return data.Result;
        }


        [Route("GetTestByTestID")]
        [HttpGet]
        public OperationResult<TestEntity> GetTestByTestID(long testID)
        {
            var data = this._testService.GetTestByTestID(testID);
            OperationResult<TestEntity> result = new OperationResult<TestEntity>();
            result = data.Result;
            return result;
        }


        [Route("RemoveTest")]
        [HttpPost]
        public OperationResult<bool> RemoveTest(long testID, string lastupdatedby, bool paper)
        {
            var data = this._testService.RemoveTest(testID, lastupdatedby,true);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("TestPaperMaintenance")]
        [HttpPost]
        public OperationResult<TestPaperEntity> TestPaperMaintenance(TestPaperEntity testInfo)
        {
            OperationResult<TestPaperEntity> result = new OperationResult<TestPaperEntity>();

            var data = this._testService.TestPaperMaintenance(testInfo);
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllTestPapaerByTest")]
        [HttpGet]
        public OperationResult<List<TestPaperEntity>> GetAllTestPapaerByTest(long testID)
        {
            var data = this._testService.GetAllTestPapaerByTest(testID);
            OperationResult<List<TestPaperEntity>> result = new OperationResult<List<TestPaperEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetTestPaperByPaperID")]
        [HttpGet]
        public OperationResult<TestPaperEntity> GetTestPaperByPaperID(long paperID)
        {
            var data = this._testService.GetTestPaperByPaperID(paperID);
            OperationResult<TestPaperEntity> result = new OperationResult<TestPaperEntity>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetTestPaperChecking")]
        [HttpGet]
        public OperationResult<List<TestEntity>> GetTestPaperChecking(long paperID)
        {
            var data = this._testService.GetTestPaperChecking(paperID);
            OperationResult<List<TestEntity>> result = new OperationResult<List<TestEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetAllTestPapaerByBranchStdDate")]
        [HttpGet]
        public OperationResult<List<TestPaperEntity>> GetAllTestPapaerByBranchStdDate(long branchID, long stdID, DateTime dt, int batchTime)
        {
            var data = this._testService.GetAllTestPapaerByBranchStdDate(branchID, stdID, dt, batchTime);
            return data.Result;
        }


        [Route("RemoveTestPaper")]
        [HttpPost]
        public OperationResult<bool> RemoveTestPaper(long paperID, string lastupdatedby)
        {
            var data = this._testService.RemoveTestPaper(paperID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }



        [Route("AnswerSheetMaintenance")]
        [HttpPost]
        public OperationResult<StudentAnswerSheetEntity> AnswerSheetMaintenance(StudentAnswerSheetEntity ansSheet)
        {
            OperationResult<StudentAnswerSheetEntity> result = new OperationResult<StudentAnswerSheetEntity>();

            var data = this._testService.StudentAnswerSheetMaintenance(ansSheet);
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("BulkStudentAnswerSheetMaintenance")]
        [HttpPost]
        public OperationResult<List<StudentAnswerSheetEntity>> BulkStudentAnswerSheetMaintenance(List<StudentAnswerSheetEntity> ansSheetInfo)
        {
            OperationResult<List<StudentAnswerSheetEntity>> result = new OperationResult<List<StudentAnswerSheetEntity>>();

            var data = this._testService.BulkStudentAnswerSheetMaintenance(ansSheetInfo);
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllAnsSheetByTest")]
        [HttpGet]
        public OperationResult<List<StudentAnswerSheetEntity>> GetAllAnswerSheetByTest(long testID)
        {
            var data = this._testService.GetAllAnswerSheetByTest(testID);
            OperationResult<List<StudentAnswerSheetEntity>> result = new OperationResult<List<StudentAnswerSheetEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetAllAnsSheetWithoutContentByTest")]
        [HttpGet]
        public OperationResult<List<StudentAnswerSheetEntity>> GetAllAnswerSheetWithoutContentByTest(long testID)
        {
            var data = this._testService.GetAllAnswerSheetWithoutContentByTest(testID);
            OperationResult<List<StudentAnswerSheetEntity>> result = new OperationResult<List<StudentAnswerSheetEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }
        [Route("GetAllAnsSheetByTestStudentID")]
        [HttpGet]
        public OperationResult<List<StudentAnswerSheetEntity>> GetAllAnsSheetByTestStudentID(long testID,long studentID)
        {
            var data = this._testService.GetAllAnsSheetByTestStudentID(testID, studentID);
            OperationResult<List<StudentAnswerSheetEntity>> result = new OperationResult<List<StudentAnswerSheetEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetAnswerSheetByAnsID")]
        [HttpGet]
        public OperationResult<StudentAnswerSheetEntity> GetAnswerSheetByID(long ansID)
        {
            var data = this._testService.GetAnswerSheetByID(ansID);
            OperationResult<StudentAnswerSheetEntity> result = new OperationResult<StudentAnswerSheetEntity>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }


        [Route("RemoveAnswerSheet")]
        [HttpPost]
        public OperationResult<bool> RemoveAnswerSheet(long ansID, string lastupdatedby)
        {
            var data = this._testService.RemoveAnswerSheet(ansID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }


        //[HttpPost]
        //[Route("TestDetailMaintenance/{TestID}/{BranchID}/{StudentID}/{Remarks}/{Status}/{SubmitDate}/{CreateId}/{CreateBy}")]
        //public OperationResult<TestDetailEntity> TestDetailMaintenance(long TestID, long BranchID, long StudentID, string Remarks, int? Status, DateTime SubmitDate, long CreateId, string CreateBy)
        //{
        //    TestDetailEntity TestDetail = new TestDetailEntity();
        //    TestDetailEntity Response = new TestDetailEntity();

        //    TestDetail.TestEntity = new TestEntity();
        //    TestDetail.BranchInfo = new BranchEntity();
        //    TestDetail.StudentInfo = new StudentEntity();
        //    var httpRequest = HttpContext.Current.Request;
        //    TestDetail.TestEntity.TestID = TestID;
        //    TestDetail.BranchInfo.BranchID = BranchID;
        //    TestDetail.StudentInfo.StudentID = StudentID;
        //    TestDetail.Remarks = "";
        //    TestDetail.Status = Status.HasValue ? Status.Value : 0;
        //    TestDetail.SubmitDate = SubmitDate;
        //    TestDetail.RowStatus = new RowStatusEntity()
        //    {
        //        RowStatusId = (int)Enums.RowStatus.Active
        //    };
        //    TestDetail.Transaction = new TransactionEntity()
        //    {
        //        CreatedBy = CreateBy,
        //        CreatedId = CreateId,
        //        CreatedDate = DateTime.Now,
        //    };
        //    OperationResult<TestDetailEntity> result = new OperationResult<TestDetailEntity>();
        //    try
        //    {
        //        foreach (string file in httpRequest.Files)
        //        {
        //            string fileName;
        //            string extension;
        //            var postedFile = httpRequest.Files[file];
        //            string randomfilename = Common.Common.RandomString(20);
        //            extension = Path.GetExtension(postedFile.FileName);
        //            fileName = Path.GetFileName(postedFile.FileName);
        //            string _Filepath = "~/TestDetailImage/" + randomfilename + extension;
        //            var filePath = HttpContext.Current.Server.MapPath("~/TestDetailImage/" + randomfilename + extension);
        //            postedFile.SaveAs(filePath);
        //            TestDetail.AnswerSheetName = fileName;
        //            TestDetail.FilePath = _Filepath;
        //            TestDetail.TestDetailID = 0;
        //            var data = this._testService.TestdetailMaintenance(TestDetail);
        //            Response = data.Result;
        //        }
        //        result.Data = null;
        //        result.Completed = false;
        //        if (Response.TestDetailID > 0)
        //        {
        //            result.Data = Response;
        //            result.Completed = true;
        //            result.Message = "Test Uploaded Successfully!!";
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }


        //    return result;
        //}

        [Route("TestPaperMaintenance/{TestID}/{TestPaperID}/{Paper_Type}/{Doc_Link}/{Paper_Remark}/{CreateId}/{CreateBy}/{TransactionId}/{FileName}/{Extension}/{HasFile}")]
        [HttpPost]
        public OperationResult<TestPaperEntity> TestPaperMaintenance(long TestID, long TestPaperID, int Paper_Type ,string Doc_Link,string Paper_Remark, long CreateId, string CreateBy, long TransactionId, string FileName, string Extension, bool HasFile)
        {
            OperationResult<TestPaperEntity> result = new OperationResult<TestPaperEntity>();
            var httpRequest = HttpContext.Current.Request;
            
            TestPaperEntity testPaperEntity = new TestPaperEntity();
            TestPaperEntity data = new TestPaperEntity();
            testPaperEntity.TestID = TestID;
            testPaperEntity.TestPaperID = TestPaperID;
            testPaperEntity.PaperTypeID = Paper_Type;
            testPaperEntity.DocLink = Doc_Link == "none" ? "" : Decode(Doc_Link);
            testPaperEntity.Remarks = Paper_Remark == "none" ? null : Decode(Paper_Remark);
            if(testPaperEntity.TestID > 0 && Doc_Link == "none" && HasFile == false)
            {
                string[] filename = FileName.Split(',');
                testPaperEntity.FileName = filename[0];
                testPaperEntity.FilePath = "/TestPaper/" + filename[1] + "." + Extension;
            }
            else
            {
                testPaperEntity.FileName = FileName == "none" ? "" : FileName;
                if (Extension == "none")
                {
                    testPaperEntity.FilePath = "";
                }
                else
                {
                    testPaperEntity.FilePath = "/TestPaper/" + FileName + "." + Extension;
                }
            }
            testPaperEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            testPaperEntity.Transaction = new TransactionEntity()
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
                            string UpdatedPath = currentDir.Replace("AshirvadAPI", "ashivadproduct");
                            // for local server
                            //string UpdatedPath = currentDir.Replace("Ashirvad.API", "Ashirvad.Web");
                            var postedFile = httpRequest.Files[file];
                            string randomfilename = Common.Common.RandomString(20);
                            extension = Path.GetExtension(postedFile.FileName);
                            fileName = Path.GetFileName(postedFile.FileName);
                            string _Filepath = "/TestPaper/" + randomfilename + extension;
                            string _Filepath1 = "TestPaper/" + randomfilename + extension;
                            var filePath = HttpContext.Current.Server.MapPath("~/TestPaper/" + randomfilename + extension);
                            string _path = UpdatedPath + _Filepath1;
                            postedFile.SaveAs(_path);
                            testPaperEntity.FileName = fileName;
                            testPaperEntity.FilePath = _Filepath;
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
            data = this._testService.TestPaperMaintenance(testPaperEntity).Result;
            result.Completed = false;
            result.Data = null;
            if (data.TestID > 0)
            {
                result.Completed = true;
                result.Data = data;
                if (TestID > 0)
                {
                    result.Message = "Test Paper Updated Successfully.";
                }
                else
                {
                    result.Message = "Test Paper Created Successfully.";
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

        [Route("TestAnswerSheetMaintenance/{TestID}/{BranchID}/{StudentID}/{Remarks}/{Status}/{SubmitDate}/{CreateId}/{CreateBy}")]
        [HttpPost]
        public OperationResult<StudentAnswerSheetEntity> TestAnswerSheetMaintenance(long TestID, long BranchID, long StudentID, 
            string Remarks, int? Status, DateTime SubmitDate, long CreateId, string CreateBy)
        {
            OperationResult<StudentAnswerSheetEntity> result = new OperationResult<StudentAnswerSheetEntity>();

            StudentAnswerSheetEntity TestDetail = new StudentAnswerSheetEntity();
            StudentAnswerSheetEntity Response = new StudentAnswerSheetEntity();

            TestDetail.TestInfo = new TestEntity();
            TestDetail.BranchInfo = new BranchEntity();
            TestDetail.StudentInfo = new StudentEntity();
            var httpRequest = HttpContext.Current.Request;
            TestDetail.TestInfo.TestID = TestID;
            TestDetail.BranchInfo.BranchID = BranchID;
            TestDetail.StudentInfo.StudentID = StudentID;
            TestDetail.Remarks = "";
            TestDetail.Status = Status.HasValue ? Status.Value : 0;
            TestDetail.SubmitDate = SubmitDate;
            TestDetail.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            TestDetail.Transaction = new TransactionEntity()
            {
                CreatedBy = CreateBy,
                CreatedId = CreateId,
                CreatedDate = DateTime.Now,
            };
            var data1 = this._testService.RemoveTestAnswerSheetdetail(TestID, StudentID);
            if (httpRequest.Files.Count > 0)
            {
                try
                {
                    for (int file = 0; file < httpRequest.Files.Count; file++)
                    {
                        string fileName;
                        string extension;
                        string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                        // for live server
                        string UpdatedPath = currentDir.Replace("AshirvadAPI", "ashivadproduct");
                        // for local server
                        //string UpdatedPath = currentDir.Replace("Ashirvad.API", "Ashirvad.Web");
                        var postedFile = httpRequest.Files[file];
                        string randomfilename = Common.Common.RandomString(20);
                        extension = Path.GetExtension(postedFile.FileName);
                        fileName = Path.GetFileName(postedFile.FileName);
                        string _Filepath = "/TestDetailImage/" + randomfilename + extension;
                        string _Filepath1 = "TestDetailImage/" + randomfilename + extension;
                        var filePath = HttpContext.Current.Server.MapPath("~/TestDetailImage/" + randomfilename + extension);
                        string _path = UpdatedPath + _Filepath1;
                        postedFile.SaveAs(_path);
                        TestDetail.AnswerSheetName = fileName;
                        TestDetail.FilePath = _Filepath;
                        var data = this._testService.StudentAnswerSheetMaintenance(TestDetail);
                        Response = data.Result;
                    }
                    result.Data = null;
                    result.Completed = false;
                    if (Response.AnsSheetID > 0)
                    {
                        result.Data = Response;
                        result.Completed = true;
                        result.Message = "Test Uploaded Successfully!!";
                    }

                }
                catch (Exception ex)
                {
                    result.Data = Response;
                    result.Completed = false;
                    result.Message = ex.Message.ToString();
                }

            }
            else
            {
                var data = this._testService.StudentAnswerSheetMaintenance(TestDetail);
                Response = data.Result;
                result.Data = null;
                result.Completed = false;
                if (Response.AnsSheetID > 0)
                {
                    result.Data = Response;
                    result.Completed = true;
                    result.Message = "Test Uploaded Successfully!!";
                }
            }
            return result;


        }

        [Route("GetAllDocLinks")]
        [HttpGet]
        public OperationResult<List<TestPaperEntity>> GetAllDocLinks(long branchID, long stdID, int batchTime)
        {
            var data = this._testService.GetAllTestDocLinks(branchID, stdID,batchTime);
            return data.Result;
           
        }

        [Route("GetAnswerSheetdata")]
        [HttpGet]
        public OperationResult<List<StudentAnswerSheetEntity>> GetAnswerSheetdata(long testID)
        {
            var data = this._testService.GetAnswerSheetdata(testID);
            OperationResult<List<StudentAnswerSheetEntity>> result = new OperationResult<List<StudentAnswerSheetEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;

        }

        [Route("UpdateAnsdetails")]
        [HttpGet]
        public OperationResult<StudentAnswerSheetEntity> UpdateAnsdetails(long TestID, long StudentID, string Remark, int Status, string CreatedBy, long CreatedId)
        {
            OperationResult<StudentAnswerSheetEntity> result = new OperationResult<StudentAnswerSheetEntity>();
            StudentAnswerSheetEntity answerSheetEntity = new StudentAnswerSheetEntity();
            answerSheetEntity.TestInfo = new TestEntity();
            answerSheetEntity.StudentInfo = new StudentEntity();
            answerSheetEntity.TestInfo.TestID = TestID;
            answerSheetEntity.StudentInfo.StudentID = StudentID;
            answerSheetEntity.Remarks = Remark;
            answerSheetEntity.Status = Status;
            var result1 = _testService.Ansdetailupdate(answerSheetEntity).Result;
            if (result1.AnsSheetID > 0)
            {
                result.Completed = true;
                result.Message = "Updated Successfully!!";
            }
            else
            {
                result.Completed = false;
                result.Message = "Failed To Updated!!";
            }
            return result;
        }

        [HttpGet]
        [Route("DownloadZipFile/{TestID}/{StudentID}/{Homework}/{Student}/{Class}")]
        public OperationResult<HomeworkEntity> SaveZipFile(long TestID, long StudentID, string Homework, string Student, string Class)
        {
            //hi = 11;
            //si = 2;
            OperationResult<HomeworkEntity> result = new OperationResult<HomeworkEntity>();
            string[] array = new string[2];
            string FileName = "";
            try
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                var homeworks = _testService.GetAnswerFiles(TestID).Result;
                //string randomfilename = Common.Common.RandomString(20);
                string randomfilename = "HomeWork_" + Homework + "_Student_" + Student + "_Class_" + Class;
                FileName = "/ZipFiles/TestPaperDetails/" + randomfilename + ".zip";
                if (homeworks.Count > 0)
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath
                                   ("~/ZipFiles/TestPaperDetails/" + randomfilename + ".zip")))
                    {
                        File.Delete(HttpContext.Current.Server.MapPath
                                      ("~/ZipFiles/TestPaperDetails/" + randomfilename + ".zip"));
                    }

                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                        foreach (var item in homeworks)
                        {
                            //string filePath = HttpContext.Current.Server.MapPath(item.FilePath);
                            //zip.AddFile(filePath, "Files");
                            using (var client = new WebClient())
                            {
                                var buffer = client.DownloadData("http://highpack-001-site12.dtempurl.com" + item.FilePath);
                                zip.AddEntry(item.AnswerSheetName, buffer);
                            }
                        }

                        string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                        // for live server
                        string UpdatedPath = currentDir.Replace("AshirvadAPI", "ashivadproduct");
                        // for local server
                        //string UpdatedPath = currentDir.Replace("Ashirvad.API", "Ashirvad.Web");
                        //Save the Zip File to MemoryStream.
                        string _Filepath1 = "ZipFiles/TestPaperDetails/" + randomfilename + ".zip";
                        var filePath = HttpContext.Current.Server.MapPath("~/ZipFiles/TestPaperDetails/" + randomfilename + ".zip");
                        string _path = UpdatedPath + _Filepath1;
                        zip.Save(_path);
                    }

                    result.Data = new HomeworkEntity()
                    {
                        FilePath = "http://highpack-001-site12.dtempurl.com" + FileName
                    };
                    result.Completed = true;
                    result.Message = "Success";
                }
                else
                {
                    result.Completed = false;
                    result.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                result.Completed = false;
                result.Message = ex.ToString();
            }
            return result;
        }

    }
}
