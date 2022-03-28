using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.UPI
{
    public interface IUPIService
    {
        Task<ResponseModel> UPIMaintenance(UPIEntity upiInfo);
        Task<OperationResult<List<UPIEntity>>> GetAllUPIs(long branchID);
        Task<OperationResult<UPIEntity>> GetUPIByUPIID(long upiID);
        ResponseModel RemoveUPI(long upiID, string lastupdatedby);
    }
}
