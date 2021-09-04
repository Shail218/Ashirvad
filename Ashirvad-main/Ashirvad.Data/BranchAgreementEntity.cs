using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class BranchAgreementEntity
    {
        public long AgreementID { get; set; }
        public BranchEntity BranchData { get; set; }
        public DateTime AgreementFromDate { get; set; }
        public DateTime AgreementToDate { get; set; }
        public double Amount { get; set; }
        public TransactionEntity TranscationData { get; set; }
        public RowStatusEntity RowStatusData { get; set; }
    }
}
