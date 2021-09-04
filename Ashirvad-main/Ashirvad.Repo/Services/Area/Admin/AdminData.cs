using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Admin
{
    public class AdminData : ModelAccess, IAdminData
    {

        public List<DataUsageEntity> GetDataUsage(long branchID)
        {
            //var data = this.context.usp_get_usage();
            //if (data.Count() != 0)
            //{
            //    var result = data.Where(z => 0 == branchID || z.branch_id == 0 || z.branch_id == branchID).Select(x => new DataUsageEntity()
            //    {
            //        BranchID = x.branch_id,
            //        Identifier = x.keyVal,
            //        Usage = x.usage
            //    }).ToList();
            //}

            //var data = (from u in this.context.usp_get_usage()
            //            where 0 == branchID || u.branch_id == branchID || u.branch_id == 0
            //            select new DataUsageEntity()
            //            {
            //                BranchID = u.branch_id,
            //                Identifier = u.keyVal,
            //                Usage = u.usage
            //            }).ToList();

            var result = this.context.usp_get_usage().Where(z => 0 == branchID || z.branch_id == 0 || z.branch_id == branchID).Select(x => new DataUsageEntity()
            {
                BranchID = x.branch_id,
                Identifier = x.keyVal,
                Usage = x.usage
            }).ToList();
            return result;

            //return new List<DataUsageEntity>();
        }
    }
}
