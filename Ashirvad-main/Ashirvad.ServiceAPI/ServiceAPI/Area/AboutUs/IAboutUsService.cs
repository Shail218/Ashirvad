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
        Task<ResponseModel> AboutUsMaintenance(AboutUsEntity aboutUsInfo);
        Task<OperationResult<List<AboutUsEntity>>> GetAllAboutUsWithoutContent(long branchID = 0);
        Task<AboutUsEntity> GetAboutUsByUniqueID(long uniqueID, long branchID = 0);
        Task<List<AboutUsDetailEntity>> GetAllAboutUs(long branchID = 0);
        ResponseModel RemoveAboutUs(long aboutUsID, string lastupdatedby, bool remomveAboutUsDetail);
        Task<ResponseModel> AboutUsDetailMaintenance(AboutUsDetailEntity aboutUsInfo);
        Task<OperationResult<List<AboutUsDetailEntity>>> GetAllAboutUsDetailWithoutContent(long aboutusID = 0, long branchID = 0);
        Task<OperationResult<AboutUsDetailEntity>> GetAboutUsDetailByUniqueID(long uniqueID);
        Task<List<AboutUsDetailEntity>> GetAllAboutUsDetail(long aboutusID = 0, long branchID = 0);
        ResponseModel RemoveAboutUsDetail(long aboutUsID, string lastupdatedby);
        Task<List<AboutUsDetailEntity>> GetAllAboutUsDetailforExport(long aboutusID = 0, long branchID = 0);
    }
}
