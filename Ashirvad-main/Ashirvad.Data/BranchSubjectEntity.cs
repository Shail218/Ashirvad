using Ashirvad.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class BranchSubjectEntity : ResponseModel
    {
        public long Class_dtl_id { get; set; }
        public long Subject_dtl_id { get; set; }
        public string JsonData { get; set; }
        public BranchEntity branch { get; set; }
        public ClassEntity Class { get; set; }
        public BranchSubjectEntity branchSubject { get; set; }
        public BranchCourseEntity BranchCourse { get; set; }
        public BranchClassEntity BranchClass { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public SuperAdminSubjectEntity Subject { get; set; }
        public Enums.UserType UserType { get; set; }
        public List<BranchSubjectEntity> BranchSubjectData { get; set; } = new List<BranchSubjectEntity>();
        public List<SuperAdminSubjectEntity> SubjectData { get; set; } = new List<SuperAdminSubjectEntity>();
        public List<BranchCourseEntity> BranchClassList { get; set; } = new List<BranchCourseEntity>();
        public List<ClassEntity> ClassData { get; set; } = new List<ClassEntity>();
        public bool isClass { get; set; }
        public bool isSubject { get; set; }
        public long Count { get; set; }
    }
}
