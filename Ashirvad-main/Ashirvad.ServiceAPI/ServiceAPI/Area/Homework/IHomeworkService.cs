using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Homework
{
    public interface IHomeworkService
    {
        Task<HomeworkEntity> HomeworkMaintenance(HomeworkEntity homework);
        Task<List<HomeworkEntity>> GetAllHomeworkByBranch(long branchID, string financialyear, long stdID = 0, int batchTime = 0);
        Task<List<HomeworkEntity>> GetAllHomeworkByBranchStudent(long branchID,long courseid, long stdID, int batchTime, string financialyear, long studentId = 0);
        Task<List<HomeworkEntity>> GetAllHomeworkWithoutContentByBranch(long branchID, string financialyear, long stdID = 0);
        Task<HomeworkEntity> GetHomeworkByHomeworkID(long hwID, string financialyear);
        Task<List<HomeworkEntity>> GetStudentHomeworkChecking(long hwID, string financialyear);
        bool RemoveHomework(long hwID, string lastupdatedby);
        Task<List<HomeworkEntity>> GetAllHomeworks(DateTime hwDate, string searchParam, string financialyear);
        Task<List<HomeworkEntity>> GetAllCustomHomework(DataTableAjaxPostModel model, long branchID, string financialyear);
        Task<List<HomeworkEntity>> GetHomeworkdetailsFiles(long hwID, string financialyear);
    }
}
