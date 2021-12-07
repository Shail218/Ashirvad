﻿using Ashirvad.Data;
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

        Task<OperationResult<List<LibraryEntity>>> GetAllLibraryWithoutContent(long branchID, long stdID);

        Task<OperationResult<LibraryEntity>> GetLibraryByLibraryID(long libID);

        Task<List<LibraryEntity>> GetAllLibrary(long branchID = 0, long stdID = 0);

        bool RemoveLibrary(long libID, string lastupdatedby);

        Task<List<LibraryEntity>> GetAllLibrary(int Type, long BranchID);

        Task<List<LibraryEntity>> GetAllLibrarybybranch(int Type, long BranchID);

        Task<List<LibraryEntity>> GetAllMobileLibrary(int Type, long BranchID);

        Task<ApprovalEntity> LibraryApprovalMaintenance(ApprovalEntity approvalEntity);

        Task<List<LibraryEntity>> GetAllLibraryApproval(long BranchId);
    }
}
