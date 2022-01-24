using Ashirvad.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class SuperAdminSubjectEntity
    {
        public long SubjectID { get; set; }
        public int Count { get; set; }
        public string SubjectName { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public Enums.UserType UserType { get; set; }
        public BranchEntity branchEntity { get; set; }
        public CourseEntity courseEntity { get; set; }
        public ClassEntity classEntity { get; set; }
        public string oldsubject { get; set; }
    }
}
