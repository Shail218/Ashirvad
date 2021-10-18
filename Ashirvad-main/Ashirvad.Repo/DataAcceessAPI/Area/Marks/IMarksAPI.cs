using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.DataAcceessAPI.Area
{
    public interface IMarksAPI
    {
        Task<long> MarksMaintenance(MarksEntity branchInfo);
        Task<List<MarksEntity>> GetAllMarks();
        Task<List<MarksEntity>> GetAllMarksWithoutImage();
        Task<MarksEntity> GetMarksByMarksID(long MarksID);
        bool RemoveMarks(long MarksID, string lastupdatedby);
       
    }
}
