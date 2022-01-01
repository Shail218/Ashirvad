using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.DataAcceessAPI.Area.Branch
{
    public interface IBranchAPI
    {
        Task<long> BranchMaintenance(BranchEntity branchInfo);
        Task<List<BranchEntity>> GetAllBranch();
        Task<List<BranchEntity>> GetAllBranchWithoutImage();
        Task<BranchEntity> GetBranchByBranchID(long branchID);
        bool RemoveBranch(long BranchID, string lastupdatedby);
        Task<long> AgreementMaintenance(BranchAgreementEntity agrInfo);
        Task<List<BranchAgreementEntity>> GetAllAgreements(long branchID);
        Task<BranchAgreementEntity> GetAgreementByID(long agreementID);
        bool RemoveAgreement(long agreementID, string lastupdatedby);
        Task<List<BranchEntity>> GetAllCustomBranch(DataTableAjaxPostModel model);
        Task<List<BranchAgreementEntity>> GetAllCustomAgreement(DataTableAjaxPostModel model, long branchID);
    }
}
