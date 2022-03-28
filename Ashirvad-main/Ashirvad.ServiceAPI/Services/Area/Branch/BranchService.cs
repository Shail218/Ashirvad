using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Branch;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Branch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area.Branch
{
    public class BranchService : IBranchService
    {
        private readonly IBranchAPI _branchContext;
        public BranchService(IBranchAPI branchContext)
        {
            this._branchContext = branchContext;
        }

        public async Task<ResponseModel> BranchMaintenance(BranchEntity branchInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            BranchEntity branch = new BranchEntity();
            try
            {
                responseModel = await _branchContext.BranchMaintenance(branchInfo);
               
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
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

        public async Task<List<BranchEntity>> GetAllCustomBranch(DataTableAjaxPostModel model)
        {
            try
            {
                return await this._branchContext.GetAllCustomBranch(model);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        
        
        
        public ResponseModel RemoveBranch(long BranchID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                return this._branchContext.RemoveBranch(BranchID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public async Task<ResponseModel> AgreementMaintenance(BranchAgreementEntity agreementInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            BranchAgreementEntity agreement = new BranchAgreementEntity();
            try
            {
                responseModel = await _branchContext.AgreementMaintenance(agreementInfo);
                //agreement.AgreementID = agrID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
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

        public async Task<List<BranchAgreementEntity>> GetAllCustomAgreement(DataTableAjaxPostModel model, long branchID)
        {
            try
            {
                return await this._branchContext.GetAllCustomAgreement(model, branchID);
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
        
        public ResponseModel RemoveAgreement(long AgreementID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                return this._branchContext.RemoveAgreement(AgreementID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        public Task<List<BranchEntity>> GetAllBranch(Common.Common.DataTableAjaxPostModel model)
        {
            throw new NotImplementedException();
        }
    }
}
