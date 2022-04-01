using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area
{
    public interface IBranchClassAPI
    {
        Task<ResponseModel> ClassMaintenance(BranchClassEntity ClassInfo);
        Task<ResponseModel> StandardMaintenance(StandardEntity standardInfo);
        Task<List<BranchClassEntity>> GetAllClass(DataTableAjaxPostModel model,long BranchID,long ClassID= 0);
        Task<List<BranchClassEntity>> GetAllClassDDL(long BranchID,long ClassID= 0);
        Task<List<BranchClassEntity>> GetMobileAllClass(long BranchID, long ClassID = 0);
        Task<List<BranchClassEntity>> GetClassByClassID(long ClassID,long BranchID);
        ResponseModel RemoveClass(long ClassID, long BranchID, string lastupdatedby);
        Task<BranchClassEntity> GetClassbyID(long ClassID);
        Task<List<BranchClassEntity>> GetAllSelectedClasses(long BranchID, long CourseID);
    }
}
