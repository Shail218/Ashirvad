using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class CompetitonMaintenanceModel
    {
        public CompetitionEntity CompetitonInfo { get; set; } = new CompetitionEntity();
        public List<CompetitionEntity> CompetitionData { get; set; } = new List<CompetitionEntity>();
        public List<CompetitionAnswerSheetEntity> competitionAnswersData { get; set; } = new List<CompetitionAnswerSheetEntity>();
        public List<CompetitionRankEntity> competitionRankData { get; set; } = new List<CompetitionRankEntity>();
        public CompetitionWinnerEntity competitionWinnerEntity { get; set; } = new CompetitionWinnerEntity();
        public List<CompetitionWinnerEntity> competitionWinnerData { get; set; } = new List<CompetitionWinnerEntity>();
    }
}
