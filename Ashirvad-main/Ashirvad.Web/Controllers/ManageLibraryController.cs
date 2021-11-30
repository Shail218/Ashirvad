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
    public class ManageLibraryController : BaseController
    {
        private readonly ILibraryService _libraryService;

        public ManageLibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        // GET: ManageLibrary
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ManageLibraryMaintenance(int Type, long branchID)
        {
            LibraryMaintenanceModel library = new LibraryMaintenanceModel();
            var branchData = await _libraryService.GetAllLibrary(Type, 0);
            library.LibraryData = branchData;
            return View("Index", library.LibraryData);
        }
    }
}