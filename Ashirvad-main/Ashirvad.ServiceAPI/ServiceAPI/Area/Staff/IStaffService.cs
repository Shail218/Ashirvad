using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Staff
{
    public interface IStaffService
    {
        Task<StaffEntity> StaffMaintenance(StaffEntity staffInfo);
        Task<StaffEntity> UpdateProfile(StaffEntity staffInfo);
        Task<List<StaffEntity>> GetAllStaff(long branchID, string financialyear);
        bool RemoveStaff(long StaffID, string lastupdatedby);
        Task<List<StaffEntity>> GetAllStaff(string financialyear);
        Task<StaffEntity> GetStaffByID(long subjectID);
        Task<List<StaffEntity>> GetAllCustomStaff(DataTableAjaxPostModel model, long branchID, string financialyear);
    }
}
