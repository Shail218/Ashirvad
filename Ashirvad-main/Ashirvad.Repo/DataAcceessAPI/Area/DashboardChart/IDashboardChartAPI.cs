using Ashirvad.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.DashboardChart
{
    public interface IDashboardChartAPI
    {
        Task<List<ChartBranchEntity>> AllBranchWithCount();
        Task<List<BranchStandardEntity>> AllBranchStandardWithCountByBranch(long branchid);
        Task<List<DataPoints>> GetStudentAttendanceDetails(long studentid);
        Task<List<ChartBranchEntity>> GetTotalCountList(long studentid);
    }
}
