using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.SuperAdminSubject
{
    public interface ISuperAdminSubjectAPI
    {
        Task<ResponseModel> SubjectMaintenance(SuperAdminSubjectEntity subjectEntity);
        Task<SuperAdminSubjectEntity> GetSubjectBySubjectID(long subjectID);
        Task<List<SuperAdminSubjectEntity>> GetAllSubject();
        ResponseModel RemoveSubject(long subjectID, string lastupdatedby);
        Task<List<SuperAdminSubjectEntity>> GetAllCustomSubject(DataTableAjaxPostModel model);
        Task<List<SuperAdminSubjectEntity>> GetAllSubjectByCourseClass(long courseid, long classid);
        Task<List<BranchSubjectEntity>> GetAllSubjectByCourseClassddl(long courseid, long classid, bool Isupdate = false);
    }
}
