using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Page;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area.Page
{
   public class PageService : IPageService
    {
        private readonly IPageAPI _pageContext;
        public PageService(IPageAPI pageContext)
        {
            this._pageContext = pageContext;
        }

        public async Task<ResponseModel> PageMaintenance(PageEntity pageInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            PageEntity standard = new PageEntity();
            try
            {
                //long pageID = await _pageContext.PageMaintenance(pageInfo);
                responseModel = await _pageContext.PageMaintenance(pageInfo);

                //standard.PageID = pageID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            //return standard;
            return responseModel;
        }

        public async Task<List<PageEntity>> GetAllPages(long branchID)
        {
            try
            {
                return await this._pageContext.GetAllPages(branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<PageEntity>> GetAllPages()
        {
            try
            {
                return await this._pageContext.GetAllPages();
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemovePage(long PageID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                return this._pageContext.RemovePage(PageID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return responseModel;
            //return false;
        }

        public async Task<PageEntity> GetPageByIDAsync(long pageID)
        {
            try
            {
                return await this._pageContext.GetPageByID(pageID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        public async Task<List<PageEntity>> GetAllCustomPages(DataTableAjaxPostModel model)
        {
            try
            {
                return await this._pageContext.GetAllCustomPages(model);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        
    }
}
