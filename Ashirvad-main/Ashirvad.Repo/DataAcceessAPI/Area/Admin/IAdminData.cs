using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Admin
{
    public interface IAdminData
    {
        List<DataUsageEntity> GetDataUsage(long branchID);
    }
}
