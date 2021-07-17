using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Library
{
    public interface ILibraryService
    {
        Task<LibraryEntity> LibraryMaintenance(LibraryEntity libInfo);

        Task<OperationResult<List<LibraryEntity>>> GetAllLibraryWithoutContent(long branchID = 0, long stdID = 0);

        Task<OperationResult<LibraryEntity>> GetLibraryByLibraryID(long libID);

        Task<List<LibraryEntity>> GetAllLibrary(long branchID = 0, long stdID = 0);

        bool RemoveLibrary(long libID, string lastupdatedby);
    }
}
