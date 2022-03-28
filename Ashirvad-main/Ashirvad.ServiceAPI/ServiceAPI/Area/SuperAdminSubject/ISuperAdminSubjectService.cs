using Ashirvad.Common;
using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.SuperAdminSubject
{
    public interface ISuperAdminSubjectService
    {
        Task<ResponseModel> SubjectMaintenance(SuperAdminSubjectEntity subjectEntity);
        Task<OperationResult<SuperAdminSubjectEntity>> GetSubjectBySubjectID(long subjectID);
        Task<OperationResult<List<SuperAdminSubjectEntity>>> GetAllSubject();
        ResponseModel RemoveSubject(long subjectID, string lastupdatedby);
        Task<OperationResult<List<SuperAdminSubjectEntity>>> GetAllCustomSubject(DataTableAjaxPostModel model);
        Task<OperationResult<List<SuperAdminSubjectEntity>>> GetAllSubjectByCourseClass(long courseid, long classid);

        Task<List<BranchSubjectEntity>> GetAllSubjectByCourseClassddl(long courseid, long classid,bool Isupdate=false);
    }
}
