using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class StandardEntity : ResponseModel
    {
        public long StandardID { get; set; }
        public string Standard { get; set; }        
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public BranchEntity BranchInfo { get; set; }
        public BranchClassEntity Branchclass { get; set; }
    }
}
