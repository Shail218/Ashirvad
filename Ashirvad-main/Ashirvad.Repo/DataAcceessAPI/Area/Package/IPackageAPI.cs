using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Package
{
  public interface IPackageAPI
    {
        Task<long> PackageMaintenance(PackageEntity packageInfo);
        Task<List<PackageEntity>> GetAllPackages(long branchID);
        bool RemovePackage(long PackageID, string lastupdatedby);
        Task<List<PackageEntity>> GetAllPackages();
        Task<PackageEntity> GetPackageByID(long packageID);
    }
}
