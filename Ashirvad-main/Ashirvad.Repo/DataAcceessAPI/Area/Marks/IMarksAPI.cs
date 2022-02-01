using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area
{
    public interface IMarksAPI
    {
        Task<long> MarksMaintenance(MarksEntity branchInfo);
        Task<List<MarksEntity>> GetAllMarks();
        Task<MarksEntity> GetMarksByMarksID(long MarksID);
        bool RemoveMarks(long MarksID, string lastupdatedby);
        Task<List<MarksEntity>> GetAllAchieveMarks(long Std, long Branch, long Batch, long MarksID);
        Task<long> UpdateMarksDetails(MarksEntity marksEntity);
        Task<List<MarksEntity>> GetAllStudentMarks(long BranchID, long StudentID);
        Task<List<MarksEntity>> GetAllCustomMarks(DataTableAjaxPostModel model, long Std, long courseid, long Branch, long Batch, long MarksID);
    }
}
