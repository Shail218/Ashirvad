using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Admin
{
    public interface IAdminDataService
    {
        List<DataUsageEntity> GetDataUsage(long branchID = 0);
    }
}
