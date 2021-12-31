using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area
{
    public interface IBranchSubjectService
    {
        Task<BranchSubjectEntity> BranchSubjectMaintenance(BranchSubjectEntity BranchSubjectInfo);
        Task<List<BranchSubjectEntity>> GetAllBranchSubject(long BrancchID = 0);
        Task<List<BranchSubjectEntity>> GetMobileAllBranchSubject(long BranchID = 0);
        Task<List<BranchSubjectEntity>> GetBranchSubjectByBranchSubjectID(long BranchSubjectID, long BranchID, long ClassID);
        Task<List<BranchSubjectEntity>> GetMobileBranchSubjectByBranchSubjectID(long BranchSubjectID, long BranchID, long ClassID);
        Task<BranchSubjectEntity> GetPackaegBranchSubjectByID(long BranchSubjectID);
        bool RemoveBranchSubject(long CourseID, long ClassID, long BranchID, string lastupdatedby);
        Task<List<BranchSubjectEntity>> GetAllSubjects(DataTableAjaxPostModel model, long BranchID);
    }
}
