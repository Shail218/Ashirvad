using Ashirvad.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class ClassEntity
    {
        public long ClassID { get; set; }
        public int Count { get; set; }
        public long course_dtl_id { get; set; }
        public string ClassName { get; set; }
        public string OldStandard { get; set; }
        public Enums.UserType UserType { get; set; }
        public TransactionEntity Transaction { get; set; }
        public BranchEntity branchEntity { get; set; }
        public RowStatusEntity RowStatus { get; set; }
    }
}
