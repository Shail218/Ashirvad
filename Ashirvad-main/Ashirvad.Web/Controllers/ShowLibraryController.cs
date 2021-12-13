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

        public async Task<ActionResult> ShowLibraryMaintenance()
        {
            LibraryEntity approval = new LibraryEntity();
            var branchData = await _libraryService.GetLibraryApprovalByBranch(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            //approval.LibraryData = branchData;
            return View("Index", branchData);
        }
    }
}