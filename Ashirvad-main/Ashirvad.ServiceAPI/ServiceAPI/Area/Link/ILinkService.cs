using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Link
{
    public interface ILinkService
    {
        Task<LinkEntity> LinkMaintenance(LinkEntity linkInfo);
        Task<OperationResult<List<LinkEntity>>> GetAllLink(int type, long branchID, long stdID = 0);
        Task<OperationResult<LinkEntity>> GetLinkByUniqueID(long uniqueID);
        bool RemoveLink(long linkID, string lastupdatedby);
        Task<List<LinkEntity>> GetAllCustomVideoLink(DataTableAjaxPostModel model, long branchID, int type);
    }
}
