using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Course;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area.Course
{
    public class CourseService : ICourseService
    {
        private readonly ICourseAPI _coursecontext;

        public CourseService(ICourseAPI coursecontext)
        {
            this._coursecontext = coursecontext;
        }

        public async Task<ResponseModel> CourseMaintenance(CourseEntity courseEntity)
        {
            ResponseModel responseModel = new ResponseModel();
            CourseEntity course = new CourseEntity();
            try
            {
                responseModel = await _coursecontext.CourseMaintenance(courseEntity);
               // course.CourseID = courseID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<OperationResult<CourseEntity>> GetCourseByCourseID(long courseID)
        {
            try
            {
                OperationResult<CourseEntity> course = new OperationResult<CourseEntity>();
                course.Data = await _coursecontext.GetCourseByCourseID(courseID);
                course.Completed = true;
                return course;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<List<CourseEntity>>> GetAllCourse()
        {
            try
            {
                OperationResult<List<CourseEntity>> course = new OperationResult<List<CourseEntity>>();
                course.Data = await _coursecontext.GetAllCourse();
                course.Completed = true;
                return course;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<List<CourseEntity>>> GetAllCustomCourse(DataTableAjaxPostModel model)
        {
            try
            {
                OperationResult<List<CourseEntity>> course = new OperationResult<List<CourseEntity>>();
                course.Data = await _coursecontext.GetAllCustomCourse(model);
                course.Completed = true;
                return course;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveCourse(long courseID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                return this._coursecontext.RemoveCourse(courseID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }
    }
}
