using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.UserRights
{
    public interface IUserRightsService
    {
      Task<ResponseModel> UserRightsMaintenance(UserWiseRightsEntity UserRightsInfo);
        ResponseModel RemoveUserRights(long UserRightsID, string lastupdatedby);
        Task<List<UserWiseRightsEntity>> GetAllUserRightss();
        Task<List<UserWiseRightsEntity>> GetAllUserRightsUniqData(long PackageRightID);
        Task<UserWiseRightsEntity> GetUserRightsByID(long standardID);
        Task<List<UserWiseRightsEntity>> GetUserRightsByUserID(long PackageRightID);
        Task<List<UserWiseRightsEntity>> GetAllCustomRights(DataTableAjaxPostModel model, long UserId);
        Task<List<UserWiseRightsEntity>> GetAllUserRightsbyBranchId(long branchId);
    }
}
