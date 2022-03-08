using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Test
{
    public interface ITestAPI
    {
        Task<long> TestMaintenance(TestEntity testInfo);

        Task<List<TestEntity>> GetAllTestByBranch(long branchID, string financialyear);
        Task<List<TestEntity>> GetAllTestByBranchType(long branchID,long BatchType, string financialyear);
        Task<TestEntity> GetTestByTestID(long testID, string financialyear);
        bool RemoveTest(long testID, string lastupdatedby, bool removePaper);
        Task<List<TestEntity>> GetAllTestByBranchAndStandard(long branchID, long courseID, long stdID, int batchTime, string financialyear);
        Task<long> TestPaperMaintenance(TestPaperEntity paperInfo);
        Task<List<TestPaperEntity>> GetAllTestPapaerByTest(long testID, string financialyear);
        Task<List<TestPaperEntity>> GetAllTestPapaerWithoutContentByTest(long testID, string financialyear);
        Task<TestPaperEntity> GetTestPaperByPaperID(long paperID, string financialyear);
        Task<List<TestEntity>> GetTestPaperChecking(long paperID, string financialyear);
        Task<List<TestPaperEntity>> GetAllTestPapaerByBranchStdDate(long branchID,long courseid, long stdID, DateTime dt, int batchTime, string financialyear);
        bool RemoveTestPaper(long paperID, string lastupdatedby);

        Task<long> AnswerSheetMaintenance(StudentAnswerSheetEntity studAnswerSheet);
        Task<List<StudentAnswerSheetEntity>> GetAllTestAnswerSheetByTestStudent(long testID, string financialyear);
        Task<List<StudentAnswerSheetEntity>> GetAllTestAnswerSheetWithoutContentByTestStudent(long testID, string financialyear);
        Task<List<StudentAnswerSheetEntity>> GetAllAnsSheetByTestStudentID(long testID, long studentID, string financialyear);
        Task<StudentAnswerSheetEntity> GetTestAnswerSheetPaperByAnswerSheetID(long ansID, string financialyear);
        bool RemoveAnswerSheet(long ansID, string lastupdatedby);
        Task<List<TestEntity>> GetAllTest(DateTime testDate, string searchParam, string financialyear);

        Task<long> TestMaintenance(TestDetailEntity TestDetail);
        Task<List<TestPaperEntity>> GetAllTestDocLinks(long branchID,long courseid, long stdID, int batchTime, string financialyear);

        Task<TestEntity> GetTestDetails(long TestID,long SubjectID, string financialyear);
        bool RemoveTestAnswerSheetdetail(long TestID, long studid);
        Task<List<StudentAnswerSheetEntity>> GetallAnswerSheetData(long testID, string financialyear);

        Task<List<StudentAnswerSheetEntity>> GetStudentAnsFile(long TestID, string financialyear);
        Task<long> AnsDetailUpdate(StudentAnswerSheetEntity studentAnswerSheet);

        Task<List<TestEntity>> TestDateDDL(long branchID, long stdID, long courseid, int batchTime, string financialyear);
        Task<List<TestEntity>> GetAllTestByBranchAPI(long branchID, string financialyear);
        Task<List<TestEntity>> GetAllCustomTest(DataTableAjaxPostModel model, long branchID, string financialyear);
    }
}
