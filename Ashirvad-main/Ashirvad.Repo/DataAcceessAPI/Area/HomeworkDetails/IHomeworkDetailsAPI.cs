using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area
{
    public interface IHomeworkDetailsAPI
    {
        Task<ResponseModel> HomeworkMaintenance(HomeworkDetailEntity homeworkDetail);
        Task<List<HomeworkDetailEntity>> GetAllHomeworkByStudent(long homeworkID);
        Task<List<HomeworkDetailEntity>> GetAllHomeworkdetailWithoutContentByStudentID(long homeworkID);
        Task<List<HomeworkDetailEntity>> GetAllAnsSheetByStudentID(long homeworkID, long studentID);
        Task<HomeworkDetailEntity> GetHomeworkByHomeworkID(long homeworkdetailID);
        ResponseModel RemoveHomework(long homeworkdetailID, string lastupdatedby);
        ResponseModel RemoveHomeworkdetail(long homeworkdetailID, long UserID);

        Task<ResponseModel> HomeworkDetailUpdate(HomeworkDetailEntity homeworkDetail);
        Task<ResponseModel> HomeworkDetailFileUpdate(HomeworkDetailEntity homeworkDetail);
    }
}
