using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.SuperAdminSubject
{
    public interface ISuperAdminSubjectAPI
    {
        Task<long> SubjectMaintenance(SuperAdminSubjectEntity subjectEntity);
        Task<SuperAdminSubjectEntity> GetSubjectBySubjectID(long subjectID);
        Task<List<SuperAdminSubjectEntity>> GetAllSubject();
        bool RemoveSubject(long subjectID, string lastupdatedby);
    }
}
