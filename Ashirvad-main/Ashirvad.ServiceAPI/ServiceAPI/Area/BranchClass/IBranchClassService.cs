using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area
{
    public interface IBranchClassService
    {
        Task<BranchClassEntity> BranchClassMaintenance(BranchClassEntity BranchClassInfo);
        Task<List<BranchClassEntity>> GetAllBranchClass(DataTableAjaxPostModel model,long BrancchID=0,long CourseID=0);
        Task<List<BranchClassEntity>> GetAllBranchClassDDL(long BrancchID=0,long CourseID=0);
        Task<List<BranchClassEntity>> GetMobileAllBranchClass(long BrancchID = 0, long CourseID = 0);
        Task<List<BranchClassEntity>> GetBranchClassByBranchClassID(long BranchClassID, long BranchID);
        Task<BranchClassEntity> GetPackaegBranchClassByID(long BranchClassID);
        ResponseModel RemoveBranchClass(long BranchClassID,long BranchID, string lastupdatedby);
        Task<List<BranchClassEntity>> GetAllSelectedClasses(long BranchID = 0, long CourseID = 0);
    }
}
