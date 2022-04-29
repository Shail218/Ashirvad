using Ashirvad.Data;
using Ashirvad.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Competition
{
    public interface ICompetitonAPI
    {
        #region Competition Entry
        
        Task<ResponseModel> CompetitionMaintenance(CompetitionEntity CompetitonInfo);
        Task<long> CheckCompetitonExist(string CompetitionName, long CompetitionID);
        Task<List<CompetitionEntity>> GetAllCompetiton();
        Task<CompetitionEntity> GetCompetitionByID(long CompetitonID);
        Task<ResponseModel> DeleteCompetition(long CompetitionID, string lastupdatedby);
        Task<List<CompetitionEntity>> GetAllCustomCompetition(DataTableAjaxPostModel model);
        #endregion

        #region Competition Answer Sheet

        Task<ResponseModel> CompetitionSheetMaintenance(CompetitionAnswerSheetEntity competitionAnswerSheet);
        Task<List<CompetitionAnswerSheetEntity>> GetAllAnswerSheetByCompetitionId(long competitionId);
        Task<List<CompetitionAnswerSheetEntity>> GetAllDistinctAnswerSheetDatabyCompetitionId(long competitionId);
        Task<List<CompetitionAnswerSheetEntity>> GetStudentAnswerSheetbyCompetitionID(long competitionId, long studentID);

        #endregion
    }
}
