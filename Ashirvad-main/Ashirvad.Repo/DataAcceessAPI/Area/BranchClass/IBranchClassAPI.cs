using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area
{
    public interface IBranchClassAPI
    {
        Task<long> ClassMaintenance(BranchClassEntity ClassInfo);
        Task<List<BranchClassEntity>> GetAllClass(long BranchID,long ClassID= 0);
        Task<List<BranchClassEntity>> GetMobileAllClass(long BranchID, long ClassID = 0);
        Task<List<BranchClassEntity>> GetClassByClassID(long ClassID,long BranchID);
        bool RemoveClass(long ClassID, long BranchID, string lastupdatedby);
        Task<BranchClassEntity> GetClassbyID(long ClassID);

    }
}
