using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Faculty
{
    public interface IFacultyService
    {
        Task<ResponseModel> FacultyMaintenance(FacultyEntity faculInfo);
        Task<List<FacultyEntity>> GetAllFaculty(long branchID = 0);
        Task<OperationResult<FacultyEntity>> GetFacultyByFacultyID(long facultyID);
        ResponseModel RemoveFaculty(long facultyID, string lastupdatedby);
        Task<OperationResult<List<FacultyEntity>>> GetAllFaculty(long branchID, int typeID);
        Task<List<FacultyEntity>> GetAllCustomFaculty(DataTableAjaxPostModel model, long branchID);

        //website

        Task<List<FacultyEntity>> GetAllFacultyWebsite(long branchID, long courseID, long classID, long SubjectID);
        Task<List<FacultyEntity>> GetFacultyDetail(long facultyID);
    }
}
