﻿using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.UPI
{
    public interface IUPIService
    {
        Task<UPIEntity> UPIMaintenance(UPIEntity upiInfo);

        Task<OperationResult<List<UPIEntity>>> GetAllUPIs(long branchID, string financialyear);

        Task<OperationResult<UPIEntity>> GetUPIByUPIID(long upiID, string financialyear);
        bool RemoveUPI(long upiID, string lastupdatedby);
    }
}
