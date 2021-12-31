using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Library;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Standard;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Subject;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Ashirvad.Common.Common;

namespace Ashirvad.Web.Controllers
{
    public class LibraryController : BaseController
    {
        private readonly ILibraryService _libraryService;
        private readonly ISubjectService _subjectService;
        private readonly IStandardService _standardService;
        public LibraryController(ILibraryService libraryService, ISubjectService subjectService, IStandardService standardService)
        {
            _libraryService = libraryService;
            _subjectService = subjectService;
            _standardService = standardService;
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
                if (result.Data.Subjectlist.Count > 0)
                {
                    result.Data.subject = new SubjectEntity()
                    {
                        Subject = result.Data.Subjectlist[0].Subject
                    };
                }
                result.Data.JsonList = JsonConvert.SerializeObject(result.Data.Standardlist);

                branch.LibraryInfo = result.Data;
            }
            if (SessionContext.Instance.LoginUser.UserType != Enums.UserType.SuperAdmin)
            {
                BranchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
            }
            //if (SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin)
            //{
            //    var branchData = await _libraryService.GetAllLibrary(Type, 0);
            //    branch.LibraryData = branchData;
            //}
            //else
            //{
            //    var branchData = await _libraryService.GetAllLibrarybybranch(Type, SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            //    branch.LibraryData = branchData;
            //}
            branch.LibraryData = new List<LibraryEntity>();
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
            var data = new LibraryEntity();
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
            library.Transaction = GetTransactionData(library.LibraryID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            library.CreatebyBranch = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
            if (library.Type != 1)
            {
                string[] stdname = library.StandardNameArray.Split(',');
                if (SessionContext.Instance.LoginUser.UserType == Ashirvad.Common.Enums.UserType.SuperAdmin)
                {
                    for (int i = 0; i < stdname.Length; i++)
                    {
                        var stdlist = await _standardService.GetAllStandardsID(stdname[i], 0);
                        library.Standardlist.AddRange(stdlist);
                    }
                    library.Subjectlist = await _subjectService.GetAllSubjectsID(library.subject.Subject, 0);
                }
                else
                {
                    for (int i = 0; i < stdname.Length; i++)
                    {
                        var stdlist = await _standardService.GetAllStandardsID(stdname[i], SessionContext.Instance.LoginUser.BranchInfo.BranchID);
                        library.Standardlist.AddRange(stdlist);
                    }
                    library.Subjectlist = await _subjectService.GetAllSubjectsID(library.subject.Subject, SessionContext.Instance.LoginUser.BranchInfo.BranchID);
                }
            }


            data = await _libraryService.LibraryMaintenance(library);
            if (data != null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        [HttpPost]
        public JsonResult RemoveLibrary(long libraryID)
        {
            var result = _libraryService.RemoveLibrary(libraryID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            try
            {
                var branchData = await _libraryService.GetAllCustomLibrary(model,2, SessionContext.Instance.LoginUser.BranchInfo.BranchID);
                long total = 0;
                if (branchData.Count > 0)
                {
                    total = branchData[0].Count;
                }
                return Json(new
                {
                    // this is what datatables wants sending back
                    draw = model.draw,
                    iTotalRecords = total,
                    iTotalDisplayRecords = total,
                    data = branchData
                });
            }
            catch(Exception ex)
            {
                throw;
            }
           

        }


        [HttpPost]
        public async Task<JsonResult> CustomServerSideSearchAction2(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            try
            {
                var branchData = await _libraryService.GetAllCustomLibrary(model, 1, SessionContext.Instance.LoginUser.BranchInfo.BranchID);
                long total = 0;
                if (branchData.Count > 0)
                {
                    total = branchData[0].Count;
                }
                return Json(new
                {
                    // this is what datatables wants sending back
                    draw = model.draw,
                    iTotalRecords = total,
                    iTotalDisplayRecords = total,
                    data = branchData
                });
            }
            catch (Exception ex)
            {
                throw;
            }


        }
    }
}