using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area
{
    public interface IPackageRightsService
    {
        Task<ResponseModel> PackageRightsMaintenance(PackageRightEntity PackageRightsInfo);
        Task<List<PackageRightEntity>> GetAllPackageRights();
        Task<List<PackageRightEntity>> GetAllCustomRights(DataTableAjaxPostModel model);
        Task<List<PackageRightEntity>> GetPackageRightsByPackageRightsID(long PackageRightsID);
        Task<PackageRightEntity> GetPackaegrightsByID(long PackageRightsID);
        ResponseModel RemovePackageRights(long PackageRightsID, string lastupdatedby);
        
    }
}
