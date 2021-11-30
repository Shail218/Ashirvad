using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Library;
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
    [RoutePrefix("api/library/v1")]
    [AshirvadAuthorization]
    public class LibraryController : ApiController
    {
        private readonly ILibraryService _libraryService = null;
        public LibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [Route("LibraryMaintenance/{LibraryID}/{LibraryDetailID}/{LibraryTitle}/{CategoryID}/{BranchID}/{ThumbnailFilePath}/{DocFileName}/{DocFilePath}/{Type}/{Library_Type}/{StandardID}/{SubjectID}/{Descripation}/{CreateId}/{CreateBy}/{TransactionId}/{FileName}/{Extension}/{HasFile}")]
        [HttpPost]
        public OperationResult<LibraryEntity> LibraryMaintenance(long LibraryID, long LibraryDetailID, string LibraryTitle,long CategoryID, long StandardID, long BranchID,
           int Type,int Library_Type, string Descripation,long SubjectID, long CreateId, string CreateBy, long TransactionId, string FileName, string Extension, bool HasFile)
        {
            OperationResult<LibraryEntity> result = new OperationResult<LibraryEntity>();
            var httpRequest = HttpContext.Current.Request;
            LibraryEntity libraryEntity = new LibraryEntity();
            LibraryEntity data = new LibraryEntity();
            libraryEntity.BranchData = new BranchEntity();
            libraryEntity.CategoryInfo = new CategoryEntity();
            libraryEntity.BranchData.BranchID = BranchID;
            libraryEntity.StandardID = StandardID;
            libraryEntity.LibraryTitle = LibraryTitle;
            libraryEntity.SubjectID = SubjectID;
            libraryEntity.Type = Type;
            libraryEntity.Library_Type = Library_Type;
            libraryEntity.Description = Descripation;
            libraryEntity.ThumbnailFileName = FileName;
            libraryEntity.ThumbnailFilePath = "/LibraryImage/" + FileName + "." + Extension;
            libraryEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            libraryEntity.Transaction = new TransactionEntity()
            {
                TransactionId = TransactionId,
                LastUpdateBy = CreateBy,
                LastUpdateId = CreateId,
                CreatedBy = CreateBy,
                CreatedId = CreateId,
            };
            if (HasFile)
            {
                try
                {
                    if (httpRequest.Files.Count > 0)
                    {
                        foreach (string file in httpRequest.Files)
                        {
                            libraryEntity.LibraryID = LibraryDetailID;
                            string fileName;
                            string extension;
                            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                            // for live server
                            //string UpdatedPath = currentDir.Replace("AshirvadAPI", "ashivadproduct");
                            // for local server
                            string UpdatedPath = currentDir.Replace("Ashirvad.API", "Ashirvad.Web");
                            var postedFile = httpRequest.Files[file];
                            string randomfilename = Common.Common.RandomString(20);
                            extension = Path.GetExtension(postedFile.FileName);
                            fileName = Path.GetFileName(postedFile.FileName);
                            string _Filepath = "/LibraryImage/" + randomfilename + extension;
                            string _Filepath1 = "LibraryImage/" + randomfilename + extension;
                            var filePath = HttpContext.Current.Server.MapPath("~/LibraryImage/" + randomfilename + extension);
                            string _path = UpdatedPath + _Filepath1;
                            postedFile.SaveAs(_path);
                            libraryEntity.ThumbnailFileName = fileName;
                            libraryEntity.ThumbnailFilePath = _Filepath;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Completed = false;
                    result.Data = null;
                    result.Message = ex.ToString();
                }
            }
            data = this._libraryService.LibraryMaintenance(libraryEntity).Result;
            result.Completed = false;
            result.Data = null;
            if (data.LibraryID > 0)
            {
                result.Completed = true;
                result.Data = data;
                if (LibraryID > 0)
                {
                    result.Message = "Library Updated Successfully";
                }
                else
                {
                    result.Message = "Library Created Successfully";
                }
            }
            else
            {
                result.Message = "Library Already Exists!!";
            }
            return result;
        }

        [Route("GetAllLibrary")]
        [HttpGet]
        public OperationResult<List<LibraryEntity>> GetAllLibrary()
        {
            var data = this._libraryService.GetAllLibrary();
            OperationResult<List<LibraryEntity>> result = new OperationResult<List<LibraryEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllLibraryByStdBranch")]
        [HttpGet]
        public OperationResult<List<LibraryEntity>> GetAllLibrary(long branchID, long stdID)
        {
            var data = this._libraryService.GetAllLibrary(branchID, stdID);
            OperationResult<List<LibraryEntity>> result = new OperationResult<List<LibraryEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetLibraryByLibraryID")]
        [HttpGet]
        public OperationResult<LibraryEntity> GetLibraryByLibraryID(long libraryID)
        {
            var data = this._libraryService.GetLibraryByLibraryID(libraryID);
            OperationResult<LibraryEntity> result = new OperationResult<LibraryEntity>();
            result = data.Result;
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
    }
}
