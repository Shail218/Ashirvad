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
            //if (branch.LibraryData.ThumbImageFile != null)
            //{
            //    branch.LibraryData = new LibraryDataEntity();
            //    branch.LibraryData.ThumbImageContent = Common.Common.ReadFully(branch.LibraryData.ThumbImageFile.InputStream);
            //    branch.LibraryData.ThumbImageFileName = branch.LibraryData.ThumbImageFile.FileName;
            //    branch.LibraryData.ThumbImageExt = Path.GetExtension(branch.LibraryData.ThumbImageFile.FileName);
            //    branch.ThumbImageName = branch.LibraryData.ThumbImageFile.FileName;
            //}

            //if (branch.LibraryData.DocFile != null)
            //{
            //    branch.LibraryData = new LibraryDataEntity();
            //    branch.LibraryData.DocContent = Common.Common.ReadFully(branch.LibraryData.DocFile.InputStream);
            //    branch.LibraryData.DocContentFileName = branch.LibraryData.DocFile.FileName;
            //    branch.LibraryData.DocContentExt = Path.GetExtension(branch.LibraryData.DocFile.FileName);
            //    branch.ThumbDocName = branch.LibraryData.DocFile.FileName;
            //}
            //library.BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
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