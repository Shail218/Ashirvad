using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ashirvad.Data.Model
{
   public class CompetitionEntity
    {
        public long CompetitionID { get; set; }
        public string CompetitionName { get; set; }
        public double TotalMarks { get; set; }
        public System.DateTime CompetitionDt { get; set; }
        public string CompetitionStartTime { get; set; }
        public string CompetitionEndTime { get; set; }
        public string Remarks { get; set; }
        public string FileName { get; set; }
        public string extra1 { get; set; }
        public string extra2 { get; set; }
        public string FilePath { get; set; }
        public string DocLink { get; set; }
        public bool DocType { get; set; } //true = document,false = link
        public string extra3 { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }
        public HttpPostedFileBase FileInfo { get; set; }
    }
}
