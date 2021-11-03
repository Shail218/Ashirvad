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
        Task<HomeworkDetailEntity> HomeworkdetailMaintenance(HomeworkDetailEntity homeworkDetail);
        Task<List<HomeworkDetailEntity>> GetAllHomeworkdetailByHomeWork(long HomeworkID);
        Task<List<HomeworkDetailEntity>> GetAllHomeworkdetailWithoutContentByHomeWork(long HomeworkID);
        Task<List<HomeworkDetailEntity>> GetAllHomeworkdetailByHomeWorkStudentID(long HomeworkID, long studentID);
        Task<HomeworkDetailEntity> GetHomeworkByID(long HomeWorkDetailID);
        bool RemoveHomeWork(long HomeWorkDetailID, string lastupdatedby);
        bool RemoveHomeworkdetail(long homeworkdetailID, long UserID);
        Task<HomeworkDetailEntity> Homeworkdetailupdate(HomeworkDetailEntity homeworkDetail);
    }
}
