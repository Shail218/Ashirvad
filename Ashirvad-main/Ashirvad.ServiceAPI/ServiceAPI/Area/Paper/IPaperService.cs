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

        Task<OperationResult<List<PaperEntity>>> GetAllPaperWithoutContent(long branchID = 0);
        Task<OperationResult<PaperEntity>> GetPaperByPaperID(long paperID);
        Task<List<PaperEntity>> GetAllPaper(long branchID = 0);
        Task<OperationResult<List<SubjectEntity>>> GetPracticePaperSubject(long branchID,long courseid, long stdID,int batch_time);
        Task<OperationResult<List<PaperEntity>>> GetPracticePapersByStandardSubjectAndBranch(long branchID, long stdID, long subID, int batchTypeID);
        Task<OperationResult<List<SubjectEntity>>> GetPracticePapersSubjectByStandardBatchAndBranch(long branchID, long stdID, int batchTypeID);
        bool RemovePaper(long paperID, string lastupdatedby);
        Task<List<PaperEntity>> GetAllCustomPaper(DataTableAjaxPostModel model, long branchID);
    }
}
