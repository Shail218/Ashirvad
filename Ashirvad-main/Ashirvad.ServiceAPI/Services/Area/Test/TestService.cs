using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Test;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Test;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area.Test
{
    public class TestService : ITestService
    {
        private readonly ITestAPI _testContext;
        public TestService(ITestAPI testContext)
        {
            _testContext = testContext;
        }

        public async Task<ResponseModel> TestMaintenance(TestEntity testInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            TestEntity paper = new TestEntity();
            try
            {
                responseModel = await _testContext.TestMaintenance(testInfo);
                //paper = testInfo;
                //paper.TestID = data;
                //return paper;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<OperationResult<List<TestEntity>>> GetAllTestByBranch(long branchID)
        {
            try
            {
                OperationResult<List<TestEntity>> paper = new OperationResult<List<TestEntity>>();
                paper.Data = await _testContext.GetAllTestByBranch(branchID);
                paper.Completed = true;
                return paper;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<TestEntity>> GetAllCustomTest(DataTableAjaxPostModel model, long branchID)
        {
            try
            {
                return await this._testContext.GetAllCustomTest(model, branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<List<TestEntity>>> GetAllTestByBranchAPI(long branchID)
        {
            try
            {
                OperationResult<List<TestEntity>> paper = new OperationResult<List<TestEntity>>();
                paper.Data = await _testContext.GetAllTestByBranchAPI(branchID);
                paper.Completed = true;
                return paper;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<List<TestEntity>>> GetAllTestByBranchAndStandard(long branchID, long courseID,long stdID, int batchTime)
        {
            try
            {
                OperationResult<List<TestEntity>> paper = new OperationResult<List<TestEntity>>();
                paper.Data = await _testContext.GetAllTestByBranchAndStandard(branchID, courseID,stdID, batchTime);
                paper.Completed = true;
                return paper;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<List<TestEntity>>> TestDateDDL(long branchID, long stdID, long courseid, int batchTime)
        {
            try
            {
                OperationResult<List<TestEntity>> paper = new OperationResult<List<TestEntity>>();
                paper.Data = await _testContext.TestDateDDL(branchID, stdID, courseid,batchTime);
                paper.Completed = true;
                return paper;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<TestEntity>> GetTestByTestID(long testID)
        {
            try
            {
                OperationResult<TestEntity> lib = new OperationResult<TestEntity>();
                lib.Data = await _testContext.GetTestByTestID(testID);
                lib.Completed = true;
                return lib;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveTest(long testID, string lastupdatedby, bool removePaper = false)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                return this._testContext.RemoveTest(testID, lastupdatedby, removePaper);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<ResponseModel> TestPaperMaintenance(TestPaperEntity paperInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            TestPaperEntity paper = new TestPaperEntity();
            try
            {
                responseModel = await _testContext.TestPaperMaintenance(paperInfo);
                //long TestID = await _testContext.TestPaperMaintenance(paperInfo);
                //if (TestID > 0)
                //{
                //    paper = paperInfo;
                //    paper.TestID = TestID;
                //}              
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }



        public async Task<List<TestPaperEntity>> GetAllTestPapaerByTest(long testID)
        {
            try
            {
                var data = await _testContext.GetAllTestPapaerByTest(testID);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<List<TestPaperEntity>>> GetAllTestPapaerByBranchStdDate(long branchID,long courseid, long stdID, DateTime dt, int batchTime)
        {
            try
            {
                var data = await _testContext.GetAllTestPapaerByBranchStdDate(branchID,courseid, stdID, dt, batchTime);
                OperationResult<List<TestPaperEntity>> res = new OperationResult<List<TestPaperEntity>>();
                res.Data = data;
                res.Completed = true;
                return res;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<List<TestPaperEntity>>> GetAllTestDocLinks(long branchID,long courseid, long stdID, int batchTime)
        {
            try
            {
                var data = await _testContext.GetAllTestDocLinks(branchID,courseid, stdID, batchTime);
                OperationResult<List<TestPaperEntity>> res = new OperationResult<List<TestPaperEntity>>();
                res.Data = data;
                res.Completed = true;
                return res;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }


        public async Task<OperationResult<List<TestEntity>>> GetAllTest(DateTime testDate, string searchParam)
        {
            try
            {
                var data = await _testContext.GetAllTest(testDate, searchParam);
                OperationResult<List<TestEntity>> res = new OperationResult<List<TestEntity>>();
                res.Data = data;
                res.Completed = true;
                return res;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<List<TestPaperEntity>>> GetAllTestPapaerWithoutContentByTest(long testID)
        {
            try
            {
                OperationResult<List<TestPaperEntity>> paper = new OperationResult<List<TestPaperEntity>>();
                paper.Data = await _testContext.GetAllTestPapaerWithoutContentByTest(testID);
                paper.Completed = true;
                return paper;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<TestPaperEntity> GetTestPaperByPaperID(long paperID)
        {
            try
            {
                var data = await _testContext.GetTestPaperByPaperID(paperID);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<TestEntity>> GetTestPaperChecking(long paperID)
        {
            try
            {
                var data = await _testContext.GetTestPaperChecking(paperID);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveTestPaper(long paperID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = _testContext.RemoveTestPaper(paperID, lastupdatedby);
                //var data = _testContext.RemoveTestPaper(paperID, lastupdatedby);
                //return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public ResponseModel RemoveTestAndPaper(long testID, string lastUpdatedBy)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = this.RemoveTest(testID, lastUpdatedBy, true);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return responseModel;
        }

        #region - Student Answer Sheet - 
        public async Task<ResponseModel> StudentAnswerSheetMaintenance(StudentAnswerSheetEntity ansSheetInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            StudentAnswerSheetEntity paper = new StudentAnswerSheetEntity();
            try
            {
                //if (ansSheetInfo.FileInfo != null)
                //{
                //    ansSheetInfo.AnswerSheetContent = Common.Common.ReadFully(ansSheetInfo.FileInfo.InputStream);
                //    ansSheetInfo.AnswerSheetName = Path.GetFileName(ansSheetInfo.FileInfo.FileName);
                //}
                //else
                //{
                //    ansSheetInfo.AnswerSheetContent = Convert.FromBase64String(ansSheetInfo.AnswerSheetContentText);
                //}

                responseModel = await _testContext.AnswerSheetMaintenance(ansSheetInfo);

                //if (data > 0)
                //{
                //    if (!string.IsNullOrEmpty(Common.Common.GetStringConfigKey("DocDirectory")))
                //    {
                //        Common.Common.SaveFile(ansSheetInfo.AnswerSheetContent, ansSheetInfo.AnswerSheetName, "StudentAnswerSheet\\");
                //    }
                //}

                // //paper = ansSheetInfo;
               // //paper.AnsSheetID = data;
               // //return paper;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<List<StudentAnswerSheetEntity>> BulkStudentAnswerSheetMaintenance(List<StudentAnswerSheetEntity> ansSheetInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            List<StudentAnswerSheetEntity> successStudents = new List<StudentAnswerSheetEntity>();
            foreach (var item in ansSheetInfo)
            {
                
                StudentAnswerSheetEntity paper = new StudentAnswerSheetEntity();
                try
                {
                    if (item.FileInfo != null)
                    {
                        item.AnswerSheetContent = Common.Common.ReadFully(item.FileInfo.InputStream);
                        item.AnswerSheetName = Path.GetFileName(item.FileInfo.FileName);
                    }
                    else
                    {
                        item.AnswerSheetContent = Convert.FromBase64String(item.AnswerSheetContentText);
                    }

                    responseModel = await _testContext.AnswerSheetMaintenance(item);
                    //var data = await _testContext.AnswerSheetMaintenance(item);
                    if (responseModel.Status)
                    {
                        var da = (StudentAnswerSheetEntity)responseModel.Data;
                        long data = da.AnsSheetID;
                        if (data > 0)
                        {
                            if (!string.IsNullOrEmpty(Common.Common.GetStringConfigKey("DocDirectory")))
                            {
                                Common.Common.SaveFile(item.AnswerSheetContent, item.AnswerSheetName, "StudentAnswerSheet\\");
                            }

                        }
                      

                        paper = item;
                        paper.AnsSheetID = data;
                        successStudents.Add(paper);
                    }
                   
                }
                catch (Exception ex)
                {
                    EventLogger.WriteEvent(Logger.Severity.Error, ex);
                }

            }
            return successStudents;
            //return successStudents;
        }


        public async Task<List<StudentAnswerSheetEntity>> GetAllAnswerSheetByTest(long testID)
        {
            try
            {
                var data = await _testContext.GetAllTestAnswerSheetByTestStudent(testID);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<StudentAnswerSheetEntity>> GetAllAnswerSheetWithoutContentByTest(long testID)
        {
            try
            {
                var data = await _testContext.GetAllTestAnswerSheetWithoutContentByTestStudent(testID);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        public async Task<List<StudentAnswerSheetEntity>> GetAllAnsSheetByTestStudentID(long testID, long studentID)
        {
            try
            {
                var data = await _testContext.GetAllAnsSheetByTestStudentID(testID,studentID);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        public async Task<StudentAnswerSheetEntity> GetAnswerSheetByID(long ansID)
        {
            try
            {
                var data = await _testContext.GetTestAnswerSheetPaperByAnswerSheetID(ansID);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveAnswerSheet(long ansID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = _testContext.RemoveAnswerSheet(ansID, lastupdatedby);
                //return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return responseModel;
            //return false;
        }

        #endregion

        public async Task<ResponseModel> TestdetailMaintenance(TestDetailEntity TestDetail)
        {
            ResponseModel responseModel = new ResponseModel();
            TestDetailEntity TestDetailEntity = new TestDetailEntity();
            try
            {
                responseModel = await _testContext.TestMaintenance(TestDetail);
                //TestDetailEntity.TestDetailID = data;
                //return TestDetailEntity;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<TestEntity> GetTestDetails(long testID,long SubjectID)
        {
            try
            {
                OperationResult<TestEntity> lib = new OperationResult<TestEntity>();
                lib.Data = await _testContext.GetTestDetails(testID, SubjectID);
                lib.Completed = true;
                return lib.Data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveTestAnswerSheetdetail(long TestID, long studid)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = _testContext.RemoveTestAnswerSheetdetail(TestID, studid);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }
        public async Task<List<StudentAnswerSheetEntity>> GetAnswerSheetdata(long testID)
        {
            try
            {
                var data = await _testContext.GetallAnswerSheetData(testID);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<StudentAnswerSheetEntity>> GetAnswerFiles(long TestID)
        {
            try
            {
                var data = await _testContext.GetStudentAnsFile(TestID);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<ResponseModel> Ansdetailupdate(StudentAnswerSheetEntity studentAnswerSheet)
        {
            ResponseModel answerSheetEntity = new ResponseModel();
            try
            {
                answerSheetEntity = await _testContext.AnsDetailUpdate(studentAnswerSheet);
               
                return answerSheetEntity;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return answerSheetEntity;
        }
    }
}
