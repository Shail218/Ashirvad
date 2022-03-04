using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area
{
    public class BranchCourseService : IBranchCourseService
    {
        private readonly IBranchCourseAPI _BranchCourseContext;        

        public BranchCourseService(IBranchCourseAPI BranchCourseContext)
        {
            this._BranchCourseContext = BranchCourseContext;
        }

        public async Task<BranchCourseEntity> BranchCourseMaintenance(BranchCourseEntity BranchCourseInfo)
        {
            BranchCourseEntity BranchCourse = new BranchCourseEntity();
            try
            {
                long BranchCourseID = await _BranchCourseContext.CourseMaintenance(BranchCourseInfo);
                BranchCourse.Data = BranchCourseID;

            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return BranchCourse;
        }

        

        public async Task<List<BranchCourseEntity>> GetBranchCourseByBranchCourseID(long BranchCourseID)
        {
            try
            {
                List<BranchCourseEntity> BranchCourse = new List<BranchCourseEntity>();
                BranchCourse = await _BranchCourseContext.GetCourseByCourseID(BranchCourseID);                
                return BranchCourse;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }


        public async Task<BranchCourseEntity> GetPackaegBranchCourseByID(long BranchCourseID)
        {
            try
            {
               BranchCourseEntity BranchCourse = new BranchCourseEntity();
                BranchCourse = await _BranchCourseContext.GetCoursebyID(BranchCourseID);
                return BranchCourse;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<BranchCourseEntity>> GetAllBranchCourse(long BranchID=0)
        {
            try
            {
                return await this._BranchCourseContext.GetAllCourse(BranchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        public async Task<List<BranchCourseEntity>> GetAllBranchCourseforExport(long BranchID = 0)
        {
            try
            {
                return await this._BranchCourseContext.GetAllCourseforExport(BranchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveBranchCourse(long BranchCourseID, string lastupdatedby)
        {
            try
            {
                return this._BranchCourseContext.RemoveCourse(BranchCourseID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<BranchCourseEntity>> GetAllSelectedCourses(long BranchID)
        {
            try
            {
                return await this._BranchCourseContext.GetAllSelectedCourses(BranchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }


    }
}
