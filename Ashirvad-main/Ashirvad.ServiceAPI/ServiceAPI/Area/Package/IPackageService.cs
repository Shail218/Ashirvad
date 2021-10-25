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
        Task<PackageEntity> PackageMaintenance(PackageEntity packageInfo);
        Task<List<PackageEntity>> GetAllPackages(long branchID);
        bool RemovePackage(long PackageID, string lastupdatedby);
        Task<List<PackageEntity>> GetAllPackages();
        Task<PackageEntity> GetPackageByIDAsync(long standardID);
    }
}
