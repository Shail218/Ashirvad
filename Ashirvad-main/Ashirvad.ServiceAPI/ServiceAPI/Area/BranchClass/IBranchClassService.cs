using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area
{
    public interface IBranchClassService
    {
        Task<BranchClassEntity> BranchClassMaintenance(BranchClassEntity BranchClassInfo);
        Task<List<BranchClassEntity>> GetAllBranchClass(long BrancchID=0,long CourseID=0);

        Task<List<BranchClassEntity>> GetBranchClassByBranchClassID(long BranchClassID, long BranchID);
            Task<BranchClassEntity> GetPackaegBranchClassByID(long BranchClassID);
        bool RemoveBranchClass(long BranchClassID,long BranchID, string lastupdatedby);
        
    }
}
