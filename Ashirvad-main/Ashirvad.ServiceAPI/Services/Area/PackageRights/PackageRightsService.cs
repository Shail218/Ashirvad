using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area
{
    public class PackageRightsService : IPackageRightsService
    {
        private readonly IPackageRightsAPI _PackageRightsContext;
        public PackageRightsService(IPackageRightsAPI PackageRightsContext)
        {
            this._PackageRightsContext = PackageRightsContext;
        }

        public async Task<PackageRightEntity> PackageRightsMaintenance(PackageRightEntity PackageRightsInfo)
        {
            PackageRightEntity PackageRights = new PackageRightEntity();
            try
            {
                long PackageRightsID = await _PackageRightsContext.RightsMaintenance(PackageRightsInfo);
                PackageRights.PackageRightsId = PackageRightsID;

            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return PackageRights;
        }

        

        public async Task<PackageRightEntity> GetPackageRightsByPackageRightsID(long PackageRightsID)
        {
            try
            {
                PackageRightEntity PackageRights = new PackageRightEntity();
                PackageRights = await _PackageRightsContext.GetRightsByRightsID(PackageRightsID);                
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

        public bool RemovePackageRights(long PackageRightsID, string lastupdatedby)
        {
            try
            {
                return this._PackageRightsContext.RemoveRights(PackageRightsID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

        
    }
}
