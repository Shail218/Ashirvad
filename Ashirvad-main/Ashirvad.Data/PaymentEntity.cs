using Ashirvad.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ashirvad.Data
{
    public class PaymentEntity
    {
        public long PaymentID { get; set; }
        public StudentEntity StudentData { get; set; }
        public string StudentRemarks { get; set; }
        public string AdminRemarks { get; set; }
        public Enums.PaymentStatus PaymentStatus { get; set; }
        public RowStatusEntity RowStatusData { get; set; }
        public TransactionEntity TransactionData { get; set; }
        public BranchEntity BranchData { get; set; }
        public PaymentContentEntity PaymentData { get; set; }
    }

    public class PaymentContentEntity
    {
        public long ContentID { get; set; }
        public HttpPostedFileBase FileInfo { get; set; }
        public byte[] PaymentContent { get; set; }
        public string PaymentContentText { get; set; }
        public string PaymentContentFileName { get; set; }
    }

}
