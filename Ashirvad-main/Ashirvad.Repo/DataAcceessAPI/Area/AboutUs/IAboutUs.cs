using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.AboutUs
{
    public interface IAboutUs
    {
        Task<long> AboutUsMaintenance(AboutUsEntity aboutUsInfo);
        Task<List<AboutUsEntity>> GetAllAboutUs(long branchID);
        Task<List<AboutUsEntity>> GetAllAboutUsWithoutContent(long branchID);
        Task<AboutUsEntity> GetAboutUsByUniqueID(long uniqueID);
        bool RemoveAboutUs(long uniqueID, string lastupdatedby, bool removeAboutUsDetail);
        Task<long> AboutUsDetailMaintenance(AboutUsDetailEntity aboutUsDetailInfo);
        Task<List<AboutUsDetailEntity>> GetAllAboutUsDetails(long aboutusID, long branchID);
        Task<List<AboutUsDetailEntity>> GetAllAboutUsDetailWithoutContent(long aboutusID, long branchID);
        Task<AboutUsDetailEntity> GetAboutUsDetailByUniqueID(long uniqueID);
        bool RemoveAboutUsDetail(long uniqueID, string lastupdatedby);
    }
}
