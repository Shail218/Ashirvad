using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class CompetitonMaintenanceModel
    {
        public CompetitionEntity CompetitonInfo { get; set; }
        public List<CompetitionEntity> CompetitionData { get; set; }
        public List<CompetitionAnswerSheetEntity> competitionAnswersData { get; set; }
        public List<CompetitionRankEntity> competitionRankData { get; set; }
    }
}
