using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Package
{
   public class Package : ModelAccess, IPackageAPI
    {
        public async Task<long> CheckPackage(string name, long branch, long Id)
        {
            long result;
            bool isExists = this.context.PACKAGE_MASTER.Where(s => (Id == 0 || s.package_id != Id) && s.package == name && s.branch_id == branch && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<long> PackageMaintenance(PackageEntity packageInfo)
        {
            Model.PACKAGE_MASTER packageMaster = new Model.PACKAGE_MASTER();
            if (CheckPackage(packageInfo.Package, packageInfo.BranchInfo.BranchID, packageInfo.PackageID).Result != -1)
            {
                bool isUpdate = true;
                var data = (from package in this.context.PACKAGE_MASTER
                            where package.package_id == packageInfo.PackageID
                            select package).FirstOrDefault();
                if (data == null)
                {
                    packageMaster = new Model.PACKAGE_MASTER();

                    isUpdate = false;
                }
                else
                {
                    packageMaster = data;
                    packageInfo.Transaction.TransactionId = data.trans_id;
                }

                packageMaster.package = packageInfo.Package;
                packageMaster.branch_id = packageInfo.BranchInfo.BranchID;
                packageMaster.row_sta_cd = packageInfo.RowStatus.RowStatusId;
                packageMaster.trans_id = this.AddTransactionData(packageInfo.Transaction);
                this.context.PACKAGE_MASTER.Add(packageMaster);
                if (isUpdate)
                {
                    this.context.Entry(packageMaster).State = System.Data.Entity.EntityState.Modified;
                }
                return this.context.SaveChanges() > 0 ? packageMaster.package_id : 0;
            }
            else
            {
                return -1;
            }

        }

        public async Task<List<PackageEntity>> GetAllPackages(long branchID)
        {
            var data = (from u in this.context.PACKAGE_MASTER
                        where branchID == 0 || u.branch_id == branchID && u.row_sta_cd == 1
                        select new PackageEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Package = u.package,
                            PackageID = u.package_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public async Task<List<PackageEntity>> GetAllPackages()
        {
            var data = (from u in this.context.PACKAGE_MASTER where u.row_sta_cd == 1
                        select new PackageEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Package = u.package,
                            PackageID = u.package_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public bool RemovePackage(long PackageID, string lastupdatedby)
        {
            var data = (from u in this.context.PACKAGE_MASTER
                        where u.package_id == PackageID
                        select u).FirstOrDefault();
            if (data != null)
            {
                data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                this.context.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<PackageEntity> GetPackageByID(long packageID)
        {
            var data = (from u in this.context.PACKAGE_MASTER
                        where u.package_id == packageID
                        select new PackageEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Package = u.package,
                            PackageID = u.package_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();

            return data;
        }
    }
}
