using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Faculty
{
    public interface IFacultyService
    {
        Task<FacultyEntity> FacultyMaintenance(FacultyEntity faculInfo);
        Task<List<FacultyEntity>> GetAllFaculty(long branchID = 0);
        Task<OperationResult<FacultyEntity>> GetFacultyByFacultyID(long facultyID);
        bool RemoveFaculty(long facultyID, string lastupdatedby);
        Task<OperationResult<List<FacultyEntity>>> GetAllFaculty(long branchID, int typeID);

        //website

        Task<List<FacultyEntity>> GetAllFacultyWebsite(long branchID, long courseID, long classID, long SubjectID);
        Task<List<FacultyEntity>> GetFacultyDetail(long facultyID);
    }
}
