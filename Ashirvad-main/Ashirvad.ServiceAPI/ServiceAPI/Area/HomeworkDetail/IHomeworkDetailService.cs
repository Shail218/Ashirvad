using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area
{
    public interface IHomeworkDetailService
    {
        Task<ResponseModel> HomeworkdetailMaintenance(HomeworkDetailEntity homeworkDetail);
        Task<List<HomeworkDetailEntity>> GetAllHomeworkdetailByHomeWork(long HomeworkID);
        Task<List<HomeworkDetailEntity>> GetAllHomeworkdetailWithoutContentByHomeWork(long HomeworkID);
        Task<List<HomeworkDetailEntity>> GetAllHomeworkdetailByHomeWorkStudentID(long HomeworkID, long studentID);
        Task<HomeworkDetailEntity> GetHomeworkByID(long HomeWorkDetailID);
        ResponseModel RemoveHomeWork(long HomeWorkDetailID, string lastupdatedby);
        ResponseModel RemoveHomeworkdetail(long homeworkdetailID, long UserID);
        Task<ResponseModel> Homeworkdetailupdate(HomeworkDetailEntity homeworkDetail);
        Task<ResponseModel> HomeworkdetailFileupdate(HomeworkDetailEntity homeworkDetail);
       
    }
}
