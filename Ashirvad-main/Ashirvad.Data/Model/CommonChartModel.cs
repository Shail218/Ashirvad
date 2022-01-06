using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class CommonChartModel
    {
        List<ChartBranchEntity> branchlist { get; set; } = new List<ChartBranchEntity>();
        List<BranchStandardEntity> branchstandardlist { get; set; } = new List<BranchStandardEntity>();
    }
    public class ChartBranchEntity
    {
        public int y { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public string drilldown { get; set; }
        public long branchid { get; set; }
    }

    public class BranchStandardEntity
    {
        public string id { get; set; }
        public long branchid { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public string standardname { get; set; }
        public ArrayList data = new ArrayList();
    }
}
