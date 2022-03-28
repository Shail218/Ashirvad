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
        Task<ResponseModel> HomeworkMaintenance(HomeworkEntity homework);
        Task<List<HomeworkEntity>> GetAllHomeworkByBranch(long branchID, long stdID = 0, int batchTime = 0);
        Task<List<HomeworkEntity>> GetAllHomeworkByBranchStudent(long branchID,long courseid, long stdID, int batchTime, long studentId = 0);
        Task<List<HomeworkEntity>> GetAllHomeworkWithoutContentByBranch(long branchID, long stdID = 0);
        Task<HomeworkEntity> GetHomeworkByHomeworkID(long hwID);
        Task<List<HomeworkEntity>> GetStudentHomeworkChecking(long hwID);
        ResponseModel RemoveHomework(long hwID, string lastupdatedby);
        Task<List<HomeworkEntity>> GetAllHomeworks(DateTime hwDate, string searchParam);
        Task<List<HomeworkEntity>> GetAllCustomHomework(DataTableAjaxPostModel model, long branchID);
        Task<List<HomeworkEntity>> GetHomeworkdetailsFiles(long hwID);
    }
}
