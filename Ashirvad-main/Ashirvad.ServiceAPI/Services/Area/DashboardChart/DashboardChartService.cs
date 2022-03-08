using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.DashboardChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.DashboardChart
{
    public class DashboardChartService : IDashboardChartService
    {
        private readonly IDashboardChartAPI _chartcontext;

        public DashboardChartService(IDashboardChartAPI chartcontext)
        {
            this._chartcontext = chartcontext;
        }

        public async Task<OperationResult<List<ChartBranchEntity>>> AllBranchWithCount(string financialyear)
        {
            try
            {
                OperationResult<List<ChartBranchEntity>> branch = new OperationResult<List<ChartBranchEntity>>();
                branch.Data = await _chartcontext.AllBranchWithCount(financialyear);
                branch.Completed = true;
                return branch;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
    }
}
