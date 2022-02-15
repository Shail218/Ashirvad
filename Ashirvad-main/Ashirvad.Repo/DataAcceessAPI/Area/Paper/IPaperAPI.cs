using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Paper
{
    public interface IPaperAPI
    {
        Task<long> PaperMaintenance(PaperEntity paperInfo);
        Task<List<PaperEntity>> GetAllPapers(long branchID);
        Task<List<PaperEntity>> GetAllPaperWithoutContent(long branchID);
        Task<PaperEntity> GetPaperByPaperID(long paperID);
        bool RemovePaper(long paperID, string lastupdatedby);
        Task<List<PaperEntity>> GetPracticePapersByStandardSubjectAndBranch(long branchID, long stdID, long subID, int batchTypeID);
        Task<List<SubjectEntity>> GetPracticePaperSubject(long branchID,long courseid, long stdID,int batch_time);
        Task<List<PaperEntity>> GetAllCustomPaper(DataTableAjaxPostModel model, long branchID);
    }
}
