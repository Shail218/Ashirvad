using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area
{
    public interface IBranchCourseService
    {
        Task<BranchCourseEntity> BranchCourseMaintenance(BranchCourseEntity BranchCourseInfo);
        Task<List<BranchCourseEntity>> GetAllBranchCourse(long BrancchID=0);

        Task<List<BranchCourseEntity>> GetBranchCourseByBranchCourseID(long BranchCourseID);
            Task<BranchCourseEntity> GetPackaegBranchCourseByID(long BranchCourseID);
        bool RemoveBranchCourse(long BranchCourseID, string lastupdatedby);
        
    }
}
