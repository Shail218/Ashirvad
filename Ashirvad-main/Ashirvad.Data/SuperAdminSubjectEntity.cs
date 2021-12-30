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
    }
}
