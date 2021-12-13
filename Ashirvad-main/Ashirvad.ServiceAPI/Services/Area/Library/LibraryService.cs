using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Library;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.Library
{
    public class LibraryService : ILibraryService
    {
        private readonly ILibraryAPI _libraryContext;
        public LibraryService(ILibraryAPI libContext)
        {
            this._libraryContext = libContext;
        }

        public async Task<LibraryEntity> LibraryMaintenance(LibraryEntity libInfo)
        {
            LibraryEntity Library = new LibraryEntity();
            try
            {
                long LibraryID = await _libraryContext.LibraryMaintenance(libInfo);
                if (LibraryID > 0)
                {
                    Library.LibraryID = LibraryID;
                }
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return Library;
        }

        public async Task<OperationResult<List<LibraryEntity>>> GetAllLibraryWithoutContent(long branchID = 0, long stdID = 0)
        {
            try
            {
                OperationResult<List<LibraryEntity>> lib = new OperationResult<List<LibraryEntity>>();
                lib.Data = await _libraryContext.GetAllLibraryWithoutContent(branchID, stdID);
                lib.Completed = true;
                return lib;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<LibraryEntity>> GetLibraryByLibraryID(long libID)
        {
            try
            {
                OperationResult<LibraryEntity> lib = new OperationResult<LibraryEntity>();
                lib.Data = await _libraryContext.GetLibraryByLibraryID(libID);
                lib.Completed = true;
                return lib;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<LibraryEntity>> GetAllLibrary(long branchID = 0, long stdID = 0)
        {
            try
            {
                return await this._libraryContext.GetAllLibrary(branchID, stdID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemoveLibrary(long libID, string lastupdatedby)
        {
            try
            {
                return this._libraryContext.RemoveLibrary(libID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

        public async Task<List<LibraryEntity>> GetAllLibrarybybranch(int Type, long BranchID)
        {
            try
            {
                return await _libraryContext.GetAllLibrarybybranch(Type, BranchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        public async Task<List<LibraryEntity>> GetAllLibrary(int Type, long BranchID)
        {
            try
            {
                return await _libraryContext.GetAllLibrary(Type, BranchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        public async Task<List<LibraryEntity>> GetAllMobileLibrary(int Type, long BranchID)
        {
            try
            {
                return await _libraryContext.GetAllMobileLibrary(Type, BranchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<ApprovalEntity> LibraryApprovalMaintenance(ApprovalEntity approvalEntity)
        {
            ApprovalEntity approval = new ApprovalEntity();
            try
            {
                long ApprovalID = await _libraryContext.LibraryApprovalMaintenance(approvalEntity);
                if (ApprovalID > 0)
                {
                    approval.Approval_id = ApprovalID;
                }
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return approval;
        }

        public async Task<List<LibraryEntity>> GetAllLibraryApproval(long BranchId)
        {
            try
            {
                return await _libraryContext.GetAllLibraryApproval(BranchId);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<LibraryEntity>> GetLibraryApprovalByBranch(long BranchId)
        {
            try
            {
                return await _libraryContext.GetLibraryApprovalByBranch(BranchId);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<LibraryEntity>> GetLibraryApprovalByBranchStd(long standardID, long BranchId, string standard)
        {
            try
            {
                return await _libraryContext.GetLibraryApprovalByBranchStd(standardID, BranchId, standard);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

    }
}
