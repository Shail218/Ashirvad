using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Staff
{
    public interface IStaffAPI
    {
        Task<long> StaffMaintenance(StaffEntity staffInfo);
        Task<long> UpdateProfile(StaffEntity staffInfo);
        Task<List<StaffEntity>> GetAllStaff(long branchID);
        bool RemoveStaff(long StaffID, string lastupdatedby);
        Task<List<StaffEntity>> GetAllStaff();
        Task<StaffEntity> GetStaffByID(long userID);
    }
}
