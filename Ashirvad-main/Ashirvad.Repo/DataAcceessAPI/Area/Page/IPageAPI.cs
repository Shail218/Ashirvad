using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Page
{
  public interface IPageAPI
    {
        Task<ResponseModel> PageMaintenance(PageEntity pageInfo);
        Task<List<PageEntity>> GetAllPages(long branchID);
        ResponseModel RemovePage(long PageID, string lastupdatedby);
        Task<List<PageEntity>> GetAllPages();
        Task<PageEntity> GetPageByID(long pageID);
        Task<List<PageEntity>> GetAllCustomPages(DataTableAjaxPostModel model);
    }
}
