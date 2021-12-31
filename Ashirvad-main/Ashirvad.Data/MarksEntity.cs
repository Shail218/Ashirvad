using Ashirvad.Common;
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
        public StudentEntity student { get; set; }  
        public BranchEntity BranchInfo { get; set; }
        public DateTime MarksDate { get; set; }
        public StandardEntity StandardInfo { get; set; }
        public SubjectEntity SubjectInfo { get; set; }
        public string TotalMarks { get; set; }        
        public HttpPostedFileBase ImageFile { get; set; }
        public string AchieveMarks { get; set; }
        public string Remarks { get; set; }
        public string MarksContentFileName { get; set; }
        public string MarksFilepath { get; set; }
        public TransactionEntity Transaction { get; set; }
        public TestEntity testEntityInfo { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public string JsonData { get; set; }
        public int BatchTime { get; set; }
        public Enums.BatchType BatchType { get; set; }
        public long Count { get; set; }
    }
}
