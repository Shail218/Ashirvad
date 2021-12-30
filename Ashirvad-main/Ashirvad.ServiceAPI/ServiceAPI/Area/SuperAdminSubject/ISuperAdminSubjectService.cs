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
        Task<SuperAdminSubjectEntity> SubjectMaintenance(SuperAdminSubjectEntity subjectEntity);
        Task<OperationResult<SuperAdminSubjectEntity>> GetSubjectBySubjectID(long subjectID);
        Task<OperationResult<List<SuperAdminSubjectEntity>>> GetAllSubject();
        bool RemoveSubject(long subjectID, string lastupdatedby);
        Task<OperationResult<List<SuperAdminSubjectEntity>>> GetAllCustomSubject(DataTableAjaxPostModel model);
    }
}
