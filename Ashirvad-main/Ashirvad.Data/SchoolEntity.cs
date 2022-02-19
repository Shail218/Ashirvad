using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class SchoolEntity
    {
        public long SchoolID { get; set; }
        public string SchoolName { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public BranchEntity BranchInfo { get; set; }
        public List<string> tablefield { get; set; }
        public long Count { get; set; }
    }
}
