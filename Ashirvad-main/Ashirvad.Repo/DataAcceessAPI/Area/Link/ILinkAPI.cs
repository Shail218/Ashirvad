using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Link
{
    public interface ILinkAPI
    {
        Task<long> LinkMaintenance(LinkEntity linkInfo);
        Task<List<LinkEntity>> GetAllLink(int type, long branchID, long stdID);
        Task<LinkEntity> GetLinkByUniqueID(long uniqueID);
        bool RemoveLink(long uniqueID, string lastupdatedby);
        Task<List<LinkEntity>> GetAllCustomLiveVideo(DataTableAjaxPostModel model, long branchID, int type);
        Task<List<LinkEntity>> GetAllLinkBySTD(int type, long branchID, long courseid, long stdID);
    }
}
