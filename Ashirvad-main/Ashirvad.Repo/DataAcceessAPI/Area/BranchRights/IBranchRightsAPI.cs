﻿using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area
{
    public interface IBranchRightsAPI
    {
        Task<long> RightsMaintenance(BranchWiseRightEntity RightsInfo);
        Task<List<BranchWiseRightEntity>> GetAllRights();
        Task<BranchWiseRightEntity> GetRightsByRightsID(long RightsID);
        bool RemoveRights(long RightsID, string lastupdatedby);
 
    }
}