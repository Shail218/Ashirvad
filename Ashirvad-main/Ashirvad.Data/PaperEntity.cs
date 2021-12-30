using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ashirvad.Data
{
    public class PaperEntity
    {
        public long PaperID { get; set; }
        public BranchEntity Branch { get; set; }
        public StandardEntity Standard { get; set; }
        public SubjectEntity Subject { get; set; }
        public int BatchTypeID { get; set; }
        public string BatchTypeText { get; set; }
        public string Remarks { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }
        public PaperData PaperData { get; set; }
        public long Count { get; set; }
    }

    public class PaperData
    {
        public long UniqueID { get; set; }
        public long PaperID { get; set; }
        public string PaperPath { get; set; }
        public byte[] PaperContent { get; set; }
        public string PaperContentText { get; set; }
        public string FilePath { get; set; }
        public HttpPostedFileBase PaperFile { get; set; }
    }
}
