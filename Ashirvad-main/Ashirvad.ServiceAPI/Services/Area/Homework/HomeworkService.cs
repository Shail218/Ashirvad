using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Homework;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Homework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area.Homework
{
    public class HomeworkService : IHomeworkService
    {
        private readonly IHomeworkAPI _homeworkContext;
        public HomeworkService(IHomeworkAPI homeworkContext)
        {
            this._homeworkContext = homeworkContext;
        }

        public async Task<HomeworkEntity> HomeworkMaintenance(HomeworkEntity homework)
        {
            HomeworkEntity hw = new HomeworkEntity();
            try
            {
                long HomeworkID = await _homeworkContext.HomeworkMaintenance(homework);
                hw.HomeworkID = HomeworkID;


            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return hw;
        }

        public async Task<List<HomeworkEntity>> GetAllHomeworkByBranchStudent(long branchID,long courseid, long stdID, int batchTime, string financialyear, long studentId=0)
        {
            try
            {
                var data = await _homeworkContext.GetAllHomeworkByBranchStudent(branchID,courseid, stdID, batchTime,financialyear, studentId);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<HomeworkEntity>> GetAllHomeworkByBranch(long branchID, string financialyear, long stdID = 0, int batchTime = 0)
        {
            try
            {
                var data = await _homeworkContext.GetAllHomeworkByBranch(branchID, stdID, batchTime,financialyear);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<HomeworkEntity>> GetAllHomeworks(DateTime hwDate, string searchParam, string financialyear)
        {
            try
            {
                var data = await _homeworkContext.GetAllHomeworks(hwDate, searchParam,financialyear);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<HomeworkEntity>> GetAllHomeworkWithoutContentByBranch(long branchID, string financialyear, long stdID = 0)
        {
            try
            {
                var data = await _homeworkContext.GetAllHomeworkWithoutContentByBranch(branchID, stdID,financialyear);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<HomeworkEntity>> GetAllCustomHomework(DataTableAjaxPostModel model, long branchID, string financialyear)
        {
            try
            {
                return await this._homeworkContext.GetAllCustomHomework(model, branchID,financialyear);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<HomeworkEntity> GetHomeworkByHomeworkID(long hwID, string financialyear)
        {
            try
            {
                var data = await _homeworkContext.GetHomeworkByHomeworkID(hwID,financialyear);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        public async Task<List<HomeworkEntity>> GetStudentHomeworkChecking(long hwID, string financialyear)
        {
            try
            {
                var data = await _homeworkContext.GetStudentHomeworkChecking(hwID,financialyear);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemoveHomework(long hwID, string lastupdatedby)
        {
            try
            {
                var data = _homeworkContext.RemoveHomework(hwID, lastupdatedby);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

        public async Task<List<HomeworkEntity>>GetHomeworkdetailsFiles(long hwID, string financialyear)
        {
            try
            {
                var data = await _homeworkContext.GetStudentHomeworkFile(hwID,financialyear);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
    }
}
