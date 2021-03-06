using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area
{
    public interface IBranchSubjectAPI
    {
        Task<ResponseModel> SubjectMaintenance(BranchSubjectEntity SubjectInfo);
        Task<List<BranchSubjectEntity>> GetAllSubject(long BranchID);
        Task<List<BranchSubjectEntity>> GetSubjectBySubjectID(long SubjectID,long BranchID, long ClassID);
        ResponseModel RemoveSubject(long CourseID,long ClassID, long BranchID, string lastupdatedby);
        Task<BranchSubjectEntity> GetSubjectbyID(long SubjectID);
        Task<List<BranchSubjectEntity>> GetAllSelectedSubjects(long BranchID, long CourseID, long ClassID);
        Task<List<BranchSubjectEntity>> GetMobileAllSubject(long BranchID);
        Task<List<BranchSubjectEntity>> GetMobileSubjectBySubjectID(long SubjectID, long BranchID, long CourseID);
        Task<List<BranchSubjectEntity>> GetAllSubjects(DataTableAjaxPostModel model, long BranchID);
        Task<List<BranchSubjectEntity>> GetSubjectDDL(long courseid, long branchid, string std);
        Task<List<BranchSubjectEntity>> GetSubjectByclasscourseid(long SubjectID, long BranchID, long CourseID);
    }
}
