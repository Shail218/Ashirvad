using Ashirvad.Data;
using Ashirvad.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.DashboardChart
{
    public interface IDashboardChartAPI
    {
        Task<List<ChartBranchEntity>> AllBranchWithCount();
        Task<List<BranchStandardEntity>> AllBranchStandardWithCountByBranch(long branchid);
        Task<List<DataPoints>> GetStudentAttendanceDetails(long studentid);
        Task<List<ChartBranchEntity>> GetTotalCountList(long studentid);
        Task<List<DataPoints>> GetHomeworkByStudent(long branchid, long studentid);
        Task<List<TestDataPoints>> GetTestdetailsByStudent(long branchid, long studentid);
        Task<List<MarksEntity>> GetTestDetailsByStudent(DataTableAjaxPostModel model,long studentid, long subjectid);
        Task<List<HomeworkDetailEntity>> GetHomeworkDetailsByStudent(DataTableAjaxPostModel model,long studentid, long subjectid);
    }
}
