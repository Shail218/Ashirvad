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
        Task<TestEntity> TestMaintenance(TestEntity testInfo);
        Task<OperationResult<List<TestEntity>>> GetAllTestByBranch(long branchID, string financialyear);
        Task<OperationResult<TestEntity>> GetTestByTestID(long testID, string financialyear);
        bool RemoveTest(long testID, string lastupdatedby, bool removePaper);
        Task<TestPaperEntity> TestPaperMaintenance(TestPaperEntity paperInfo);
        Task<List<TestPaperEntity>> GetAllTestPapaerByTest(long testID, string financialyear);
        Task<OperationResult<List<TestPaperEntity>>> GetAllTestPapaerWithoutContentByTest(long testID, string financialyear);
        Task<TestPaperEntity> GetTestPaperByPaperID(long paperID, string financialyear);
        Task<List<TestEntity>> GetTestPaperChecking(long paperID, string financialyear);
        bool RemoveTestPaper(long paperID, string lastupdatedby);

        Task<StudentAnswerSheetEntity> StudentAnswerSheetMaintenance(StudentAnswerSheetEntity ansSheetInfo);
        Task<List<StudentAnswerSheetEntity>> GetAllAnswerSheetByTest(long testID, string financialyear);
        Task<List<StudentAnswerSheetEntity>> GetAllAnswerSheetWithoutContentByTest(long testID, string financialyear);
        Task<List<StudentAnswerSheetEntity>> GetAllAnsSheetByTestStudentID(long testID, long studentID, string financialyear);
        Task<StudentAnswerSheetEntity> GetAnswerSheetByID(long ansID, string financialyear);
        bool RemoveAnswerSheet(long ansID, string lastupdatedby);
        bool RemoveTestAndPaper(long testID, string lastUpdatedBy);

        Task<OperationResult<List<TestEntity>>> GetAllTestByBranchAndStandard(long branchID, long courseID, long stdID, int batchTime, string financialyear);
        Task<OperationResult<List<TestPaperEntity>>> GetAllTestPapaerByBranchStdDate(long branchID,long courseid, long stdID, DateTime dt, int batchTime, string financialyear);

        Task<OperationResult<List<TestEntity>>> GetAllTest(DateTime testDate, string searchParam, string financialyear);
        Task<List<StudentAnswerSheetEntity>> BulkStudentAnswerSheetMaintenance(List<StudentAnswerSheetEntity> ansSheetInfo);

        Task<TestDetailEntity> TestdetailMaintenance(TestDetailEntity testInfo);
        Task<OperationResult<List<TestPaperEntity>>> GetAllTestDocLinks(long branchID,long courseid, long stdID, int batchTime, string financialyear);
        Task<List<TestEntity>> GetAllCustomTest(DataTableAjaxPostModel model, long branchID, string financialyear);

        #region New
        Task<TestEntity> GetTestDetails(long testID,long SubjectID, string financialyear);
        bool RemoveTestAnswerSheetdetail(long TestID, long studid);
        #endregion

        Task<List<StudentAnswerSheetEntity>> GetAnswerSheetdata(long testID, string financialyear);
        Task<List<StudentAnswerSheetEntity>> GetAnswerFiles(long hwID, string financialyear);

        Task<StudentAnswerSheetEntity> Ansdetailupdate(StudentAnswerSheetEntity answerSheetEntity);
        Task<OperationResult<List<TestEntity>>> TestDateDDL(long branchID, long stdID, long courseid, int batchTime, string financialyear);
        Task<OperationResult<List<TestEntity>>> GetAllTestByBranchAPI(long branchID, string financialyear);
    }
}
