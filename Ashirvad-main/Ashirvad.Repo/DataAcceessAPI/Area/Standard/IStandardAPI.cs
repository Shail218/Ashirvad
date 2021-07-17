using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Standard
{
    public interface IStandardAPI
    {
        Task<long> StandardMaintenance(StandardEntity standardInfo);
        Task<List<StandardEntity>> GetAllStandards(long branchID);
        bool RemoveStandard(long StandardID, string lastupdatedby);
        Task<List<StandardEntity>> GetAllStandards();
        Task<StandardEntity> GetStandardsByID(long standardID);
    }
}
