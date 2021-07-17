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

namespace Ashirvad.ServiceAPI.Services.Area.Test
{
    public class TestService : ITestService
    {
        private readonly ITestAPI _testContext;
        public TestService(ITestAPI testContext)
        {
            _testContext = testContext;
        }

        public async Task<TestEntity> TestMaintenance(TestEntity testInfo)
        {
            TestEntity paper = new TestEntity();
            try
            {
                var data = await _testContext.TestMaintenance(testInfo);
                paper = testInfo;
                paper.TestID = data;
                return paper;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return paper;
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

        public async Task<OperationResult<List<TestEntity>>> GetAllTestByBranchAndStandard(long branchID, long stdID, int batchTime)
        {
            try
            {
                OperationResult<List<TestEntity>> paper = new OperationResult<List<TestEntity>>();
                paper.Data = await _testContext.GetAllTestByBranchAndStandard(branchID, stdID, batchTime);
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

        public bool RemoveTest(long testID, string lastupdatedby, bool removePaper = false)
        {
            try
            {
                return this._testContext.RemoveTest(testID, lastupdatedby, removePaper);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

        public async Task<TestPaperEntity> TestPaperMaintenance(TestPaperEntity paperInfo)
        {
            TestPaperEntity paper = new TestPaperEntity();
            try
            {
                if (paperInfo.FileInfo != null)
                {
                    paperInfo.DocContent = Common.Common.ReadFully(paperInfo.FileInfo.InputStream);
                    paperInfo.FileName = Path.GetFileName(paperInfo.FileInfo.FileName);
                }
                else
                {
                    paperInfo.DocContent = Convert.FromBase64String(paperInfo.DocContentText);
                }
                

                var data = await _testContext.TestPaperMaintenance(paperInfo);
                if (data > 0)
                {
                    if (!string.IsNullOrEmpty(Common.Common.GetStringConfigKey("DocDirectory")))
                    {
                        Common.Common.SaveFile(paperInfo.DocContent, paperInfo.FileName, "TestPaper\\");
                    }
                }

                paper = paperInfo;
                paper.TestID = data;
                return paper;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return paper;
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

        public async Task<OperationResult<List<TestPaperEntity>>> GetAllTestPapaerByBranchStdDate(long branchID, long stdID, DateTime dt, int batchTime)
        {
            try
            {
                var data = await _testContext.GetAllTestPapaerByBranchStdDate(branchID, stdID, dt, batchTime);
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

        public async Task<List<TestPaperEntity>> GetAllTestPapaerWithoutContentByTest(long testID)
        {
            try
            {
                var data = await _testContext.GetAllTestPapaerWithoutContentByTest(testID);
                return data;
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

        public bool RemoveTestPaper(long paperID, string lastupdatedby)
        {
            try
            {
                var data = _testContext.RemoveTestPaper(paperID, lastupdatedby);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

        #region - Student Answer Sheet - 
        public async Task<StudentAnswerSheetEntity> StudentAnswerSheetMaintenance(StudentAnswerSheetEntity ansSheetInfo)
        {
            StudentAnswerSheetEntity paper = new StudentAnswerSheetEntity();
            try
            {
                if (ansSheetInfo.FileInfo != null)
                {
                    ansSheetInfo.AnswerSheetContent = Common.Common.ReadFully(ansSheetInfo.FileInfo.InputStream);
                    ansSheetInfo.AnswerSheetName = Path.GetFileName(ansSheetInfo.FileInfo.FileName);
                }
                else
                {
                    ansSheetInfo.AnswerSheetContent = Convert.FromBase64String(ansSheetInfo.AnswerSheetContentText);
                }


                var data = await _testContext.AnswerSheetMaintenance(ansSheetInfo);
                if (data > 0)
                {
                    if (!string.IsNullOrEmpty(Common.Common.GetStringConfigKey("DocDirectory")))
                    {
                        Common.Common.SaveFile(ansSheetInfo.AnswerSheetContent, ansSheetInfo.AnswerSheetName, "StudentAnswerSheet\\");
                    }
                }

                paper = ansSheetInfo;
                paper.AnsSheetID = data;
                return paper;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return paper;
        }

        public async Task<List<StudentAnswerSheetEntity>> BulkStudentAnswerSheetMaintenance(List<StudentAnswerSheetEntity> ansSheetInfo)
        {
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


                    var data = await _testContext.AnswerSheetMaintenance(item);
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
                catch (Exception ex)
                {
                    EventLogger.WriteEvent(Logger.Severity.Error, ex);
                }

            }
            return successStudents;
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

        public bool RemoveAnswerSheet(long ansID, string lastupdatedby)
        {
            try
            {
                var data = _testContext.RemoveAnswerSheet(ansID, lastupdatedby);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }
        #endregion
    }
}
