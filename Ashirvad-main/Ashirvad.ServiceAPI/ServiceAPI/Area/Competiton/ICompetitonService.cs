using Ashirvad.Data;
using Ashirvad.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Competiton
{
    public interface ICompetitonService
    {
        Task<ResponseModel> CompetitionMaintenance(CompetitionEntity CompetitonInfo);
        Task<ResponseModel> SaveCompetition(CompetitionEntity competition, string Username, bool IsUpdate);
        //Task<long> CheckCompetitonExist(string CompetitionName, long CompetitionID);
        Task<List<CompetitionEntity>> GetAllCompetiton();
        Task<CompetitionEntity> GetCompetitionByID(int Competiton);
        Task<ResponseModel> DeleteCompetition(long CompetitionID);
    }
}
