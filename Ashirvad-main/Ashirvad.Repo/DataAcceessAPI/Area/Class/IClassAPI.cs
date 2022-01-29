using Ashirvad.Common;
using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Class
{
    public interface IClassAPI
    {
        Task<long> ClassMaintenance(ClassEntity classEntity);
        Task<ClassEntity> GetClassByClassID(long classID);
        Task<List<ClassEntity>> GetAllClass();
        bool RemoveClass(long classID, string lastupdatedby);
        Task<List<ClassEntity>> GetAllCustomClass(DataTableAjaxPostModel model);
        Task<List<CourseEntity>> GetAllCourse();
        Task<List<ClassEntity>> GetAllClassDDL(long BranchID, long ClassID = 0);
        Task<List<ClassEntity>> GetAllClassByCourse(long courseid, bool Isupdate = false);
        Task<List<BranchClassEntity>> GetAllClassByCourseddl(long courseid, bool Isupdate = false);
    }
}
