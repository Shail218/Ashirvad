using Ashirvad.Common;
using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Course
{
    public interface ICourseService
    {
        Task<ResponseModel> CourseMaintenance(CourseEntity courseEntity);
        Task<OperationResult<CourseEntity>> GetCourseByCourseID(long courseID);
        Task<OperationResult<List<CourseEntity>>> GetAllCourse();
        ResponseModel RemoveCourse(long courseID, string lastupdatedby);
        Task<OperationResult<List<CourseEntity>>> GetAllCustomCourse(DataTableAjaxPostModel model);
    }
}
