using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area
{
    public interface IMarksService
    {
        Task<MarksEntity> MarksMaintenance(MarksEntity MarksInfo);
        Task<List<MarksEntity>> GetAllMarks();
    
        Task<List<MarksEntity>> GetAllMarksWithoutImage();
        Task<MarksEntity> GetMarksByMarksID(long MarksID);
        bool RemoveMarks(long MarksID, string lastupdatedby);
        
    }
}
