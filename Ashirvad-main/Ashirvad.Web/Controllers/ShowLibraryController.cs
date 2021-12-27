using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class ShowLibraryController : BaseController
    {
        private readonly ILibraryService _libraryService;

        public ShowLibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }
        // GET: ShowLibrary
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ShowLibraryMaintenance(int type)
        {
            LibraryEntity approval = new LibraryEntity();
            List<LibraryEntity> libraryEntities = new List<LibraryEntity>();
            var branchData = await _libraryService.GetLibraryApprovalByBranch(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            if (branchData.Count > 0)
            {
                foreach (LibraryEntity entity in branchData)
                {
                    if (type == entity.Library_Type)
                        libraryEntities.Add(entity);
                }
            }
            return View("Index", libraryEntities);
        }
    }
}