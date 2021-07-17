using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Branch
{
    public interface IBranchService
    {
        Task<BranchEntity> BranchMaintenance(BranchEntity branchInfo);
        Task<List<BranchEntity>> GetAllBranch();

        Task<OperationResult<List<BranchEntity>>> GetAllBranchWithoutImage();
        Task<OperationResult<BranchEntity>> GetBranchByBranchID(long branchID);
        bool RemoveBranch(long BranchID, string lastupdatedby);
    }
}
