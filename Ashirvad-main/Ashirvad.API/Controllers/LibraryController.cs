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
using System.Text;
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

        [Route("LibraryMaintenance/{LibraryID}/{LibraryDetailID}/{LibraryTitle}/{CategoryID}/{StandardID}/{BranchID}/{CreatebyBranch}/{Type}/{Library_Type}/{Description}/{SubjectID}/{CreateId}/{CreateBy}/{TransactionId}/{VideoLink}/{ThumbnailFileName}/{ThumbnailFileExtension}/{DocFileName}/{DocFileExtension}/{HasThumbnailFile}/{HasDocFile}")]
        [HttpPost]
        public OperationResult<LibraryEntity> LibraryMaintenance(long LibraryID, long LibraryDetailID, string LibraryTitle, long CategoryID, string StandardID, long BranchID, long CreatebyBranch, int Type, int Library_Type, string Description, long SubjectID, int CreateId, string CreateBy, long TransactionId, string VideoLink, string ThumbnailFileName, string ThumbnailFileExtension, string DocFileName, string DocFileExtension, bool HasThumbnailFile, bool HasDocFile)
        {
            OperationResult<LibraryEntity> result = new OperationResult<LibraryEntity>();
            var httpRequest = HttpContext.Current.Request;
            LibraryEntity libraryEntity = new LibraryEntity();
            LibraryEntity data = new LibraryEntity();
            libraryEntity.BranchData = new BranchEntity();
            libraryEntity.CategoryInfo = new CategoryEntity()
            {
                CategoryID = CategoryID
            };
            libraryEntity.LibraryID = LibraryID;
            libraryEntity.BranchID = BranchID;
            libraryEntity.LibraryTitle = Decode(LibraryTitle);
            libraryEntity.Type = Type;
            libraryEntity.Library_Type = Library_Type;
            libraryEntity.Description = Description != "-" ? Decode(Description) : "";
            libraryEntity.CreatebyBranch = CreatebyBranch;
            libraryEntity.ThumbnailFileName = ThumbnailFileName.Split(',')[0];
            libraryEntity.ThumbnailFilePath = "/ThumbnailImage/" + ThumbnailFileName.Split(',')[1] + "." + ThumbnailFileExtension;
            libraryEntity.DocFileName = DocFileName.Split(',')[0];
            libraryEntity.DocFilePath = "/LibraryImage/" + DocFileName.Split(',')[1] + "." + DocFileExtension;
            string[] stdname = StandardID.Split(',');
            if (!StandardID.Equals("none"))
            {
                for (int i = 0; i < stdname.Length; i++)
                {
                    libraryEntity.Standardlist.Add(new StandardEntity()
                    {
                        StandardID = long.Parse(stdname[i])
                    });
                }
            }
            var subjectEntities = new List<SubjectEntity>();
            subjectEntities.Add(new SubjectEntity()
            {
                SubjectID = SubjectID
            });
            libraryEntity.Subjectlist = subjectEntities;
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
            if (VideoLink != "none")
            {
                libraryEntity.VideoLink = Decode(VideoLink);
                libraryEntity.ThumbnailFileName = "";
                libraryEntity.ThumbnailFilePath = "";
                libraryEntity.DocFileName = "";
                libraryEntity.DocFilePath = "";
            }
            else
            {
                libraryEntity.VideoLink = "";
            }
            if (HasThumbnailFile && HasDocFile)
            {
                try
                {
                    if (httpRequest.Files.Count == 2)
                    {                       
                        string fileName;
                        string extension;
                        string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                        // for live server
                        string UpdatedPath = currentDir.Replace("AshirvadAPI", "ashivadproduct");
                        // for local server
                        //string UpdatedPath = currentDir.Replace("Ashirvad.API", "Ashirvad.Web");
                        var thumbnailFile = httpRequest.Files[0];
                        string randomfilename = Common.Common.RandomString(20);
                        extension = Path.GetExtension(thumbnailFile.FileName);
                        fileName = Path.GetFileName(thumbnailFile.FileName);
                        string _Filepath = "/ThumbnailImage/" + randomfilename + extension;
                        string _Filepath1 = "ThumbnailImage/" + randomfilename + extension;
                        string _path = UpdatedPath + _Filepath1;
                        thumbnailFile.SaveAs(_path);
                        libraryEntity.ThumbnailFileName = fileName;
                        libraryEntity.ThumbnailFilePath = _Filepath;

                        var docFile = httpRequest.Files[1];
                        string docrandomfilename = Common.Common.RandomString(20);
                        string docExtension = Path.GetExtension(docFile.FileName);
                        string docFileName = Path.GetFileName(docFile.FileName);
                        string _docFilepath = "/LibraryImage/" + docrandomfilename + docExtension;
                        string _docFilepath1 = "LibraryImage/" + docrandomfilename + docExtension;
                        string _docpath = UpdatedPath + _docFilepath1;
                        docFile.SaveAs(_docpath);
                        libraryEntity.DocFileName = docFileName;
                        libraryEntity.DocFilePath = _docFilepath;
                    }
                }
                catch (Exception ex)
                {
                    result.Completed = false;
                    result.Data = null;
                    result.Message = ex.ToString();
                }
            }
            else
            {
                if (HasThumbnailFile)
                {
                    try
                    {
                        
                        string fileName;
                        string extension;
                        string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                        // for live server
                        string UpdatedPath = currentDir.Replace("AshirvadAPI", "ashivadproduct");
                        // for local server
                        //string UpdatedPath = currentDir.Replace("Ashirvad.API", "Ashirvad.Web");
                        var thumbnailFile = httpRequest.Files[0];
                        string randomfilename = Common.Common.RandomString(20);
                        extension = Path.GetExtension(thumbnailFile.FileName);
                        fileName = Path.GetFileName(thumbnailFile.FileName);
                        string _Filepath = "/ThumbnailImage/" + randomfilename + extension;
                        string _Filepath1 = "ThumbnailImage/" + randomfilename + extension;
                        string _path = UpdatedPath + _Filepath1;
                        thumbnailFile.SaveAs(_path);
                        libraryEntity.ThumbnailFileName = fileName;
                        libraryEntity.ThumbnailFilePath = _Filepath;
                    }
                    catch (Exception ex)
                    {
                        result.Completed = false;
                        result.Data = null;
                        result.Message = ex.ToString();
                    }
                }
                if (HasDocFile)
                {
                    try
                    {                       
                        string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                        string UpdatedPath = currentDir.Replace("AshirvadAPI", "ashivadproduct");
                        var docFile = httpRequest.Files[0];
                        string docrandomfilename = Common.Common.RandomString(20);
                        string docExtension = Path.GetExtension(docFile.FileName);
                        string docFileName = Path.GetFileName(docFile.FileName);
                        string _docFilepath = "/LibraryImage/" + docrandomfilename + docExtension;
                        string _docFilepath1 = "LibraryImage/" + docrandomfilename + docExtension;
                        string _docpath = UpdatedPath + _docFilepath1;
                        docFile.SaveAs(_docpath);
                        libraryEntity.DocFileName = docFileName;
                        libraryEntity.DocFilePath = _docFilepath;
                    }
                    catch (Exception ex)
                    {
                        result.Completed = false;
                        result.Data = null;
                        result.Message = ex.ToString();
                    }
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

        [Route("GetAllMobileLibrary")]
        [HttpGet]
        public OperationResult<List<LibraryEntity>> GetAllMobileLibrary(int Type, long branchID)
        {
            var data = this._libraryService.GetAllMobileLibrary(Type, branchID);
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

        [Route("GetAllLibraryApproval")]
        [HttpGet]
        public OperationResult<List<LibraryEntity>> GetAllLibraryApproval(long branchID)
        {
            var data = this._libraryService.GetAllLibraryApproval(branchID);
            OperationResult<List<LibraryEntity>> result = new OperationResult<List<LibraryEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("LibraryApprovalMaintenance")]
        [HttpPost]
        public OperationResult<ApprovalEntity> LibraryApprovalMaintenance(ApprovalEntity approvalEntity)
        {
            var data = this._libraryService.LibraryApprovalMaintenance(approvalEntity);
            OperationResult<ApprovalEntity> result = new OperationResult<ApprovalEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetLibraryApprovalByBranch")]
        [HttpGet]
        public OperationResult<List<LibraryEntity>> GetLibraryApprovalByBranch(long branchID)
        {
            var data = this._libraryService.GetLibraryApprovalByBranch(branchID);
            OperationResult<List<LibraryEntity>> result = new OperationResult<List<LibraryEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetLibraryApprovalByBranchStd")]
        [HttpGet]
        public OperationResult<List<LibraryEntity>> GetLibraryApprovalByBranchStd(long standardID, long branchID, string standard)
        {
            var data = this._libraryService.GetLibraryApprovalByBranchStd(standardID, branchID, standard);
            OperationResult<List<LibraryEntity>> result = new OperationResult<List<LibraryEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        public static string Decode(string Path)
        {
            byte[] mybyte = Convert.FromBase64String(Path);
            string returntext = Encoding.UTF8.GetString(mybyte);
            return returntext;
        }
    }
}
