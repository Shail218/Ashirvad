using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Test
{
    public interface ITestService
    {
        Task<TestEntity> TestMaintenance(TestEntity testInfo);
        Task<OperationResult<List<TestEntity>>> GetAllTestByBranch(long branchID);
        Task<OperationResult<TestEntity>> GetTestByTestID(long testID);
        bool RemoveTest(long testID, string lastupdatedby, bool removePaper);
        Task<TestPaperEntity> TestPaperMaintenance(TestPaperEntity paperInfo);
        Task<List<TestPaperEntity>> GetAllTestPapaerByTest(long testID);
        Task<List<TestPaperEntity>> GetAllTestPapaerWithoutContentByTest(long testID);
        Task<TestPaperEntity> GetTestPaperByPaperID(long paperID);
        Task<List<TestEntity>> GetTestPaperChecking(long paperID);
        bool RemoveTestPaper(long paperID, string lastupdatedby);

        Task<StudentAnswerSheetEntity> StudentAnswerSheetMaintenance(StudentAnswerSheetEntity ansSheetInfo);
        Task<List<StudentAnswerSheetEntity>> GetAllAnswerSheetByTest(long testID);
        Task<List<StudentAnswerSheetEntity>> GetAllAnswerSheetWithoutContentByTest(long testID);
        Task<List<StudentAnswerSheetEntity>> GetAllAnsSheetByTestStudentID(long testID, long studentID);
        Task<StudentAnswerSheetEntity> GetAnswerSheetByID(long ansID);
        bool RemoveAnswerSheet(long ansID, string lastupdatedby);
        bool RemoveTestAndPaper(long testID, string lastUpdatedBy);

        Task<OperationResult<List<TestEntity>>> GetAllTestByBranchAndStandard(long branchID, long stdID, int batchTime);
        Task<OperationResult<List<TestPaperEntity>>> GetAllTestPapaerByBranchStdDate(long branchID, long stdID, DateTime dt, int batchTime);

        Task<OperationResult<List<TestEntity>>> GetAllTest(DateTime testDate, string searchParam);
        Task<List<StudentAnswerSheetEntity>> BulkStudentAnswerSheetMaintenance(List<StudentAnswerSheetEntity> ansSheetInfo);

        Task<TestDetailEntity> TestdetailMaintenance(TestDetailEntity testInfo);
        Task<OperationResult<List<TestPaperEntity>>> GetAllTestDocLinks(long branchID, long stdID, int batchTime);

        #region New
        Task<TestEntity> GetTestDetails(long testID,long SubjectID);
        bool RemoveTestAnswerSheetdetail(long TestID, long studid);
        #endregion
    }
}
