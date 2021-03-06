using Ashirvad.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class BatchEntity
    {
        public long BatchID { get; set; }
        public int BatchTime { get; set; }
        public string MonFriBatchTime { get; set; }
        public string SatBatchTime { get; set; }
        public string SunBatchTime { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public BranchEntity BranchInfo { get; set; }
        public StandardEntity StandardInfo { get; set; }
        public Enums.BatchType BatchType { get; set; }
        public string BatchText { get; set; }
        public long Count { get; set; }
        public BranchCourseEntity BranchCourse { get; set; }
        public BranchClassEntity BranchClass { get; set; }
    }
}
