using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Staff
{
    public interface IStaffService
    {
        Task<StaffEntity> StaffMaintenance(StaffEntity staffInfo);
        Task<List<StaffEntity>> GetAllStaff(long branchID);
        bool RemoveStaff(long StaffID, string lastupdatedby);
        Task<List<StaffEntity>> GetAllStaff();
        Task<StaffEntity> GetStaffByID(long subjectID);
    }
}
