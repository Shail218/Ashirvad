using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ashirvad.Data
{
    public class StudentAnswerSheetEntity
    {
        public long AnsSheetID { get; set; }
        public TestEntity TestInfo { get; set; }
        public BranchEntity BranchInfo { get; set; }
        public StudentEntity StudentInfo { get; set; }
        public HttpPostedFileBase FileInfo{ get; set; }
        public byte[] AnswerSheetContent { get; set; }
        public string AnswerSheetContentText { get; set; }
        public string AnswerSheetName { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public string FilePath { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }
        public DateTime SubmitDate { get; set; }
    }
}
