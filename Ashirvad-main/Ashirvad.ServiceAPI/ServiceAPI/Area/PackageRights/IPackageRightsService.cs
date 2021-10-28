using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area
{
    public interface IPackageRightsService
    {
        Task<PackageRightEntity> PackageRightsMaintenance(PackageRightEntity PackageRightsInfo);
        Task<List<PackageRightEntity>> GetAllPackageRights();

        Task<List<PackageRightEntity>> GetPackageRightsByPackageRightsID(long PackageRightsID);
            Task<PackageRightEntity> GetPackaegrightsByID(long PackageRightsID);
        bool RemovePackageRights(long PackageRightsID, string lastupdatedby);
        
    }
}
