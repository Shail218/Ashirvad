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
                if (homework.FileInfo != null)
                {
                    homework.HomeworkContent = Common.Common.ReadFully(homework.FileInfo.InputStream);
                    homework.HomeworkContentFileName = Path.GetFileName(homework.FileInfo.FileName);
                }
                else
                {
                    homework.HomeworkContent = Convert.FromBase64String(homework.HomeworkContentText);
                }


                var data = await _homeworkContext.HomeworkMaintenance(homework);
                if (data > 0)
                {
                    if (!string.IsNullOrEmpty(Common.Common.GetStringConfigKey("DocDirectory")))
                    {
                        Common.Common.SaveFile(homework.HomeworkContent, homework.HomeworkContentFileName, "Homework\\");
                    }
                }

                hw = homework;
                hw.HomeworkID = data;
                return hw;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return hw;
        }

        public async Task<List<HomeworkEntity>> GetAllHomeworkByBranch(long branchID, long stdID = 0, int batchTime = 0)
        {
            try
            {
                var data = await _homeworkContext.GetAllHomeworkByBranch(branchID, stdID, batchTime);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<HomeworkEntity>> GetAllHomeworks(DateTime hwDate, string searchParam)
        {
            try
            {
                var data = await _homeworkContext.GetAllHomeworks(hwDate, searchParam);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<HomeworkEntity>> GetAllHomeworkWithoutContentByBranch(long branchID, long stdID = 0)
        {
            try
            {
                var data = await _homeworkContext.GetAllHomeworkWithoutContentByBranch(branchID, stdID);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<HomeworkEntity> GetHomeworkByHomeworkID(long hwID)
        {
            try
            {
                var data = await _homeworkContext.GetHomeworkByHomeworkID(hwID);
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
    }
}
