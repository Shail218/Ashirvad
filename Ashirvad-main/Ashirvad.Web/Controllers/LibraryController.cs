using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class LibraryController : BaseController
    {
        private readonly ILibraryService _libraryService;
        public LibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        // GET: Library
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> LibraryMaintenance(long libraryID)
        {
            LibraryMaintenanceModel branch = new LibraryMaintenanceModel();
            if (libraryID > 0)
            {
                var result = await _libraryService.GetLibraryByLibraryID(libraryID);
                branch.LibraryInfo = result.Data;
            }

            var branchData = await _libraryService.GetAllLibraryWithoutContent();
            branch.LibraryData = branchData.Data;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveLibrary(LibraryEntity library)
        {
            library.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            library.Transaction = GetTransactionData(library.LibraryID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            var data = await _libraryService.LibraryMaintenance(library);
            if (data != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult RemoveLibrary(long libraryID)
        {
            var result = _libraryService.RemoveLibrary(libraryID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        [HttpPost]
        public async Task<string> GetLibraryThumbnail(long libraryID)
        {
            var data = await _libraryService.GetLibraryByLibraryID(libraryID);
            var result = data.Data;
            return "data:image/jpg;base64, " + result.LibraryData.ThumbImageContentText;
        }

    }
}