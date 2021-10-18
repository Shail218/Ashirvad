using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ashirvad.Data
{
    public class MarksEntity
    {
        public long MarksID { get; set; }
        public BranchEntity BranchInfo { get; set; }
        public DateTime MarksDate { get; set; }
        public StandardEntity StandardInfo { get; set; }
        public SubjectEntity SubjectInfo { get; set; }
        public BatchEntity batchEntityInfo  { get; set; }       
        
        public HttpPostedFileBase FileInfo { get; set; }
        public byte[] MarksContent { get; set; }
        public string MarksContentText { get; set; }
        public string MarksContentFileName { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
    }
    public class MarksDetailEntity
    {
    
        public long MarksDetailID { get; set; }       
        public MarksEntity MarksEntity { get; set; }
       
        public HttpPostedFileBase FileInfo { get; set; }
        public byte[] MarksSheetContent { get; set; }
        public string MarksSheetContentText { get; set; }
        public string MarksSheetName { get; set; }      
        public string Remarks { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }
        
    }
}
