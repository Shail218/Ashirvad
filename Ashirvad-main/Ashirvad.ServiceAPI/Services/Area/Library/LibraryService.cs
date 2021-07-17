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
            LibraryEntity library = new LibraryEntity();
            try
            {
                if (libInfo.LibraryData != null)
                {
                    if (libInfo.LibraryData.DocFile != null)
                    {
                        libInfo.LibraryData.DocContent = Common.Common.ReadFully(libInfo.LibraryData.DocFile.InputStream);
                        libInfo.LibraryData.DocContentExt = Path.GetExtension(libInfo.LibraryData.DocFile.FileName);
                        libInfo.LibraryData.DocContentFileName = Path.GetFileName(libInfo.LibraryData.DocFile.FileName);
                        libInfo.ThumbDocName = libInfo.LibraryData.DocContentFileName;
                    }

                    if (libInfo.LibraryData.ThumbImageFile != null)
                    {
                        libInfo.LibraryData.ThumbImageContent = Common.Common.ReadFully(libInfo.LibraryData.ThumbImageFile.InputStream);
                        libInfo.LibraryData.ThumbImageExt = Path.GetExtension(libInfo.LibraryData.ThumbImageFile.FileName);
                        libInfo.LibraryData.ThumbImageFileName = Path.GetFileName(libInfo.LibraryData.ThumbImageFile.FileName);
                        libInfo.ThumbImageName = libInfo.LibraryData.ThumbImageFileName;
                    }
                }

                long libraryID = await _libraryContext.LibraryMaintenance(libInfo);
                if (libraryID > 0)
                {
                    library.LibraryID = libraryID;
                    if (!string.IsNullOrEmpty(Common.Common.GetStringConfigKey("DocDirectory")))
                    {
                        Common.Common.SaveFile(libInfo.LibraryData.DocContent, libInfo.LibraryData.DocContentFileName, "Library\\");
                        Common.Common.SaveFile(libInfo.LibraryData.ThumbImageContent, libInfo.LibraryData.ThumbImageFileName, "Library\\");
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return library;
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

    }
}
