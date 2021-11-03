using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Faculty
{
    public interface IFacultyAPI
    {
        Task<long> FacultyMaintenance(FacultyEntity facultyinfo);
        Task<List<FacultyEntity>> GetAllFaculty(long branchID);
        Task<FacultyEntity> GetFacultyByFacultyID(long FacultyID);
        bool RemoveFaculty(long FacultyID, string lastupdatedby);
        Task<List<FacultyEntity>> GetAllFaculty(long branchID, int typeID);
    }
}
