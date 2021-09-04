using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Branch;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Branch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.Branch
{
    public class BranchService : IBranchService
    {
        private readonly IBranchAPI _branchContext;
        public BranchService(IBranchAPI branchContext)
        {
            this._branchContext = branchContext;
        }

        public async Task<BranchEntity> BranchMaintenance(BranchEntity branchInfo)
        {
            BranchEntity branch = new BranchEntity();
            try
            {
                long branchID = await _branchContext.BranchMaintenance(branchInfo);
                if (branchID > 0)
                {
                    //Add User
                    //Get Branch
                    branch.BranchID = branchID;
                }
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return branch;
        }

        public async Task<OperationResult<List<BranchEntity>>> GetAllBranchWithoutImage()
        {
            try
            {
                OperationResult<List<BranchEntity>> branch = new OperationResult<List<BranchEntity>>();
                branch.Data = await _branchContext.GetAllBranchWithoutImage();
                branch.Completed = true;
                return branch;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<BranchEntity>> GetBranchByBranchID(long branchID)
        {
            try
            {
                OperationResult<BranchEntity> branch = new OperationResult<BranchEntity>();
                branch.Data = await _branchContext.GetBranchByBranchID(branchID);
                branch.Completed = true;
                return branch;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<BranchEntity>> GetAllBranch()
        {
            try
            {
                return await this._branchContext.GetAllBranch();
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemoveBranch(long BranchID, string lastupdatedby)
        {
            try
            {
                return this._branchContext.RemoveBranch(BranchID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

        public async Task<BranchAgreementEntity> AgreementMaintenance(BranchAgreementEntity agreementInfo)
        {
            BranchAgreementEntity agreement = new BranchAgreementEntity();
            try
            {
                long agrID = await _branchContext.AgreementMaintenance(agreementInfo);
                if (agrID > 0)
                {
                    //Add User
                    //Get Branch
                    agreement.AgreementID = agrID;
                }
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return agreement;
        }

        public async Task<OperationResult<List<BranchAgreementEntity>>> GetAllAgreement(long branchID)
        {
            try
            {
                OperationResult<List<BranchAgreementEntity>> agreements = new OperationResult<List<BranchAgreementEntity>>();
                agreements.Data = await _branchContext.GetAllAgreements(branchID);
                agreements.Completed = true;
                return agreements;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<OperationResult<BranchAgreementEntity>> GetAgreementByAgreementID(long agreementID)
        {
            try
            {
                OperationResult<BranchAgreementEntity> agreement = new OperationResult<BranchAgreementEntity>();
                agreement.Data = await _branchContext.GetAgreementByID(agreementID);
                agreement.Completed = true;
                return agreement;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        
        public bool RemoveAgreement(long AgreementID, string lastupdatedby)
        {
            try
            {
                return this._branchContext.RemoveAgreement(AgreementID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

    }
}
