using Ashirvad.Data;
using Ashirvad.Logger;
using Ashirvad.Repo.DataAcceessAPI.Area.Package;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.ServiceAPI.Services.Area.Package
{
   public class PackageService : IPackageService
    {
        private readonly IPackageAPI _packageContext;
        public PackageService(IPackageAPI packageContext)
        {
            this._packageContext = packageContext;
        }

        public async Task<PackageEntity> PackageMaintenance(PackageEntity packageInfo)
        {
            PackageEntity standard = new PackageEntity();
            try
            {
                long packageID = await _packageContext.PackageMaintenance(packageInfo);
                standard.PackageID = packageID;
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }
            return standard;
        }

        public async Task<List<PackageEntity>> GetAllPackages(long branchID)
        {
            try
            {
                return await this._packageContext.GetAllPackages(branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<PackageEntity>> GetAllCustomPackage(Common.Common.DataTableAjaxPostModel model, long branchID)
        {
            try
            {
                return await this._packageContext.GetAllCustomPackage(model, branchID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public async Task<List<PackageEntity>> GetAllPackages()
        {
            try
            {
                return await this._packageContext.GetAllPackages();
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }

        public bool RemovePackage(long PackageID, string lastupdatedby)
        {
            try
            {
                return this._packageContext.RemovePackage(PackageID, lastupdatedby);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return false;
        }

        public async Task<PackageEntity> GetPackageByIDAsync(long packageID)
        {
            try
            {
                return await this._packageContext.GetPackageByID(packageID);
            }
            catch (Exception ex)
            {
                EventLogger.WriteEvent(Logger.Severity.Error, ex);
            }

            return null;
        }
    }
}
