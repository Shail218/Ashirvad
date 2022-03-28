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
        Task<ResponseModel> BranchMaintenance(BranchEntity branchInfo);
        Task<List<BranchEntity>> GetAllBranch();
        Task<List<BranchEntity>> GetAllBranchWithoutImage();
        Task<BranchEntity> GetBranchByBranchID(long branchID);
        ResponseModel RemoveBranch(long BranchID, string lastupdatedby);
        Task<ResponseModel> AgreementMaintenance(BranchAgreementEntity agrInfo);
        Task<List<BranchAgreementEntity>> GetAllAgreements(long branchID);
        Task<BranchAgreementEntity> GetAgreementByID(long agreementID);
        ResponseModel RemoveAgreement(long agreementID, string lastupdatedby);
        Task<List<BranchEntity>> GetAllCustomBranch(DataTableAjaxPostModel model);
        Task<List<BranchAgreementEntity>> GetAllCustomAgreement(DataTableAjaxPostModel model, long branchID);
    }
}
