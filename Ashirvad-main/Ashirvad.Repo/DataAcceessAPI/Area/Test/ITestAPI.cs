using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Test
{
    public interface ITestAPI
    {
        Task<long> TestMaintenance(TestEntity testInfo);

        Task<List<TestEntity>> GetAllTestByBranch(long branchID);
        Task<List<TestEntity>> GetAllTestByBranchType(long branchID,long BatchType);
        Task<TestEntity> GetTestByTestID(long testID);
        bool RemoveTest(long testID, string lastupdatedby, bool removePaper);
        Task<List<TestEntity>> GetAllTestByBranchAndStandard(long branchID, long stdID, int batchTime);
        Task<long> TestPaperMaintenance(TestPaperEntity paperInfo);
        Task<List<TestPaperEntity>> GetAllTestPapaerByTest(long testID);
        Task<List<TestPaperEntity>> GetAllTestPapaerWithoutContentByTest(long testID);
        Task<TestPaperEntity> GetTestPaperByPaperID(long paperID);
        Task<List<TestPaperEntity>> GetAllTestPapaerByBranchStdDate(long branchID, long stdID, DateTime dt, int batchTime);
        bool RemoveTestPaper(long paperID, string lastupdatedby);

        Task<long> AnswerSheetMaintenance(StudentAnswerSheetEntity studAnswerSheet);
        Task<List<StudentAnswerSheetEntity>> GetAllTestAnswerSheetByTestStudent(long testID);
        Task<List<StudentAnswerSheetEntity>> GetAllTestAnswerSheetWithoutContentByTestStudent(long testID);
        Task<List<StudentAnswerSheetEntity>> GetAllAnsSheetByTestStudentID(long testID, long studentID);
        Task<StudentAnswerSheetEntity> GetTestAnswerSheetPaperByAnswerSheetID(long ansID);
        bool RemoveAnswerSheet(long ansID, string lastupdatedby);
        Task<List<TestEntity>> GetAllTest(DateTime testDate, string searchParam);

        Task<long> TestMaintenance(TestDetailEntity TestDetail);
        Task<List<TestPaperEntity>> GetAllTestDocLinks(long branchID, long stdID, int batchTime);

        Task<TestEntity> GetTestDetails(long TestID,long SubjectID);
        bool RemoveTestAnswerSheetdetail(long TestID, long studid);
    }
}
