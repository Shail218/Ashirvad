using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class CompetitionWinnerEntity
    {
        public long competition_winner_id { get; set; }
        public string prizeName { get; set; }
        public CompetitionEntity competitionInfo { get; set; }
        public CompetitionRankEntity competitionRankInfo { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }
        public List<CompetitionWinnerEntity> winnerData { get; set; }
    }
}
