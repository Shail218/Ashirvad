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

        public async Task<ResponseModel> HomeworkdetailMaintenance(HomeworkDetailEntity homeworkDetail)
        {
            ResponseModel responseModel = new ResponseModel();
            HomeworkDetailEntity homeworkDetailEntity = new HomeworkDetailEntity();
            try
            {
                 responseModel = await _testContext.HomeworkMaintenance(homeworkDetail);
                //homeworkDetailEntity.HomeworkDetailID = data;
                //return homeworkDetailEntity;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
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

        public ResponseModel RemoveHomeWork(long HomeWorkDetailID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = _testContext.RemoveHomework(HomeWorkDetailID, lastupdatedby);
                //return data;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public ResponseModel RemoveHomeworkdetail(long homeworkdetailID, long UserID)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = _testContext.RemoveHomeworkdetail(homeworkdetailID, UserID);
              
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<ResponseModel> Homeworkdetailupdate(HomeworkDetailEntity homeworkDetail)
        {
            HomeworkDetailEntity homeworkDetailEntity = new HomeworkDetailEntity();
            homeworkDetailEntity.HomeworkEntity = new HomeworkEntity();
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _testContext.HomeworkDetailUpdate(homeworkDetail);
                //homeworkDetailEntity.HomeworkEntity.HomeworkID = data;
                //return homeworkDetailEntity;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }
        public async Task<ResponseModel> HomeworkdetailFileupdate(HomeworkDetailEntity homeworkDetail)
        {
            ResponseModel responseModel = new ResponseModel();
            HomeworkDetailEntity homeworkDetailEntity = new HomeworkDetailEntity();
            try
            {
                responseModel = await _testContext.HomeworkDetailFileUpdate(homeworkDetail);
                //homeworkDetailEntity.HomeworkDetailID = data;
                //return homeworkDetailEntity;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }
    }
}
