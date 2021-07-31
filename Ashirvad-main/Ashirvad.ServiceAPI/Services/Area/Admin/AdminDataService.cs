using Ashirvad.ServiceAPI.ServiceAPI.Area.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Admin;
using Ashirvad.Logger;

namespace Ashirvad.ServiceAPI.Services.Area.Admin
{
    public class AdminDataService : IAdminDataService
    {
        private readonly IAdminData _adminData;
        public AdminDataService(IAdminData adminData)
        {
            this._adminData = adminData;
        }

        public List<DataUsageEntity> GetDataUsage(long branchID = 0)
        {
            try
            {
                return _adminData.GetDataUsage(branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
                return new List<DataUsageEntity>();
            }
        }
    }
}
