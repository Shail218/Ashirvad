using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Library
{
    public interface ILibraryAPI
    {
        Task<long> LibraryMaintenance(LibraryEntity libraryInfo);

        Task<List<LibraryEntity>> GetAllLibrary(long branchID, long stdID);

        Task<List<LibraryEntity>> GetAllLibraryWithoutContent(long branchID, long stdID);

        Task<LibraryEntity> GetLibraryByLibraryID(long library);

        bool RemoveLibrary(long libraryID, string lastupdatedby);

        Task<List<LibraryEntity>> GetAllLibrary(int Type, long BranchID);

        Task<List<LibraryEntity>> GetAllLibrarybybranch(int Type, long BranchID);

        Task<List<LibraryEntity>> GetAllMobileLibrary(int Type, long BranchID);

        Task<long> LibraryApprovalMaintenance(ApprovalEntity approvalEntity);

        Task<List<LibraryEntity>> GetAllLibraryApproval(long BranchId);

        Task<List<LibraryEntity>> GetLibraryApprovalByBranch(long BranchId);

        Task<List<LibraryEntity>> GetLibraryApprovalByBranchStd(long standardID, long BranchId, string standard);

    }
}
