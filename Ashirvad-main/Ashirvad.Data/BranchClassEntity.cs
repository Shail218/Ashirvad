using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class BranchClassEntity : ResponseModel
    {
        public long Class_dtl_id { get; set; }
        public int Count { get; set; }
        public string JsonData { get; set; }
        public BranchEntity branch { get; set; }
        public ClassEntity Class { get; set; }
        public BranchClassEntity branchClass { get; set; }
        public BranchCourseEntity BranchCourse { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public List<BranchClassEntity> BranchClassData { get; set; } = new List<BranchClassEntity>();
        public List<BranchCourseEntity> BranchClassList { get; set; } = new List<BranchCourseEntity>();
        public List<ClassEntity> ClassData { get; set; } = new List<ClassEntity>();
        public bool isClass { get; set; }
    }
}
