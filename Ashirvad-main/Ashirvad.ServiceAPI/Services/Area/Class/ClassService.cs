using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Class;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area.Class
{
    public class ClassService : IClassService
    {
        private readonly IClassAPI _classService;

        public ClassService(IClassAPI classService)
        {
            this._classService = classService;
        }

        public async Task<ResponseModel> ClassMaintenance(ClassEntity classEntity)
        {
            ClassEntity cl = new ClassEntity();
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _classService.ClassMaintenance(classEntity);
                //cl.ClassID = classID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<OperationResult<ClassEntity>> GetClassByClassID(long classID)
        {
            try
            {
                OperationResult<ClassEntity> course = new OperationResult<ClassEntity>();
                course.Data = await _classService.GetClassByClassID(classID);
                course.Completed = true;
                return course;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<List<ClassEntity>>> GetAllClass()
        {
            try
            {
                OperationResult<List<ClassEntity>> course = new OperationResult<List<ClassEntity>>();
                course.Data = await _classService.GetAllClass();
                course.Completed = true;
                return course;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<List<ClassEntity>>> GetAllClassByCourse(long courseid, bool Isupdate = false)
        {
            try
            {
                OperationResult<List<ClassEntity>> course = new OperationResult<List<ClassEntity>>();
                course.Data = await _classService.GetAllClassByCourse(courseid, Isupdate);
                course.Completed = true;
                return course;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<BranchClassEntity>> GetAllClassByCourseddl(long courseid, bool Isupdate = false)
        {
            try
            {
                List<BranchClassEntity> course = new List<BranchClassEntity>();
                course = await _classService.GetAllClassByCourseddl(courseid, Isupdate);                
                return course;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        
        public async Task<OperationResult<List<ClassEntity>>> GetAllCustomClass(DataTableAjaxPostModel model)
        {
            try
            {
                OperationResult<List<ClassEntity>> course = new OperationResult<List<ClassEntity>>();
                course.Data = await _classService.GetAllCustomClass(model);
                course.Completed = true;
                return course;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveClass(long classID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                return this._classService.RemoveClass(classID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<List<CourseEntity>> GetAllCourse()
        {
            try
            {
                return await this._classService.GetAllCourse();
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<ClassEntity>> GetAllBranchClassDDL(long BranchID = 0, long ClassID = 0)
        {
            try
            {
                return await this._classService.GetAllClassDDL(BranchID, ClassID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
    }
}
