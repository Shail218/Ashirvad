using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Staff
{
    public interface IStaffAPI
    {
        Task<long> StaffMaintenance(StaffEntity staffInfo);
        Task<long> UpdateProfile(StaffEntity staffInfo);
        Task<List<StaffEntity>> GetAllStaff(long branchID, string financialyear);
        bool RemoveStaff(long StaffID, string lastupdatedby);
        Task<List<StaffEntity>> GetAllStaff(string financialyear);
        Task<StaffEntity> GetStaffByID(long userID);
        Task<List<StaffEntity>> GetAllCustomStaff(DataTableAjaxPostModel model, long branchID, string financialyear);
    }
}
