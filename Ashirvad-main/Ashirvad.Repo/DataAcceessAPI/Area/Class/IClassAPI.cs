using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Class
{
    public interface IClassAPI
    {
        Task<long> ClassMaintenance(ClassEntity classEntity);
        Task<ClassEntity> GetClassByClassID(long classID);
        Task<List<ClassEntity>> GetAllClass();
        bool RemoveClass(long classID, string lastupdatedby);
    }
}
