using Ashirvad.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.ServiceAPI.Area.Branch
{
    public interface IBranchService
    {
        Task<BranchEntity> BranchMaintenance(BranchEntity branchInfo);
        Task<List<BranchEntity>> GetAllBranch();

        Task<OperationResult<List<BranchEntity>>> GetAllBranchWithoutImage();
        Task<OperationResult<BranchEntity>> GetBranchByBranchID(long branchID);
        bool RemoveBranch(long BranchID, string lastupdatedby);

        Task<BranchAgreementEntity> AgreementMaintenance(BranchAgreementEntity agreementInfo);

        Task<OperationResult<List<BranchAgreementEntity>>> GetAllAgreement(long branchID);

        Task<OperationResult<BranchAgreementEntity>> GetAgreementByAgreementID(long agreementID);
        bool RemoveAgreement(long agreementID, string lastupdatedby);

        Task<List<BranchEntity>> GetAllCustomBranch(DataTableAjaxPostModel model);
        Task<List<BranchAgreementEntity>> GetAllCustomAgreement(DataTableAjaxPostModel model, long branchID);
    }
}
