using Ashirvad.Data;
using Ashirvad.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.DashboardChart
{
    public interface IDashboardChartService
    {
        Task<OperationResult<List<ChartBranchEntity>>> AllBranchWithCount();
        Task<OperationResult<PackageUsageEntity>> GetPackageUsageDetailbyBranch(long branchId);
    }
}
