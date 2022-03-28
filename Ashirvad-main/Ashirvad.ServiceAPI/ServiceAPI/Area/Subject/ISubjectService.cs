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
        Task<ResponseModel> SubjectMaintenance(SubjectEntity subjectInfo);
        Task<List<SubjectEntity>> GetAllSubjects(long branchID);
        Task<List<SubjectEntity>> GetAllSubjectsName(long branchid);
        Task<List<SubjectEntity>> GetAllSubjectsID(string subjectName, long branchid);
        ResponseModel RemoveSubject(long SubjectID, string lastupdatedby);
        Task<List<SubjectEntity>> GetAllSubjects();
        Task<SubjectEntity> GetSubjectByIDAsync(long standardID);
        Task<List<SubjectEntity>> GetAllSubjectsByTestDate(string TestDate);
    }
}
