using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Paper
{
    public interface IPaperService
    {
        Task<PaperEntity> PaperMaintenance(PaperEntity paperInfo);

        Task<OperationResult<List<PaperEntity>>> GetAllPaperWithoutContent( string financialyear,long branchID = 0);
        Task<OperationResult<PaperEntity>> GetPaperByPaperID(long paperID, string financialyear);
        Task<List<PaperEntity>> GetAllPaper(string financialyear,long branchID = 0);
        Task<OperationResult<List<SubjectEntity>>> GetPracticePaperSubject(long branchID,long courseid, long stdID,int batch_time, string financialyear);
        Task<OperationResult<List<PaperEntity>>> GetPracticePapersByStandardSubjectAndBranch(long branchID, long stdID, long subID, int batchTypeID, string financialyear);
        Task<OperationResult<List<SubjectEntity>>> GetPracticePapersSubjectByStandardBatchAndBranch(long branchID, long stdID, int batchTypeID, string financialyear);
        bool RemovePaper(long paperID, string lastupdatedby);
        Task<List<PaperEntity>> GetAllCustomPaper(DataTableAjaxPostModel model, long branchID, string financialyear);
    }
}
