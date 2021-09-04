using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class UPIEntity
    {
        public long UPIId { get; set; }
        public string UPICode { get; set; }
        public BranchEntity BranchData { get; set; }
        public RowStatusEntity RowStatusData { get; set; }
        public TransactionEntity TransactionData { get; set; }
    }
}
