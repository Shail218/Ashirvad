using Ashirvad.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class ApprovalEntity
    {
        public long Approval_id { get; set; }
        public LibraryEntity library { get; set; }
        public long Branch_id { get; set; }
        public TransactionEntity TransactionInfo { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public Enums.ApprovalStatus Library_Status { get; set; }
        public string Library_Status_text { get; set; }
    }
}
