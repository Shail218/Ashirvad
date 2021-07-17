using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Link;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Link;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Link
{
    public class LinkService : ILinkService
    {
        private readonly ILinkAPI _linkContext;
        public LinkService(ILinkAPI linkContext)
        {
            this._linkContext = linkContext;
        }

        public async Task<LinkEntity> LinkMaintenance(LinkEntity linkInfo)
        {
            LinkEntity link = new LinkEntity();
            try
            {
                long linkID = await _linkContext.LinkMaintenance(linkInfo);
                linkInfo.UniqueID = linkID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return link;
        }

        public async Task<OperationResult<List<LinkEntity>>> GetAllLink(int type, long branchID = 0, long stdID = 0)
        {
            try
            {
                OperationResult<List<LinkEntity>> link = new OperationResult<List<LinkEntity>>();
                link.Data = await _linkContext.GetAllLink(type, branchID, stdID);
                link.Completed = true;
                return link;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<LinkEntity>> GetLinkByUniqueID(long uniqueID)
        {
            try
            {
                OperationResult<LinkEntity> link = new OperationResult<LinkEntity>();
                link.Data = await _linkContext.GetLinkByUniqueID(uniqueID);
                link.Completed = true;
                return link;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        
        public bool RemoveLink(long linkID, string lastupdatedby)
        {
            try
            {
                return this._linkContext.RemoveLink(linkID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

    }
}
