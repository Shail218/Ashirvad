using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class NewLibraryController : BaseController
    {
        // GET: LibraryStructure
        private readonly ILibrary1Service _LibraryService;
        public NewLibraryController(ILibrary1Service LibraryService)
        {
            _LibraryService = LibraryService;
        }
        // GET: Library
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> LibraryMaintenance(long LibraryID)
        {
            LibraryNewMaintenanceModel Library = new LibraryNewMaintenanceModel();
            if (LibraryID > 0)
            {
                var result = await _LibraryService.GetLibraryByLibraryID(LibraryID);
                Library.LibraryInfo = result;
            }

            var LibraryData = await _LibraryService.GetAllLibrary();
            Library.LibraryData = LibraryData;

            return View("Index", Library);
        }

      

        [HttpPost]
        public async Task<JsonResult> SaveLibrary(LibraryEntity1 Library)
        {
            if (Library.ImageFile != null)
            {
                
                Library.FileContent = Common.Common.ReadFully(Library.ImageFile.InputStream);
                string _FileName = Path.GetFileName(Library.ImageFile.FileName);
                string extension = System.IO.Path.GetExtension(Library.ImageFile.FileName);
                string randomfilename = Common.Common.RandomString(20);
                string _Filepath = "/LibraryImage/"+randomfilename + extension;
                string _path = Path.Combine(Server.MapPath("~/LibraryImage"), randomfilename + extension);
                Library.ImageFile.SaveAs(_path);
                Library.FileName= _FileName;
                Library.FilePath = _Filepath;
            }

            Library.Transaction = GetTransactionData(Library.LibraryID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            Library.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _LibraryService.LibraryMaintenance(Library);
            if (data != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult RemoveLibrary(long LibraryID)
        {
            var result = _LibraryService.RemoveLibrary(LibraryID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> LibraryData()
        {
            var LibraryData = await _LibraryService.GetAllLibraryWithoutImage();           

            return Json(LibraryData);
        }
    }
}