using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Homework
{
    public interface IHomeworkAPI
    {
        Task<long> HomeworkMaintenance(HomeworkEntity homeworkInfo);
        Task<List<HomeworkEntity>> GetAllHomeworkByBranch(long branchID, long stdID, int batchTime);
        Task<List<HomeworkEntity>> GetAllHomeworkByBranchStudent(long branchID, long stdID, int batchTime, long studentId = 0);
        Task<List<HomeworkEntity>> GetAllHomeworkWithoutContentByBranch(long branchID, long stdID);
        Task<HomeworkEntity> GetHomeworkByHomeworkID(long homeworkID);
        bool RemoveHomework(long homeworkID, string lastupdatedby);
        Task<List<HomeworkEntity>> GetAllHomeworks(DateTime hwDate, string searchParam);
    }
}
