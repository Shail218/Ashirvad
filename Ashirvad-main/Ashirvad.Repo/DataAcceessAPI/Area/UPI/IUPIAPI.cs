using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.UPI
{
    public interface IUPIAPI
    {
        Task<ResponseModel> UPIMaintenance(UPIEntity upiInfo);
        Task<List<UPIEntity>> GetAllUPIs(long branchID);
        Task<UPIEntity> GetUPIByID(long upiID);
        ResponseModel RemoveUPI(long upiID, string lastupdatedby);
    }
}
