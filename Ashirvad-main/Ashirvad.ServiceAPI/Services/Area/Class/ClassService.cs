using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Class;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.Class
{
    public class ClassService : IClassService
    {
        private readonly IClassAPI _classService;

        public ClassService(IClassAPI classService)
        {
            this._classService = classService;
        }

        public async Task<ClassEntity> ClassMaintenance(ClassEntity classEntity)
        {
            ClassEntity cl = new ClassEntity();
            try
            {
                long classID = await _classService.ClassMaintenance(classEntity);
                cl.ClassID = classID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return cl;
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

        public bool RemoveClass(long classID, string lastupdatedby)
        {
            try
            {
                return this._classService.RemoveClass(classID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }
    }
}
