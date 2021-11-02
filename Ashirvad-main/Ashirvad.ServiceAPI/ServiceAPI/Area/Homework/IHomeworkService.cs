using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Homework
{
    public interface IHomeworkService
    {
        Task<HomeworkEntity> HomeworkMaintenance(HomeworkEntity homework);
        Task<List<HomeworkEntity>> GetAllHomeworkByBranch(long branchID, long stdID = 0, int batchTime = 0);
        Task<List<HomeworkEntity>> GetAllHomeworkByBranchStudent(long branchID, long stdID, int batchTime, long studentId = 0);
        Task<List<HomeworkEntity>> GetAllHomeworkWithoutContentByBranch(long branchID, long stdID = 0);
        Task<HomeworkEntity> GetHomeworkByHomeworkID(long hwID);
        bool RemoveHomework(long hwID, string lastupdatedby);
        Task<List<HomeworkEntity>> GetAllHomeworks(DateTime hwDate, string searchParam);
    }
}
