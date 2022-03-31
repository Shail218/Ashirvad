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

namespace Ashirvad.ServiceAPI.Services.Area.Competition
{
    public class CompetitionService : ICompetitonService
    {
        private readonly ICompetitonAPI _competitionContext;
        public CompetitionService(ICompetitonAPI competitionContext)
        {
            this._competitionContext = competitionContext;
        }
        public async Task<ResponseModel> CompetitionMaintenance(CompetitionEntity CompetitonInfo)
        {
            ClassEntity cl = new ClassEntity();
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

        public Task<ResponseModel> DeleteCompetition(long CompetitionID)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CompetitionEntity>> GetAllCompetiton()
        {
            try
            {
                return await _competitionContext.GetAllCompetiton();
            }
            catch(Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }

        public Task<CompetitionEntity> GetCompetitionByID(int Competiton)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> SaveCompetition(CompetitionEntity competition, string Username, bool IsUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
