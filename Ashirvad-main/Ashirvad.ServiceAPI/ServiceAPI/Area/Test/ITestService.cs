using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Test
{
    public interface ITestService
    {
        Task<ResponseModel> TestMaintenance(TestEntity testInfo);
        Task<OperationResult<List<TestEntity>>> GetAllTestByBranch(long branchID);
        Task<OperationResult<TestEntity>> GetTestByTestID(long testID);
        ResponseModel RemoveTest(long testID, string lastupdatedby, bool removePaper);
        Task<ResponseModel> TestPaperMaintenance(TestPaperEntity paperInfo);
        Task<List<TestPaperEntity>> GetAllTestPapaerByTest(long testID);
        Task<OperationResult<List<TestPaperEntity>>> GetAllTestPapaerWithoutContentByTest(long testID);
        Task<TestPaperEntity> GetTestPaperByPaperID(long paperID);
        Task<List<TestEntity>> GetTestPaperChecking(long paperID);
        ResponseModel RemoveTestPaper(long paperID, string lastupdatedby);

        Task<ResponseModel> StudentAnswerSheetMaintenance(StudentAnswerSheetEntity ansSheetInfo);
        Task<List<StudentAnswerSheetEntity>> GetAllAnswerSheetByTest(long testID);
        Task<List<StudentAnswerSheetEntity>> GetAllAnswerSheetWithoutContentByTest(long testID);
        Task<List<StudentAnswerSheetEntity>> GetAllAnsSheetByTestStudentID(long testID, long studentID);
        Task<StudentAnswerSheetEntity> GetAnswerSheetByID(long ansID);
        ResponseModel RemoveAnswerSheet(long ansID, string lastupdatedby);
        ResponseModel RemoveTestAndPaper(long testID, string lastUpdatedBy);

        Task<OperationResult<List<TestEntity>>> GetAllTestByBranchAndStandard(long branchID, long courseID, long stdID, int batchTime);
        Task<OperationResult<List<TestPaperEntity>>> GetAllTestPapaerByBranchStdDate(long branchID,long courseid, long stdID, DateTime dt, int batchTime);

        Task<OperationResult<List<TestEntity>>> GetAllTest(DateTime testDate, string searchParam);
        Task<List<StudentAnswerSheetEntity>> BulkStudentAnswerSheetMaintenance(List<StudentAnswerSheetEntity> ansSheetInfo);

        Task<ResponseModel> TestdetailMaintenance(TestDetailEntity testInfo);
        Task<OperationResult<List<TestPaperEntity>>> GetAllTestDocLinks(long branchID,long courseid, long stdID, int batchTime);
        Task<List<TestEntity>> GetAllCustomTest(DataTableAjaxPostModel model, long branchID);

        #region New
        Task<TestEntity> GetTestDetails(long testID,long SubjectID);
        ResponseModel RemoveTestAnswerSheetdetail(long TestID, long studid);
        #endregion

        Task<List<StudentAnswerSheetEntity>> GetAnswerSheetdata(long testID);
        Task<List<StudentAnswerSheetEntity>> GetAnswerFiles(long hwID);

        Task<ResponseModel> Ansdetailupdate(StudentAnswerSheetEntity answerSheetEntity);
        Task<OperationResult<List<TestEntity>>> TestDateDDL(long branchID, long stdID, long courseid, int batchTime);
        Task<OperationResult<List<TestEntity>>> GetAllTestByBranchAPI(long branchID);
    }
}
