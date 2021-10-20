using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area
{
    public interface ILibrary1API
    {
        Task<long> LibraryMaintenance(LibraryEntity1 branchInfo);
        Task<List<LibraryEntity1>> GetAllLibrary(long Type);
        Task<List<LibraryEntity1>> GetAllLibraryWithoutImage();
        Task<LibraryEntity1> GetLibraryByLibraryID(long LibraryID);
        bool RemoveLibrary(long LibraryID, string lastupdatedby);
       
    }
}
