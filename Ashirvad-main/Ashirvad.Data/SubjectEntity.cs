using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class SubjectEntity
    {
        public long SubjectID { get; set; }
        public string Subject { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public BranchEntity BranchInfo { get; set; }
        public BranchSubjectEntity BranchSubject { get; set; }
    }
}
