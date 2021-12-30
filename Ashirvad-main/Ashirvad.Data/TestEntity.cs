using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ashirvad.Data
{
    public class TestEntity
    {
        public long TestID { get; set; }
        public string TestName { get; set; }
        public BranchEntity Branch { get; set; }
        public StandardEntity Standard { get; set; }
        public SubjectEntity Subject { get; set; }
        public int BatchTimeID { get; set; }
        public string BatchTimeText { get; set; }
        public double Marks { get; set; }
        public DateTime TestDate { get; set; }
        public string TestStartTime { get; set; }
        public string TestEndTime { get; set; }
        public string Remarks { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }
        public TestPaperEntity test { get; set; }
        public HttpPostedFileBase FileInfo { get; set; }
        public TestDetailEntity testdtl { get; set; }
        public bool marksentered { get; set; }
        public long Count { get; set; }
    }

    public class TestPaperEntity
    {
        public TestEntity testinfo { get; set; }
        public long TestPaperID { get; set; }
        public long TestID { get; set; }
        public string TestName { get; set; }
        public DateTime TestDate { get; set; }
        public int PaperTypeID { get; set; }
        public string PaperType { get; set; }
        public string DocContentText { get; set; }
        public byte[] DocContent { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string DocLink { get; set; }
        public string Remarks { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }
        public HttpPostedFileBase FileInfo  { get; set; }
    }

    public class TestDetailEntity
    {

        public long TestDetailID { get; set; }
        public TestEntity TestEntity { get; set; }
        public BranchEntity BranchInfo { get; set; }
        public StudentEntity StudentInfo { get; set; }
        public HttpPostedFileBase FileInfo { get; set; }
        public byte[] AnswerSheetContent { get; set; }
        public string AnswerSheetContentText { get; set; }
        public string AnswerSheetName { get; set; }
        public string FilePath { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public string Remarks { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }
        public DateTime SubmitDate { get; set; }
    }
}
