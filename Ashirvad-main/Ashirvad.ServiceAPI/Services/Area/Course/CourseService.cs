﻿using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Course;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.Course
{
    public class CourseService : ICourseService
    {
        private readonly ICourseAPI _coursecontext;

        public CourseService(ICourseAPI coursecontext)
        {
            this._coursecontext = coursecontext;
        }

        public async Task<CourseEntity> CourseMaintenance(CourseEntity courseEntity)
        {
            CourseEntity course = new CourseEntity();
            try
            {
                long courseID = await _coursecontext.CourseMaintenance(courseEntity);
                course.CourseID = courseID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return course;
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

        public bool RemoveCourse(long courseID, string lastupdatedby)
        {
            try
            {
                return this._coursecontext.RemoveCourse(courseID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }
    }
}