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
        Task<ResponseModel> AboutUsMaintenance(AboutUsEntity aboutUsInfo);
        Task<List<AboutUsDetailEntity>> GetAllAboutUs(long AboutID);
        Task<List<AboutUsEntity>> GetAllAboutUsWithoutContent(long branchID);
        Task<AboutUsEntity> GetAboutUsByUniqueID(long uniqueID, long BranchID);
        ResponseModel RemoveAboutUs(long uniqueID, string lastupdatedby, bool removeAboutUsDetail);
        Task<ResponseModel> AboutUsDetailMaintenance(AboutUsDetailEntity aboutUsDetailInfo);
        Task<List<AboutUsDetailEntity>> GetAllAboutUsDetails(long aboutusID, long branchID);
        Task<List<AboutUsDetailEntity>> GetAllAboutUsDetailWithoutContent(long aboutusID, long branchID);
        Task<AboutUsDetailEntity> GetAboutUsDetailByUniqueID(long uniqueID);
        ResponseModel RemoveAboutUsDetail(long uniqueID, string lastupdatedby);
        Task<List<AboutUsDetailEntity>> GetAllAboutUsDetailsforExport(long aboutusID, long branchID);
    }
}
