using Ashirvad.Data;
using Ashirvad.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Competiton
{
    public interface ICompetitonService
    {
        #region Competition Entry
        Task<ResponseModel> CompetitionMaintenance(CompetitionEntity CompetitonInfo);
        Task<List<CompetitionEntity>> GetAllCompetiton();
        Task<CompetitionEntity> GetCompetitionByID(long CompetitonID);
        Task<ResponseModel> DeleteCompetition(long CompetitionID, string lastupdatedby);

        #endregion

        #region Competition Answer Sheet
        
        Task<ResponseModel> CompetitionSheetMaintenance(CompetitionAnswerSheetEntity competitionAnswerSheet);
        Task<List<CompetitionAnswerSheetEntity>> GetAllAnswerSheetByCompetitionId(long competitionId);
        Task<List<CompetitionAnswerSheetEntity>> GetAllDistinctAnswerSheetDatabyCompetitionId(long competitionId);
        Task<List<CompetitionAnswerSheetEntity>> GetStudentAnswerSheetbyCompetitionID(long competitionId, long studentID);
        Task<List<CompetitionEntity>> GetAllCustomCompetition(DataTableAjaxPostModel model);

        #endregion


    }
}
