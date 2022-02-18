using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area
{
    public class HomeworkDetailService : IHomeworkDetailService
    {
        private readonly IHomeworkDetailsAPI _testContext;
        public HomeworkDetailService(IHomeworkDetailsAPI testContext)
        {
            _testContext = testContext;
        }

        public async Task<HomeworkDetailEntity> HomeworkdetailMaintenance(HomeworkDetailEntity homeworkDetail)
        {
            HomeworkDetailEntity homeworkDetailEntity = new HomeworkDetailEntity();
            try
            {
                var data = await _testContext.HomeworkMaintenance(homeworkDetail);
                homeworkDetailEntity.HomeworkDetailID = data;
                return homeworkDetailEntity;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return homeworkDetailEntity;
        }

        public async Task<List<HomeworkDetailEntity>> GetAllHomeworkdetailByHomeWork(long HomeworkID)
        {
            try
            {
                var data = await _testContext.GetAllHomeworkByStudent(HomeworkID);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<HomeworkDetailEntity>> GetAllHomeworkdetailWithoutContentByHomeWork(long HomeworkID)
        {
            try
            {
                var data = await _testContext.GetAllHomeworkdetailWithoutContentByStudentID(HomeworkID);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<HomeworkDetailEntity>> GetAllHomeworkdetailByHomeWorkStudentID(long HomeworkID, long studentID)
        {
            try
            {
                var data = await _testContext.GetAllAnsSheetByStudentID(HomeworkID, studentID);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<HomeworkDetailEntity> GetHomeworkByID(long HomeWorkDetailID)
        {
            try
            {
                var data = await _testContext.GetHomeworkByHomeworkID(HomeWorkDetailID);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemoveHomeWork(long HomeWorkDetailID, string lastupdatedby)
        {
            try
            {
                var data = _testContext.RemoveHomework(HomeWorkDetailID, lastupdatedby);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

        public bool RemoveHomeworkdetail(long homeworkdetailID, long UserID)
        {
            try
            {
                var data = _testContext.RemoveHomeworkdetail(homeworkdetailID, UserID);
                return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

        public async Task<HomeworkDetailEntity> Homeworkdetailupdate(HomeworkDetailEntity homeworkDetail)
        {
            HomeworkDetailEntity homeworkDetailEntity = new HomeworkDetailEntity();
            homeworkDetailEntity.HomeworkEntity = new HomeworkEntity();
            try
            {
                var data = await _testContext.HomeworkDetailUpdate(homeworkDetail);
                homeworkDetailEntity.HomeworkEntity.HomeworkID = data;
                return homeworkDetailEntity;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return homeworkDetailEntity;
        }
        public async Task<HomeworkDetailEntity> HomeworkdetailFileupdate(HomeworkDetailEntity homeworkDetail)
        {
            HomeworkDetailEntity homeworkDetailEntity = new HomeworkDetailEntity();
            try
            {
                var data = await _testContext.HomeworkDetailFileUpdate(homeworkDetail);
                homeworkDetailEntity.HomeworkDetailID = data;
                return homeworkDetailEntity;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return homeworkDetailEntity;
        }
    }
}
