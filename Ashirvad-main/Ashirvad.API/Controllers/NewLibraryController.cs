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
        public NewLibraryController(ILibrary1Service libraryService)
        {
            _libraryService = libraryService;
        }

        [Route("LibraryMaintenance")]
        [HttpPost]
        public OperationResult<LibraryEntity1> LibraryMaintenance(long LibraryID, long LibraryDetailID, 
             string Title, string link, string FileName, string FilePath, 
            string Description, long BranchID, long CategoryID,string CreateBy,int CreateId)
        {
            LibraryEntity1 libInfo = new LibraryEntity1();
            OperationResult<LibraryEntity1> result = new OperationResult<LibraryEntity1>();
            var httpRequest = HttpContext.Current.Request;
            libInfo.BranchInfo = new BranchEntity();
            libInfo.CategoryInfo = new CategoryEntity();
            libInfo.BranchInfo.BranchID = BranchID;
            libInfo.CategoryInfo.CategoryID = CategoryID;
            libInfo.LibraryID = LibraryID;
            libInfo.LibraryDetailID = LibraryDetailID;
            libInfo.Title = Title;
            libInfo.link = link;
            libInfo.FileName = FileName;
            libInfo.FilePath = FilePath;         
            libInfo.Description = Description;
            libInfo.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            libInfo.Transaction = new TransactionEntity()
            {
                CreatedBy = CreateBy,
                CreatedId = CreateId,
                CreatedDate = DateTime.Now,
            };
            if (httpRequest.Files.Count > 0)
            {
                try
                {
                    foreach (string file in httpRequest.Files)
                    {
                        string fileName;
                        string extension;
                        var postedFile = httpRequest.Files[file];
                        string randomfilename = Common.Common.RandomString(20);
                        extension = Path.GetExtension(postedFile.FileName);
                        fileName = Path.GetFileName(postedFile.FileName);
                        string _Filepath = "~/LibraryImage/" + randomfilename + extension;
                        var filePath = HttpContext.Current.Server.MapPath("~/LibraryImage/" + randomfilename + extension);
                        postedFile.SaveAs(filePath);
                        libInfo.FileName = fileName;
                        libInfo.FilePath = _Filepath;
                    }

                    if (libInfo.link != "" && libInfo.link != null)
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

                    libInfo.RowStatus = new RowStatusEntity()
                    {
                        RowStatusId = (int)Enums.RowStatus.Active
                    };
                    var data = this._libraryService.LibraryMaintenance(libInfo).Result;
                   
                    result.Completed = false;
                    result.Data = null;
                    if (data.LibraryID > 0)
                    {
                        result.Completed = false;
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



                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return result;



        }

        [Route("GetAllLibrary")]
        [HttpGet]
        public OperationResult<List<LibraryEntity1>> GetAllLibrary(int Type,int BranchID,int UserType)
        {
            if (UserType == (int)Enums.UserType.SuperAdmin)
            {
                BranchID = 0;
            }
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
            return result;
        }


        [Route("RemoveLibrary")]
        [HttpPost]
        public OperationResult<bool> RemoveLibrary(long libraryID, string lastupdatedby)
        {
            var data =this._libraryService.RemoveLibrary(libraryID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }
    }
}











