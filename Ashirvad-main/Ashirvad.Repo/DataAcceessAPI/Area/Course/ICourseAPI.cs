using Ashirvad.Common;
using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Course
{
    public interface ICourseAPI
    {
        Task<long> CourseMaintenance(CourseEntity courseEntity);
        Task<CourseEntity> GetCourseByCourseID(long courseID);
        Task<List<CourseEntity>> GetAllCourse();
        bool RemoveCourse(long courseID, string lastupdatedby);
        Task<List<CourseEntity>> GetAllCustomCourse(DataTableAjaxPostModel model);
    }
}
