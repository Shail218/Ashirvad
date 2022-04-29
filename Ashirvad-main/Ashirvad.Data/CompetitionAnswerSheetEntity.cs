using Ashirvad.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class CompetitionAnswerSheetEntity
    {
        public long CompetitionDtlId { get; set; }
        public CompetitionEntity competitionInfo { get; set; }
        public BranchEntity branchInfo { get; set; }
        public StudentEntity studentInfo { get; set; }
        public byte[] CompetitionSheetContent { get; set; }
        public string CompetitionSheetName { get; set; }
        public string CompetitionFilepath { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public string Remarks { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public DateTime SubmitDate { get; set; }
        public TransactionEntity Transaction { get; set; }
    }
}
