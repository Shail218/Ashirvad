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
        #endregion


    }
}
