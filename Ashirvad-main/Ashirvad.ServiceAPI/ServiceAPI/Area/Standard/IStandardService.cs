using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Standard
{
   public interface IStandardService
    {
        Task<ResponseModel> StandardMaintenance(StandardEntity standardInfo);
        Task<List<StandardEntity>> GetAllStandards(long branchID);
        ResponseModel RemoveStandard(long StandardID, string lastupdatedby);
        Task<StandardEntity> GetStandardsByID(long schoolID);
        Task<List<StandardEntity>> GetAllStandardsName(long branchid);
        Task<List<StandardEntity>> GetAllStandardsID(string standardname, long branchid);
    }
}
