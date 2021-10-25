using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Page
{
  public interface IPageService
    {
        Task<PageEntity> PageMaintenance(PageEntity pageInfo);
        Task<List<PageEntity>> GetAllPages(long branchID);
        bool RemovePage(long PageID, string lastupdatedby);
        Task<List<PageEntity>> GetAllPages();
        Task<PageEntity> GetPageByIDAsync(long standardID);
    }
}
