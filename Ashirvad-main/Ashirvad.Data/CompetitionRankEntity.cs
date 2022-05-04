using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class CompetitionRankEntity
    {
        public long CompetitionRankId { get; set; }
        public CompetitionEntity competitionInfo { get; set; }
        public BranchEntity branchInfo { get; set; }
        public StudentEntity studentInfo { get; set; }
        public string competitionRank { get; set; }
        public string competitionMarks { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public DateTime RankDate { get; set; }
        public TransactionEntity Transaction { get; set; }
    }
}
