using Ashirvad.API.Filter;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Test;
using Ashirvad.ServiceAPI.Services.Area.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public OperationResult<bool> RemoveTest(long testID, string lastupdatedby)
        {
            var data = this._testService.RemoveTest(testID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("RemoveTestAndPaper")]
        [HttpPost]
        public OperationResult<bool> RemoveTest(long testID, string lastupdatedby, bool removePaper)
        {
            var data = this._testService.RemoveTest(testID, lastupdatedby, removePaper);
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
    }
}
