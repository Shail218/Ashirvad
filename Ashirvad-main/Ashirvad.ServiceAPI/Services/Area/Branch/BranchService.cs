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

    }
}
