using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Competition;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Competiton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area.Competition
{
    public class CompetitionService : ICompetitonService
    {
        private readonly ICompetitonAPI _competitionContext;
        public CompetitionService(ICompetitonAPI competitionContext)
        {
            this._competitionContext = competitionContext;
        }

        #region Competition Entry
        public async Task<ResponseModel> CompetitionMaintenance(CompetitionEntity CompetitonInfo)
        {

            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _competitionContext.CompetitionMaintenance(CompetitonInfo);
                //cl.ClassID = classID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<ResponseModel> DeleteCompetition(long CompetitionID, string lastupdatedby)
        {
            try
            {
                return await _competitionContext.DeleteCompetition(CompetitionID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }

        public async Task<List<CompetitionEntity>> GetAllCompetiton()
        {
            try
            {
                return await _competitionContext.GetAllCompetiton();
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }

        public async Task<CompetitionEntity> GetCompetitionByID(long CompetitonID)
        {
            try
            {
                return await _competitionContext.GetCompetitionByID(CompetitonID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }
                public async Task<List<CompetitionEntity>> GetAllCustomCompetition(DataTableAjaxPostModel model)
        {
            try
            {
                return await _competitionContext.GetAllCustomCompetition(model);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }
        public async Task<List<CompetitionEntity>> GetAllCompetitonData()
        {
            try
            {
                return await _competitionContext.GetAllCompetitonData();
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }
        #endregion

        #region Competition Answer Sheet

        public async Task<ResponseModel> CompetitionSheetMaintenance(CompetitionAnswerSheetEntity competitionAnswerSheet)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _competitionContext.CompetitionSheetMaintenance(competitionAnswerSheet);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<List<CompetitionAnswerSheetEntity>> GetAllAnswerSheetByCompetitionId(long competitionId)
        {
            try
            {
                return await _competitionContext.GetAllAnswerSheetByCompetitionId(competitionId);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }

        public async Task<List<CompetitionAnswerSheetEntity>> GetAllDistinctAnswerSheetDatabyCompetitionId(long competitionId)
        {
            try
            {
                return await _competitionContext.GetAllDistinctAnswerSheetDatabyCompetitionId(competitionId);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }

        public async Task<List<CompetitionAnswerSheetEntity>> GetStudentAnswerSheetbyCompetitionID(long competitionId, long studentID)
        {
            try
            {
                return await _competitionContext.GetStudentAnswerSheetbyCompetitionID(competitionId,studentID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }
        public ResponseModel RemoveCompetitionAnswerSheetdetail(long competitionId, long studid)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = _competitionContext.RemoveCompetitionAnswerSheetdetail(competitionId, studid);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }        
        public ResponseModel UpdateCompetitionAnswerSheetRemarks(long competitionId, long studid, string remarks)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = _competitionContext.UpdateCompetitionAnswerSheetRemarks(competitionId, studid,remarks);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }
        #endregion

        #region Competition Rank Entry

        public async Task<CommonResponseModel<List<CompetitionAnswerSheetEntity>>> GetStudentListforCompetitionRankEntry(long competitionId)
        {
            CommonResponseModel<List<CompetitionAnswerSheetEntity>> responseModel = new CommonResponseModel<List<CompetitionAnswerSheetEntity>>();
            try
            {
                responseModel = await _competitionContext.GetStudentListforCompetitionRankEntry(competitionId);
               
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }
        public async Task<ResponseModel> CompetitionRankMaintenance(CompetitionRankEntity rankEntity)
        {
            try
            {
                return await _competitionContext.CompetitionRankMaintenance(rankEntity);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }
        public async Task<ResponseModel> UpdateRankDetail(long CompetitionId, long CompetitionRankId, string Remarks)
        {
            try
            {
                return await _competitionContext.UpdateRankDetail(CompetitionId, CompetitionRankId, Remarks);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }
        public async Task<CommonResponseModel<List<CompetitionRankEntity>>> GetCompetitionRankListbyCompetitionId(long CompetitionId)
        {
            try
            {
                return await _competitionContext.GetCompetitionRankListbyCompetitionId(CompetitionId);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }
        public async Task<CommonResponseModel<List<CompetitionRankEntity>>> GetCompetitionRankDistinctList()
        {
            try
            {
                return await _competitionContext.GetCompetitionRankDistinctList();
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }

        public async Task<List<CompetitionRankEntity>> GetCompetitionRankListByCompetitionIdandBranchID(long competitionId, long branchId)
        {
            try
            {
                return await _competitionContext.GetCompetitionRankListByCompetitionIdandBranchID(competitionId, branchId);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }


        #endregion

        #region Competition Winner Entry
        public async Task<ResponseModel> CompetitionWinnerMaintenance(CompetitionWinnerEntity winnerEntity)
        {
            try
            {
                return await _competitionContext.CompetitionWinnerMaintenance(winnerEntity);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }
        public async Task<CommonResponseModel<List<CompetitionWinnerEntity>>> GetCompetitionWinnerListbyCompetitionId()
        {
            try
            {
                return await _competitionContext.GetCompetitionWinnerListbyCompetitionId();
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }
        public async Task<CommonResponseModel<CompetitionWinnerEntity>> GetCompetitionWinnerDetailbyId(long competitionWinnerId)
        {
            try
            {
                return await _competitionContext.GetCompetitionWinnerDetailbyId(competitionWinnerId);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }
        public async Task<ResponseModel> DeleteCompetitionWinner(long competitionWinnerId, string lastupdatedby)
        {
            try
            {
                return await _competitionContext.DeleteCompetitionWinner(competitionWinnerId, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }
        #endregion
    }
}
