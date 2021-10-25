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
 
        Task<PackageRightEntity> GetPackageRightsByPackageRightsID(long PackageRightsID);     
        bool RemovePackageRights(long PackageRightsID, string lastupdatedby);
        
    }
}
