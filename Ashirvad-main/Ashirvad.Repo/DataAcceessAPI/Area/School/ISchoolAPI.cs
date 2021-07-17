using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.School
{
    public interface ISchoolAPI
    {
        Task<long> SchoolMaintenance(SchoolEntity schoolInfo);
        Task<List<SchoolEntity>> GetAllSchools(long branchID);
        Task<List<SchoolEntity>> GetAllSchools();
        Task<SchoolEntity> GetSchoolsByID(long schoolInfo);
        bool RemoveSchool(long SchoolID, string lastupdatedby);
    }
}
