using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area
{
    public interface IPackageRightsAPI
    {
        Task<long> RightsMaintenance(PackageRightEntity RightsInfo);
        Task<List<PackageRightEntity>> GetAllRights();
        Task<List<PackageRightEntity>> GetRightsByRightsID(long RightsID);
        bool RemoveRights(long RightsID, string lastupdatedby);
        Task<PackageRightEntity> GetPackagebyID(long RightsID);
        Task<List<PackageRightEntity>> GetAllCustomRights(DataTableAjaxPostModel model);
    }
}
