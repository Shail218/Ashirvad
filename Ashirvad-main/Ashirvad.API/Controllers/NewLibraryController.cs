using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/NewLibrary/v1")]
    [AshirvadAuthorization]
    public class NewLibraryController : ApiController
    {
        private readonly ILibrary1Service _libraryService = null;
        private readonly ICategoryService _CategoryService = null;
        public NewLibraryController(ILibrary1Service libraryService, ICategoryService categoryService)
        {
            _libraryService = libraryService;
            _CategoryService = categoryService;
        }

        [Route("LibraryMaintenance/{LibraryID}/{LibraryDetailID}/{Title}/{link}/{FileName}/{Extension}/{Description}/{BranchID}/{CategoryID}/{CreateId}/{CreateBy}/{TransactionId}/{HasFile}/{Type}")]
        [HttpPost]
        public OperationResult<LibraryEntity1> LibraryMaintenance(long LibraryID, long LibraryDetailID,
             string Title, string link, string FileName, string Extension,
            string Description, long BranchID, long CategoryID, int CreateId, string CreateBy, long TransactionId, bool HasFile, int Type)
        {
            LibraryEntity1 libInfo = new LibraryEntity1();
            OperationResult<LibraryEntity1> result = new OperationResult<LibraryEntity1>();
            var httpRequest = HttpContext.Current.Request;
            libInfo.BranchInfo = new BranchEntity();
            libInfo.CategoryInfo = new CategoryEntity();
            libInfo.BranchInfo.BranchID = BranchID;
            libInfo.CategoryInfo.CategoryID = CategoryID;
            libInfo.LibraryID = LibraryID;
            libInfo.Title = Title;
            libInfo.link = link;
            libInfo.FileName = FileName;
            libInfo.FilePath = "/LibraryImage/" + FileName + "." + Extension;
            libInfo.Description = Description;
            libInfo.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            libInfo.Transaction = new TransactionEntity()
            {
                TransactionId = TransactionId,
                LastUpdateBy = CreateBy,
                LastUpdateId = CreateId,
                CreatedBy = CreateBy,
                CreatedId = CreateId,
            };
            if (HasFile)
            {
                if (httpRequest.Files.Count > 0)
                {
                    try
                    {
                        foreach (string file in httpRequest.Files)
                        {
                            string fileName;
                            string extension;
                            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                            // for live server
                            //string UpdatedPath = currentDir.Replace("mastermindapi", "mastermind");
                            // for local server
                            string UpdatedPath = currentDir.Replace("WebAPI", "wwwroot");
                            var postedFile = httpRequest.Files[file];
                            string randomfilename = Common.Common.RandomString(20);
                            extension = Path.GetExtension(postedFile.FileName);
                            fileName = Path.GetFileName(postedFile.FileName);
                            string _Filepath = "/LibraryImage/" + randomfilename + extension;
                            string _Filepath1 = "LibraryImage/" + randomfilename + extension;
                            var filePath = HttpContext.Current.Server.MapPath("~/LibraryImage/" + randomfilename + extension);
                            string _path = UpdatedPath + _Filepath1;
                            postedFile.SaveAs(_path);
                            libInfo.FileName = fileName;
                            libInfo.FilePath = _Filepath1;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            if (Type == 1)
            {
                libInfo.Type = (int)Enums.GalleryType.Video;
                libInfo.FileName = "";
                libInfo.FilePath = "";
            }
            else
            {
                libInfo.Type = (int)Enums.GalleryType.Image;
                libInfo.link = "";
            }

            var data = this._libraryService.LibraryMaintenance(libInfo).Result;

            result.Completed = false;
            result.Data = null;
            if (data.LibraryID > 0)
            {
                result.Completed = true;
                result.Data = data;
                if (LibraryID > 0)
                {
                    result.Message = "Library Updated Successfully!";
                }
                else
                {
                    result.Message = "Library Created Successfully!";
                }

            }
            return result;
        }

        [Route("LibraryLinkMaintenance")]
        [HttpPost]
        public OperationResult<LibraryEntity1> LibraryLinkMaintenance(LibraryEntity1 libraryEntity1)
        {
            OperationResult<LibraryEntity1> result = new OperationResult<LibraryEntity1>();
            var data = this._libraryService.LibraryMaintenance(libraryEntity1).Result;
            result.Completed = true;
            result.Data = data;
            result.Message = "Success";
            return result;
        }

        [Route("GetAllLibrary")]
        [HttpGet]
        public OperationResult<List<LibraryEntity1>> GetAllLibrary(int Type, int BranchID)
        {
            var data = this._libraryService.GetAllLibrary(Type, BranchID);
            OperationResult<List<LibraryEntity1>> result = new OperationResult<List<LibraryEntity1>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetLibraryByLibraryID")]
        [HttpGet]
        public OperationResult<LibraryEntity1> GetLibraryByLibraryID(long libraryID)
        {
            var data = this._libraryService.GetLibraryByLibraryID(libraryID);
            OperationResult<LibraryEntity1> result = new OperationResult<LibraryEntity1>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }


        [Route("RemoveLibrary")]
        [HttpPost]
        public OperationResult<bool> RemoveLibrary(long libraryID, string lastupdatedby)
        {
            var data = this._libraryService.RemoveLibrary(libraryID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetAllCategory")]
        [HttpGet]
        public OperationResult<List<CategoryEntity>> GetAllCategory(long BranchID)
        {
            var data = this._CategoryService.GetAllCategorys(BranchID);
            OperationResult<List<CategoryEntity>> result = new OperationResult<List<CategoryEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }
    }
}











