using Ashirvad.Common;
using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Package
{
   public interface IPackageService
    {
        Task<ResponseModel> PackageMaintenance(PackageEntity packageInfo);
        Task<List<PackageEntity>> GetAllPackages(long branchID);
        ResponseModel RemovePackage(long PackageID, string lastupdatedby);
        Task<List<PackageEntity>> GetAllPackages();
        Task<PackageEntity> GetPackageByIDAsync(long standardID);
        Task<List<PackageEntity>> GetAllCustomPackage(Common.Common.DataTableAjaxPostModel model, long branchID);
    }
}
