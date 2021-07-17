using Ashirvad.API.Filter;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        [Route("LibraryMaintenance")]
        [HttpPost]
        public OperationResult<LibraryEntity> LibraryMaintenance(LibraryEntity libInfo)
        {
            OperationResult<LibraryEntity> result = new OperationResult<LibraryEntity>();

            var data = this._libraryService.LibraryMaintenance(libInfo);
            result.Completed = true;
            result.Data = data.Result;
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
