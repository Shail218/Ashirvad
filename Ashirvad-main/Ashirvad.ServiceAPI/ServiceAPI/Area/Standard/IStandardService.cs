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
        Task<StandardEntity> StandardMaintenance(StandardEntity standardInfo);
        Task<List<StandardEntity>> GetAllStandards(long branchID);
        bool RemoveStandard(long StandardID, string lastupdatedby);
        Task<List<StandardEntity>> GetAllStandards();
        Task<StandardEntity> GetStandardsByID(long schoolID);
    }
}
