﻿using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Branch
{
    public interface IBranchAPI
    {
        Task<long> BranchMaintenance(BranchEntity branchInfo);
        Task<List<BranchEntity>> GetAllBranch();
        Task<List<BranchEntity>> GetAllBranchWithoutImage();
        Task<BranchEntity> GetBranchByBranchID(long branchID);
        bool RemoveBranch(long BranchID, string lastupdatedby);
    }
}
