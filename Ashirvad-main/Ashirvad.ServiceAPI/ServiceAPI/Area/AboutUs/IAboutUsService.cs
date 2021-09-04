using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.AboutUs
{
    public interface IAboutUsService
    {
        Task<AboutUsEntity> AboutUsMaintenance(AboutUsEntity aboutUsInfo);
        Task<OperationResult<List<AboutUsEntity>>> GetAllAboutUsWithoutContent(long branchID = 0);
        Task<OperationResult<AboutUsEntity>> GetAboutUsByUniqueID(long uniqueID);
        Task<List<AboutUsEntity>> GetAllAboutUs(long branchID = 0);
        bool RemoveAboutUs(long aboutUsID, string lastupdatedby, bool remomveAboutUsDetail = false);
        Task<AboutUsDetailEntity> AboutUsDetailMaintenance(AboutUsDetailEntity aboutUsInfo);
        Task<OperationResult<List<AboutUsDetailEntity>>> GetAllAboutUsDetailWithoutContent(long aboutusID = 0, long branchID = 0);
        Task<OperationResult<AboutUsDetailEntity>> GetAboutUsDetailByUniqueID(long uniqueID);
        Task<List<AboutUsDetailEntity>> GetAllAboutUsDetail(long aboutusID = 0, long branchID = 0);
        bool RemoveAboutUsDetail(long aboutUsID, string lastupdatedby);
    }
}
