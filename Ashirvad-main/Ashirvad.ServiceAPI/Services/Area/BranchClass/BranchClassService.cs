using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.ServiceAPI.Services.Area
{
    public class BranchClassService : IBranchClassService
    {
        private readonly IBranchClassAPI _BranchClassContext;        

        public BranchClassService(IBranchClassAPI BranchClassContext)
        {
            this._BranchClassContext = BranchClassContext;
        }

        public async Task<ResponseModel> BranchClassMaintenance(BranchClassEntity BranchClassInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _BranchClassContext.ClassMaintenance(BranchClassInfo);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return responseModel;
        }

        

        public async Task<List<BranchClassEntity>> GetBranchClassByBranchClassID(long BranchClassID, long BranchID)
        {
            try
            {
                List<BranchClassEntity> BranchClass = new List<BranchClassEntity>();
                BranchClass = await _BranchClassContext.GetClassByClassID(BranchClassID,BranchID);                
                return BranchClass;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }


        public async Task<BranchClassEntity> GetPackaegBranchClassByID(long BranchClassID)
        {
            try
            {
               BranchClassEntity BranchClass = new BranchClassEntity();
                BranchClass = await _BranchClassContext.GetClassbyID(BranchClassID);
                return BranchClass;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<BranchClassEntity>> GetAllBranchClass(DataTableAjaxPostModel model,long BranchID=0,long ClassID=0)
        {
            try
            {
                return await this._BranchClassContext.GetAllClass(model,BranchID, ClassID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        public async Task<List<BranchClassEntity>> GetAllBranchClassDDL(long BranchID = 0, long ClassID = 0)
        {
            try
            {
                return await this._BranchClassContext.GetAllClassDDL(BranchID, ClassID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        public async Task<List<BranchClassEntity>> GetMobileAllBranchClass(long BranchID = 0, long ClassID = 0)
        {
            try
            {
                return await this._BranchClassContext.GetMobileAllClass(BranchID, ClassID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public ResponseModel RemoveBranchClass(long BranchClassID,long BranchID, string lastupdatedby)
        {
            try
            {
                return this._BranchClassContext.RemoveClass(BranchClassID, BranchID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
        public async Task<List<BranchClassEntity>> GetAllSelectedClasses(long BranchID = 0, long CourseID = 0)
        {
            try
            {
                return await this._BranchClassContext.GetAllSelectedClasses(BranchID, CourseID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }


    }
}
