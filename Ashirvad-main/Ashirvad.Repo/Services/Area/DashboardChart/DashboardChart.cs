using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Repo.DataAcceessAPI.Area.DashboardChart;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.DashboardChart
{
    public class DashboardChart : ModelAccess,IDashboardChartAPI
    {
        public async Task<List<ChartBranchEntity>> AllBranchWithCount()
        {
            var data = (from u in this.context.BRANCH_MASTER
                        join s in this.context.STUDENT_MASTER on u.branch_id equals s.branch_id
                        where (u.branch_id != 1 && u.row_sta_cd == 1)
                        select new ChartBranchEntity()
                        {
                            name = u.branch_name,
                            drilldown = u.branch_name,
                            branchid = u.branch_id
                        }).Distinct().ToList();
            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    item.y = (from u in this.context.STUDENT_MASTER
                              where (u.branch_id == item.branchid && u.row_sta_cd == 1)
                              select new ChartBranchEntity()
                              {
                                  name = u.first_name
                              }).Distinct().Count();                    
                    item.branchstandardlist = AllBranchStandardWithCount(item);
                }
            }           
            return data;
        }

        public List<BranchStandardEntity> AllBranchStandardWithCount(ChartBranchEntity chart)
        {         
            List<BranchStandardEntity> branches = new List<BranchStandardEntity>();
            BranchStandardEntity standardEntity = new BranchStandardEntity();        
            var data = (from u in this.context.STUDENT_MASTER
                       .Include("STD_MASTER")
                        where (u.branch_id == chart.branchid && u.row_sta_cd == 1)
                        select new BranchStandardEntity()
                        {
                          name= u.STD_MASTER.standard,
                         branchid = u.branch_id
                        }).Distinct().ToArray();
            foreach(var item in data)
            {
                ArrayList data1 = new ArrayList();
                int count = (from u in this.context.STUDENT_MASTER
                       .Include("STD_MASTER")
                             where (u.branch_id == chart.branchid && u.row_sta_cd == 1 && u.STD_MASTER.standard == item.name)
                             select new BranchStandardEntity()
                             {                                 
                                branchid = u.branch_id
                             }).Count();
                data1.Add("Standard " + item.name);
                data1.Add(count);
                standardEntity.data.Add(data1);
            }
            standardEntity.id = chart.name;           
            branches.Add(standardEntity);
            return branches;
        }       

        public async Task<List<BranchStandardEntity>> AllBranchStandardWithCountByBranch(long branchid)
        {
            List<BranchStandardEntity> branches = new List<BranchStandardEntity>();
            BranchStandardEntity standardEntity = new BranchStandardEntity();
            var data = (from u in this.context.STUDENT_MASTER
                       .Include("STD_MASTER")
                        where (u.branch_id == branchid && u.row_sta_cd == 1)
                        select new BranchStandardEntity()
                        {
                            name = u.STD_MASTER.standard,
                            branchid = u.branch_id,
                            id = u.BRANCH_MASTER.branch_name
                        }).Distinct().ToArray();
            foreach (var item in data)
            {
                ArrayList data1 = new ArrayList();
                int count = (from u in this.context.STUDENT_MASTER
                       .Include("STD_MASTER")
                             where (u.branch_id == branchid && u.row_sta_cd == 1 && u.STD_MASTER.standard == item.name)
                             select new BranchStandardEntity()
                             {
                                 branchid = u.branch_id
                             }).Count();
                data1.Add("Standard " + item.name);
                data1.Add(count);
                standardEntity.data.Add(data1);
                standardEntity.id = item.id;
            }           
            branches.Add(standardEntity);
            return branches;
        }
    }
}
