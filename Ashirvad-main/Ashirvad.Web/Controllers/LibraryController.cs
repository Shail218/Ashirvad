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

        public async Task<ActionResult> LibraryMaintenance(long libraryID, int Type)
        {
            long BranchID = 0;
            LibraryMaintenanceModel branch = new LibraryMaintenanceModel();
            if (libraryID > 0)
            {
                var result = await _libraryService.GetLibraryByLibraryID(libraryID);
                branch.LibraryInfo = result.Data;
            }
            if (SessionContext.Instance.LoginUser.UserType != Enums.UserType.SuperAdmin)
            {
                BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
            }
            var branchData = await _libraryService.GetAllLibrary(Type,0);
            branch.LibraryData = branchData;
            if (Type == (int)Enums.GalleryType.Image)
            {
                return View("Index", branch);

            }
            else
            {
                return View("VideoIndex", branch);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveLibrary(LibraryEntity library)
        {
            LibraryEntity data = new LibraryEntity();
            long id = library.LibraryID;
            if (library.ThumbImageFile != null)
            {
                string _FileName = Path.GetFileName(library.ThumbImageFile.FileName);
                string extension = System.IO.Path.GetExtension(library.ThumbImageFile.FileName);
                string randomfilename = Common.Common.RandomString(20);
                string _Filepath = "/ThumbnailImage/" + randomfilename + extension;
                string _path = Path.Combine(Server.MapPath("~/ThumbnailImage"), randomfilename + extension);
                library.ThumbImageFile.SaveAs(_path);
                library.ThumbnailFileName = _FileName;
                library.ThumbnailFilePath = _Filepath;
            }
            if (library.DocFile != null)
            {
                string _FileName = Path.GetFileName(library.DocFile.FileName);
                string extension = System.IO.Path.GetExtension(library.DocFile.FileName);
                string randomfilename = Common.Common.RandomString(20);
                string _Filepath = "/LibraryImage/" + randomfilename + extension;
                string _path = Path.Combine(Server.MapPath("~/LibraryImage"), randomfilename + extension);
                library.DocFile.SaveAs(_path);
                library.DocFileName = _FileName;
                library.DocFilePath = _Filepath;
            }
            if (library.VideoLink != "" && library.VideoLink != null)
            {
                library.Library_Type = (int)Enums.GalleryType.Video;
                library.ThumbnailFileName = "";
                library.ThumbnailFilePath = "";
                library.DocFileName = "";
                library.DocFilePath = "";
            }
            else
            {
                library.Library_Type = (int)Enums.GalleryType.Image;
                library.VideoLink = "";
            }
            library.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            if (library.BranchID == null)
            {
                library.BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
            }
            if (library.StandardArray != null)
            {
                string[] std = library.StandardArray.Split(',');
                for (int i = 0; i < std.Length; i++)
                {
                    library.LibraryID = id;
                    library.StandardID = Convert.ToInt64(std[i]);
                    library.Transaction = GetTransactionData(library.LibraryID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
                    data = await _libraryService.LibraryMaintenance(library);
                }                
            }
            else
            {
                library.Transaction = GetTransactionData(library.LibraryID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
                data = await _libraryService.LibraryMaintenance(library);
            }           
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
    }
}