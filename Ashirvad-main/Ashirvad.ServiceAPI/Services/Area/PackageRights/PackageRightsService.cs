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
    public class PackageRightsService : IPackageRightsService
    {
        private readonly IPackageRightsAPI _PackageRightsContext;
        public PackageRightsService(IPackageRightsAPI PackageRightsContext)
        {
            this._PackageRightsContext = PackageRightsContext;
        }

        public async Task<ResponseModel> PackageRightsMaintenance(PackageRightEntity PackageRightsInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            PackageRightEntity PackageRights = new PackageRightEntity();
            try
            {
                //long PackageRightsID = await _PackageRightsContext.RightsMaintenance(PackageRightsInfo);
                responseModel = await _PackageRightsContext.RightsMaintenance(PackageRightsInfo);
                //PackageRights.PackageRightsId = PackageRightsID;

            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            //return PackageRights;
            return responseModel;
        }

        

        public async Task<List<PackageRightEntity>> GetPackageRightsByPackageRightsID(long PackageRightsID)
        {
            try
            {
                List<PackageRightEntity> PackageRights = new List<PackageRightEntity>();
                PackageRights = await _PackageRightsContext.GetRightsByRightsID(PackageRightsID);                
                return PackageRights;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }


        public async Task<PackageRightEntity> GetPackaegrightsByID(long PackageRightsID)
        {
            try
            {
               PackageRightEntity PackageRights = new PackageRightEntity();
                PackageRights = await _PackageRightsContext.GetPackagebyID(PackageRightsID);
                return PackageRights;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<PackageRightEntity>> GetAllPackageRights()
        {
            try
            {
                return await this._PackageRightsContext.GetAllRights();
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<PackageRightEntity>> GetAllCustomRights(DataTableAjaxPostModel model)
        {
            try
            {
                return await this._PackageRightsContext.GetAllCustomRights(model);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return null;
        }

        public ResponseModel RemovePackageRights(long PackageRightsID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                return this._PackageRightsContext.RemoveRights(PackageRightsID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return responseModel;
            // return false;
        }

        
    }
}
