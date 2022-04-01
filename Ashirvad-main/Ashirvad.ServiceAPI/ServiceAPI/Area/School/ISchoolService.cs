using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.School
{
   public interface ISchoolService
    {
        Task<ResponseModel> SchoolMaintenance(SchoolEntity schoolInfo);
        Task<List<SchoolEntity>> GetAllSchools(long branchID);
        Task<List<SchoolEntity>> GetAllExportSchools(long branchID);
        Task<List<SchoolEntity>> GetAllSchools();
        Task<SchoolEntity> GetSchoolsByID(long schoolInfo);
        ResponseModel RemoveSchool(long SchoolID, string lastupdatedby);
        Task<List<SchoolEntity>> GetAllCustomSchools(DataTableAjaxPostModel model, long branchID);
    }
}
