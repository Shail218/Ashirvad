using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Link
{
    public interface ILinkAPI
    {
        Task<long> LinkMaintenance(LinkEntity linkInfo);
        Task<List<LinkEntity>> GetAllLink(int type, long branchID, long stdID);
        Task<LinkEntity> GetLinkByUniqueID(long uniqueID);
        bool RemoveLink(long uniqueID, string lastupdatedby);
    }
}
