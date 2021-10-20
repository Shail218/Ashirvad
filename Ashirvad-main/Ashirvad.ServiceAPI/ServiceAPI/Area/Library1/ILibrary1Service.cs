using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area
{
    public interface ILibrary1Service
    {
        Task<LibraryEntity1> LibraryMaintenance(LibraryEntity1 LibraryInfo);
        Task<List<LibraryEntity1>> GetAllLibrary(int Type);
    
        Task<List<LibraryEntity1>> GetAllLibraryWithoutImage();
        Task<LibraryEntity1> GetLibraryByLibraryID(long LibraryID);
        bool RemoveLibrary(long LibraryID, string lastupdatedby);
        
    }
}
