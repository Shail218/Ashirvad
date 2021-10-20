using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.Repo.DataAcceessAPI.Area.Library;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area
{
    public class Library1Service : ILibrary1Service
    {
        private readonly ILibrary1API _LibraryContext;
        public Library1Service(ILibrary1API LibraryContext)
        {
            this._LibraryContext = LibraryContext;
        }

        public async Task<LibraryEntity1> LibraryMaintenance(LibraryEntity1 LibraryInfo)
        {
            LibraryEntity1 Library = new LibraryEntity1();
            try
            {
                long LibraryID = await _LibraryContext.LibraryMaintenance(LibraryInfo);
                if (LibraryID > 0)
                {
                    //Add User
                    //Get Library
                    Library.LibraryID = LibraryID;
                }
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return Library;
        }

        public async Task<List<LibraryEntity1>> GetAllLibraryWithoutImage()
        {
            try
            {
                OperationResult<List<LibraryEntity1>> Library = new OperationResult<List<LibraryEntity1>>();
                Library.Data = await _LibraryContext.GetAllLibraryWithoutImage();
                Library.Completed = true;
                return Library.Data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<LibraryEntity1> GetLibraryByLibraryID(long LibraryID)
        {
            try
            {
                LibraryEntity1 Library = new LibraryEntity1();
                Library = await _LibraryContext.GetLibraryByLibraryID(LibraryID);
                return Library;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<LibraryEntity1>> GetAllLibrary(int Type)
        {
            try
            {
                return await this._LibraryContext.GetAllLibrary(Type);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemoveLibrary(long LibraryID, string lastupdatedby)
        {
            try
            {
                return this._LibraryContext.RemoveLibrary(LibraryID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

        
    }
}
