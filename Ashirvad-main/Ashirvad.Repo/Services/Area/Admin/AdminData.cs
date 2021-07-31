using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Admin
{
    public class AdminData : ModelAccess
    {

        public List<DataUsageEntity> GetDataUsage(long branchID = 0)
        {
            var data = this.context.usp_get_usage();
            if (data.Count() != 0)
            {
                var result = data.Where(z => 0 == branchID || z.branch_id == 0 || z.branch_id == branchID).Select(x => new DataUsageEntity()
                {
                    BranchID = x.branch_id,
                    Identifier = x.keyVal,
                    Usage = x.usage
                }).ToList();

                return result;
            }

            return new List<DataUsageEntity>();
        }
    }
}
