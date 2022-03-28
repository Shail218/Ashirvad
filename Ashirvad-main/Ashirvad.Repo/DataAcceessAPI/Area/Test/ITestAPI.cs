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
        Task<ResponseModel> TestMaintenance(TestEntity testInfo);
        Task<List<TestEntity>> GetAllTestByBranch(long branchID);
        Task<List<TestEntity>> GetAllTestByBranchType(long branchID,long BatchType);
        Task<TestEntity> GetTestByTestID(long testID);
        ResponseModel RemoveTest(long testID, string lastupdatedby, bool removePaper);
        Task<List<TestEntity>> GetAllTestByBranchAndStandard(long branchID, long courseID, long stdID, int batchTime);
        Task<ResponseModel> TestPaperMaintenance(TestPaperEntity paperInfo);
        Task<List<TestPaperEntity>> GetAllTestPapaerByTest(long testID);
        Task<List<TestPaperEntity>> GetAllTestPapaerWithoutContentByTest(long testID);
        Task<TestPaperEntity> GetTestPaperByPaperID(long paperID);
        Task<List<TestEntity>> GetTestPaperChecking(long paperID);
        Task<List<TestPaperEntity>> GetAllTestPapaerByBranchStdDate(long branchID,long courseid, long stdID, DateTime dt, int batchTime);
        ResponseModel RemoveTestPaper(long paperID, string lastupdatedby);

        Task<ResponseModel> AnswerSheetMaintenance(StudentAnswerSheetEntity studAnswerSheet);
        Task<List<StudentAnswerSheetEntity>> GetAllTestAnswerSheetByTestStudent(long testID);
        Task<List<StudentAnswerSheetEntity>> GetAllTestAnswerSheetWithoutContentByTestStudent(long testID);
        Task<List<StudentAnswerSheetEntity>> GetAllAnsSheetByTestStudentID(long testID, long studentID);
        Task<StudentAnswerSheetEntity> GetTestAnswerSheetPaperByAnswerSheetID(long ansID);
        ResponseModel RemoveAnswerSheet(long ansID, string lastupdatedby);
        Task<List<TestEntity>> GetAllTest(DateTime testDate, string searchParam);

        Task<ResponseModel> TestMaintenance(TestDetailEntity TestDetail);
        Task<List<TestPaperEntity>> GetAllTestDocLinks(long branchID,long courseid, long stdID, int batchTime);

        Task<TestEntity> GetTestDetails(long TestID,long SubjectID);
        ResponseModel RemoveTestAnswerSheetdetail(long TestID, long studid);
        Task<List<StudentAnswerSheetEntity>> GetallAnswerSheetData(long testID);

        Task<List<StudentAnswerSheetEntity>> GetStudentAnsFile(long TestID);
        Task<long> AnsDetailUpdate(StudentAnswerSheetEntity studentAnswerSheet);

        Task<List<TestEntity>> TestDateDDL(long branchID, long stdID, long courseid, int batchTime);
        Task<List<TestEntity>> GetAllTestByBranchAPI(long branchID);
        Task<List<TestEntity>> GetAllCustomTest(DataTableAjaxPostModel model, long branchID);
    }
}
