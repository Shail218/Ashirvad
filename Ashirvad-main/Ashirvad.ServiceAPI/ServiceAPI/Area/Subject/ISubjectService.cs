using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Subject
{
    public interface ISubjectService
    {
        Task<SubjectEntity> SubjectMaintenance(SubjectEntity subjectInfo);
        Task<List<SubjectEntity>> GetAllSubjects(long branchID);
        bool RemoveSubject(long SubjectID, string lastupdatedby);
        Task<List<SubjectEntity>> GetAllSubjects();
        Task<SubjectEntity> GetSubjectByIDAsync(long standardID);
    }
}
