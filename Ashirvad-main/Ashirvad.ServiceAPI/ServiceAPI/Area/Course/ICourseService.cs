using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Course
{
    public interface ICourseService
    {
        Task<CourseEntity> CourseMaintenance(CourseEntity courseEntity);
        Task<OperationResult<CourseEntity>> GetCourseByCourseID(long courseID);
        Task<OperationResult<List<CourseEntity>>> GetAllCourse();
        bool RemoveCourse(long courseID, string lastupdatedby);
    }
}
