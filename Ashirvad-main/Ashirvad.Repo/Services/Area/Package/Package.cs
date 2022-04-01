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
        public async Task<long> CheckPackage(string name, long Id)
        {
            long result;
            bool isExists = this.context.PACKAGE_MASTER.Where(s => (Id == 0 || s.package_id != Id) && s.package == name && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<ResponseModel> PackageMaintenance(PackageEntity packageInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                Model.PACKAGE_MASTER packageMaster = new Model.PACKAGE_MASTER();
                if (CheckPackage(packageInfo.Package, packageInfo.PackageID).Result != -1)
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
                    packageMaster.student_no = packageInfo.Studentno;
                    packageMaster.branch_id = packageInfo.BranchInfo.BranchID;
                    packageMaster.row_sta_cd = packageInfo.RowStatus.RowStatusId;
                    packageMaster.trans_id = this.AddTransactionData(packageInfo.Transaction);
                    this.context.PACKAGE_MASTER.Add(packageMaster);
                    if (isUpdate)
                    {
                        this.context.Entry(packageMaster).State = System.Data.Entity.EntityState.Modified;
                    }
                    //return this.context.SaveChanges() > 0 ? packageMaster.package_id : 0;
                    var da = this.context.SaveChanges() > 0 ? packageMaster.package_id : 0;
                    if (da > 0)
                    {
                        packageInfo.PackageID = da;
                        responseModel.Data = packageInfo;
                        responseModel.Message = isUpdate == true ? "Package Updated Successfully." : "Package Inserted Successfully.";
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = isUpdate == true ? "Package Not Updated." : "Package Not Inserted.";
                        responseModel.Status = false;
                    }
                }
                else
                {
                    responseModel.Message = "Package Already Exists.";
                    responseModel.Status = false;
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
            return responseModel;

        }

        public async Task<List<PackageEntity>> GetAllPackages(long branchID)
        {
            var data = (from u in this.context.PACKAGE_MASTER
                        orderby u.package_id descending
                        where (branchID == 0 || u.branch_id == branchID) && u.row_sta_cd == 1
                        select new PackageEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Package = u.package,
                            Studentno=u.student_no,
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

        public async Task<List<PackageEntity>> GetAllCustomPackage(Common.Common.DataTableAjaxPostModel model, long branchID)
        {
            var Count = this.context.PACKAGE_MASTER.Where(a => a.branch_id == branchID && a.row_sta_cd == 1).Count();
            var data = (from u in this.context.PACKAGE_MASTER
                        orderby u.package_id descending
                        where (branchID == 0 || u.branch_id == branchID) && u.row_sta_cd == 1
                        select new PackageEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Package = u.package,
                            Studentno=u.student_no,
                            PackageID = u.package_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Count = Count,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public async Task<List<PackageEntity>> GetAllPackages()
        {
            var data = (from u in this.context.PACKAGE_MASTER
                        orderby u.package_id descending
                        where u.row_sta_cd == 1
                        select new PackageEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Package = u.package,
                            Studentno=u.student_no,
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

        public ResponseModel RemovePackage(long PackageID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {

                var data = (from u in this.context.PACKAGE_MASTER
                            where u.package_id == PackageID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                   // return true;
                    responseModel.Status = true;
                    responseModel.Message = "Package Removed Successfully.";
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "Package Not Found.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
            return responseModel;

            //return false;
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
                            Studentno=u.student_no,
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
