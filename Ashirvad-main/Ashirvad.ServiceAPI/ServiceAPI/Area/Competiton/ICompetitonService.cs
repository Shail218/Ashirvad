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
        Task<List<CompetitionEntity>> GetAllCompetitonData();

        #endregion

        #region Competition Answer Sheet

        Task<ResponseModel> CompetitionSheetMaintenance(CompetitionAnswerSheetEntity competitionAnswerSheet);
        Task<List<CompetitionAnswerSheetEntity>> GetAllAnswerSheetByCompetitionId(long competitionId);
        Task<List<CompetitionAnswerSheetEntity>> GetAllDistinctAnswerSheetDatabyCompetitionId(long competitionId);
        Task<List<CompetitionAnswerSheetEntity>> GetStudentAnswerSheetbyCompetitionID(long competitionId, long studentID);
        Task<List<CompetitionEntity>> GetAllCustomCompetition(DataTableAjaxPostModel model);
        ResponseModel RemoveCompetitionAnswerSheetdetail(long competitionId, long studid);
        ResponseModel UpdateCompetitionAnswerSheetRemarks(long competitionId, long studid, string remarks);
        #endregion

        #region Competition Rank Entry

        Task<CommonResponseModel<List<CompetitionAnswerSheetEntity>>> GetStudentListforCompetitionRankEntry(long competitionId);
        Task<ResponseModel> CompetitionRankMaintenance(CompetitionRankEntity rankEntity);
        Task<ResponseModel> UpdateRankDetail(long CompetitionId, long CompetitionRankId, string Remarks);
        Task<CommonResponseModel<List<CompetitionRankEntity>>> GetCompetitionRankListbyCompetitionId(long CompetitionId);
        Task<CommonResponseModel<List<CompetitionRankEntity>>> GetCompetitionRankDistinctList();
        Task<List<CompetitionRankEntity>> GetCompetitionRankListByCompetitionIdandBranchID(long competitionId, long branchId);
        #endregion

        #region Competition Winner Entry
        Task<ResponseModel> CompetitionWinnerMaintenance(CompetitionWinnerEntity winnerEntity);
        Task<CommonResponseModel<List<CompetitionWinnerEntity>>> GetCompetitionWinnerListbyCompetitionId();
        Task<CommonResponseModel<CompetitionWinnerEntity>> GetCompetitionWinnerDetailbyId(long competitionWinnerId);
        Task<ResponseModel> DeleteCompetitionWinner(long competitionWinnerId, string lastupdatedby);
        #endregion
    }
}
