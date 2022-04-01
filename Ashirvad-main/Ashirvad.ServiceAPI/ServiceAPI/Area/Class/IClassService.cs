using Ashirvad.Common;
using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Class
{
    public interface IClassService
    {
        Task<ResponseModel> ClassMaintenance(ClassEntity classEntity);
        Task<OperationResult<ClassEntity>> GetClassByClassID(long classID);
        Task<OperationResult<List<ClassEntity>>> GetAllClass();
        ResponseModel RemoveClass(long classID, string lastupdatedby);
        Task<OperationResult<List<ClassEntity>>> GetAllCustomClass(DataTableAjaxPostModel model);
        Task<List<CourseEntity>> GetAllCourse();
        Task<List<ClassEntity>> GetAllBranchClassDDL(long BrancchID = 0, long CourseID = 0);
        Task<OperationResult<List<ClassEntity>>> GetAllClassByCourse(long courseid, bool Isupdate = false);
        Task<List<BranchClassEntity>> GetAllClassByCourseddl(long courseid, bool Isupdate = false);

    }
}
