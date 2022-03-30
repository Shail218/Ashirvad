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
        Task<ResponseModel> StaffMaintenance(StaffEntity staffInfo);
        Task<ResponseModel> UpdateProfile(StaffEntity staffInfo);
        Task<List<StaffEntity>> GetAllStaff(long branchID);
        ResponseModel RemoveStaff(long StaffID, string lastupdatedby);
        Task<List<StaffEntity>> GetAllStaff();
        Task<StaffEntity> GetStaffByID(long subjectID);
        Task<List<StaffEntity>> GetAllCustomStaff(DataTableAjaxPostModel model, long branchID);
    }
}
