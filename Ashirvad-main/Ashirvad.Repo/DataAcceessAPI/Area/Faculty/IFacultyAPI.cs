using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Faculty
{
    public interface IFacultyAPI
    {
        Task<ResponseModel> FacultyMaintenance(FacultyEntity facultyinfo);
        Task<List<FacultyEntity>> GetAllFaculty(long branchID);
        Task<FacultyEntity> GetFacultyByFacultyID(long FacultyID);
        ResponseModel RemoveFaculty(long FacultyID, string lastupdatedby);
        Task<List<FacultyEntity>> GetAllFaculty(long branchID, int typeID);
        Task<List<FacultyEntity>> GetAllFacultyWebsite(long branchID, long courseID, long classID, long subjectID);
        Task<List<FacultyEntity>> GetFacultyDetail(long facultyID);
        Task<List<FacultyEntity>> GetAllCustomFaculty(DataTableAjaxPostModel model, long branchID);
    }
}
