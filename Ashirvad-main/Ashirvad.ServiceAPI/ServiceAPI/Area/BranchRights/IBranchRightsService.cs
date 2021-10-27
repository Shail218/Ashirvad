﻿using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area
{
   public interface IBranchRightsService
    {
        Task<BranchWiseRightEntity> BranchRightsMaintenance(BranchWiseRightEntity BranchRightsInfo);     
        bool RemoveBranchRights(long BranchRightsID, string lastupdatedby);
        Task<List<BranchWiseRightEntity>> GetAllBranchRightss();
        Task<BranchWiseRightEntity> GetBranchRightsByID(long standardID);
    }
}