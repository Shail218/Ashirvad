using Ashirvad.Data;
using Ashirvad.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.DashboardChart
{
    public interface IDashboardChartService
    {
        Task<OperationResult<List<ChartBranchEntity>>> AllBranchWithCount(string financialyear);
    }
}