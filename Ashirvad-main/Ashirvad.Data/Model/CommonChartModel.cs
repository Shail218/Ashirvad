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
        public List<ChartBranchEntity> branchlist { get; set; } = new List<ChartBranchEntity>();
        public List<BranchStandardEntity> branchstandardlist { get; set; } = new List<BranchStandardEntity>();
        public List<DataPoints> dataPoints { get; set; } = new List<DataPoints>();
        public List<TestDataPoints> testdataPoints { get; set; } = new List<TestDataPoints>();
    }
    public class ChartBranchEntity
    {
        public int y { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public string drilldown { get; set; }
        public long branchid { get; set; }
        public List<BranchStandardEntity> branchstandardlist { get; set; } = new List<BranchStandardEntity>();
    }

    public class BranchStandardEntity
    {
        public string id { get; set; }
        public long branchid { get; set; }
        public int count { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public string standardname { get; set; }
        public string achievemarks { get; set; }
        public double totalmarks { get; set; }
        public ArrayList data = new ArrayList();
    }
    public class DataPoints
    {
        public string label { get; set; }
        public int y { get; set; }
        public int Days { get; set; }
    }
    public class TestDataPoints
    {
        public string label { get; set; }
        public double y { get; set; }
        public string Days { get; set; }
        public double TotalMarks { get; set; }
        public double AchieveMarks { get; set; }
    }
}
