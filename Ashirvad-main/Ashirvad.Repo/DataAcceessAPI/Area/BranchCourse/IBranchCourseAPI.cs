using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area
{
    public interface IBranchCourseAPI
    {
        Task<long> CourseMaintenance(BranchCourseEntity CourseInfo);
        Task<List<BranchCourseEntity>> GetAllCourse(long BranchID);
        Task<List<BranchCourseEntity>> GetCourseByCourseID(long CourseID);
        ResponseModel RemoveCourse(long CourseID, string lastupdatedby);
        Task<BranchCourseEntity> GetCoursebyID(long CourseID);
        Task<List<BranchCourseEntity>> GetAllSelectedCourses(long BranchID);

    }
}
