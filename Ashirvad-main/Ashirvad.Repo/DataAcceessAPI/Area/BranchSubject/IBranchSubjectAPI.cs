using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area
{
    public interface IBranchSubjectAPI
    {
        Task<long> SubjectMaintenance(BranchSubjectEntity SubjectInfo);
        Task<List<BranchSubjectEntity>> GetAllSubject(long BranchID);
        Task<List<BranchSubjectEntity>> GetSubjectBySubjectID(long SubjectID,long BranchID, long ClassID);
        bool RemoveSubject(long CourseID,long ClassID, long BranchID, string lastupdatedby);
        Task<BranchSubjectEntity> GetSubjectbyID(long SubjectID);

        Task<List<BranchSubjectEntity>> GetMobileAllSubject(long BranchID);
        Task<List<BranchSubjectEntity>> GetMobileSubjectBySubjectID(long SubjectID, long BranchID, long CourseID);

    }
}
