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
        Task<ResponseModel> BranchCourseMaintenance(BranchCourseEntity BranchCourseInfo);
        Task<List<BranchCourseEntity>> GetAllBranchCourse(long BrancchID = 0);

        Task<List<BranchCourseEntity>> GetBranchCourseByBranchCourseID(long BranchCourseID);
        Task<BranchCourseEntity> GetPackaegBranchCourseByID(long BranchCourseID);
        ResponseModel RemoveBranchCourse(long BranchCourseID, string lastupdatedby);
        Task<List<BranchCourseEntity>> GetAllSelectedCourses(long BranchID);

        Task<List<BranchCourseEntity>> GetAllBranchCourseforExport(long BranchID = 0);
    }
}
