using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area
{
    public interface IPackageRightsAPI
    {
        Task<long> RightsMaintenance(PackageRightEntity RightsInfo);
        Task<List<PackageRightEntity>> GetAllRights();
        Task<List<PackageRightEntity>> GetRightsByRightsID(long RightsID);
        Task<PackageRightEntity> GetPackagebyID(long PackageRightsID);
        bool RemoveRights(long RightsID, string lastupdatedby);
 
    }
}
