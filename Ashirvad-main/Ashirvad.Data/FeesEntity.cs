using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ashirvad.Data
{
    public class FeesEntity
    {
        public long FeesID { get; set; }
        public long FeesDetailID { get; set; }
        public byte[] Fees_Content { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public string FileName { get; set; }
        public StandardEntity standardInfo { get; set; }
        public BranchEntity BranchInfo { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }
        public string FilePath { get; set; }
        public string Remark { get; set; }
    }
}
