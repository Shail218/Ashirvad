using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Subject
{
    public interface ISubjectAPI
    {
        Task<long> SubjectMaintenance(SubjectEntity subjectInfo);
        Task<List<SubjectEntity>> GetAllSubjects(long branchID);
        bool RemoveSubject(long SubjectID, string lastupdatedby);
        Task<List<SubjectEntity>> GetAllSubjects();
        Task<SubjectEntity> GetSubjectByID(long subjectID);
        Task<List<SubjectEntity>> GetAllSubjectsByTestDate(string TestDate);
    }
}
