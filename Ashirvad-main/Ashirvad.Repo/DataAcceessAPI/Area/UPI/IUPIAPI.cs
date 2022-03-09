﻿using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.UPI
{
    public interface IUPIAPI
    {
        Task<long> UPIMaintenance(UPIEntity upiInfo);
        Task<List<UPIEntity>> GetAllUPIs(long branchID, string financialyear);
        Task<UPIEntity> GetUPIByID(long upiID, string financialyear);
        bool RemoveUPI(long upiID, string lastupdatedby);
    }
}
