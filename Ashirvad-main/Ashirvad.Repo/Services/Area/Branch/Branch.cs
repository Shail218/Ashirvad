using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Branch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Ashirvad.Repo.Services.Area.Branch
{
    public class Branch : ModelAccess, IBranchAPI
    {
        public async Task<long> BranchMaintenance(BranchEntity branchInfo)
        {
            Model.BRANCH_MASTER branchMaster = new Model.BRANCH_MASTER();
            branchMaster.BRANCH_MAINT = new Model.BRANCH_MAINT();
            bool isUpdate = true;
            var data = (from branch in this.context.BRANCH_MASTER.Include("BRANCH_MAINT")
                        where branch.branch_id == branchInfo.BranchID
                        select new
                        {
                            branchMaster = branch
                        }).FirstOrDefault();
            if (data == null)
            {
                branchMaster = new Model.BRANCH_MASTER();
                branchMaster.BRANCH_MAINT = new Model.BRANCH_MAINT();
                isUpdate = false;
            }
            else
            {
                branchMaster = data.branchMaster;
                branchMaster.BRANCH_MAINT = data.branchMaster.BRANCH_MAINT;
                branchInfo.Transaction.TransactionId = data.branchMaster.trans_id;
            }

            branchMaster.about_us = branchInfo.AboutUs;
            branchMaster.branch_name = branchInfo.BranchName;
            branchMaster.contact_no = branchInfo.ContactNo;
            branchMaster.branch_type = 2;
            branchMaster.email_id = branchInfo.EmailID;
            branchMaster.mobile_no = branchInfo.MobileNo;
            branchMaster.row_sta_cd = branchInfo.RowStatus.RowStatusId;
            branchMaster.trans_id = this.AddTransactionData(branchInfo.Transaction);
            this.context.BRANCH_MASTER.Add(branchMaster);
            if (isUpdate)
            {
                this.context.Entry(branchMaster).State = System.Data.Entity.EntityState.Modified;
            }
            if (!isUpdate)
            {
                branchMaster.BRANCH_MAINT.branch_id = branchMaster.branch_id;
            }

            branchMaster.BRANCH_MAINT.branch_logo = branchInfo.BranchMaint.BranchLogo;
            branchMaster.BRANCH_MAINT.header_logo = branchInfo.BranchMaint.HeaderLogo;
            branchMaster.BRANCH_MAINT.website = branchInfo.BranchMaint.Website;
            branchMaster.BRANCH_MAINT.branch_logo_ext = branchInfo.BranchMaint.BranchLogoExt;
            branchMaster.BRANCH_MAINT.header_logo_ext = branchInfo.BranchMaint.HeaderLogoExt;
            this.context.BRANCH_MAINT.Add(branchMaster.BRANCH_MAINT);
            if (isUpdate)
            {
                this.context.Entry(branchMaster.BRANCH_MAINT).State = System.Data.Entity.EntityState.Modified;
            }
            return this.context.SaveChanges() > 0 ? branchMaster.branch_id : 0;
        }

        public async Task<List<BranchEntity>> GetAllBranch()
        {
            var data = (from u in this.context.BRANCH_MASTER.Include("BRANCH_MAINT")
                        where u.branch_type == 2
                        select new BranchEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            BranchID = u.branch_id,
                            BranchName = u.branch_name,
                            AboutUs = u.about_us,
                            EmailID = u.email_id,
                            ContactNo = u.contact_no,
                            MobileNo = u.mobile_no,
                            BranchType = u.branch_type,
                            BranchMaint = new BranchMaint()
                            {
                                BranchId = u.BRANCH_MAINT.branch_id,
                                BranchLogo = u.BRANCH_MAINT.branch_logo,
                                HeaderLogo = u.BRANCH_MAINT.header_logo,
                                Website = u.BRANCH_MAINT.website,
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public async Task<List<BranchEntity>> GetAllBranchWithoutImage()
        {
            var data = (from u in this.context.BRANCH_MASTER.Include("BRANCH_MAINT")
                        where u.branch_type == 2
                        select new BranchEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            BranchID = u.branch_id,
                            BranchName = u.branch_name,
                            AboutUs = u.about_us,
                            EmailID = u.email_id,
                            ContactNo = u.contact_no,
                            BranchType = u.branch_type,
                            MobileNo = u.mobile_no,
                            BranchMaint = new BranchMaint()
                            {
                                BranchId = u.BRANCH_MAINT.branch_id,
                                Website = u.BRANCH_MAINT.website,
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public async Task<BranchEntity> GetBranchByBranchID(long branchID)
        {
            var data = (from u in this.context.BRANCH_MASTER.Include("BRANCH_MAINT")
                        where u.branch_id == branchID
                        select new BranchEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            BranchID = u.branch_id,
                            BranchName = u.branch_name,
                            AboutUs = u.about_us,
                            EmailID = u.email_id,
                            ContactNo = u.contact_no,
                            MobileNo = u.mobile_no,
                            BranchType = u.branch_type,
                            BranchMaint = new BranchMaint()
                            {
                                BranchId = u.BRANCH_MAINT.branch_id,
                                HasImage = u.BRANCH_MAINT.branch_logo != null,
                                BranchLogo = u.BRANCH_MAINT.branch_logo,
                                //HeaderLogo = u.BRANCH_MAINT.header_logo,
                                Website = u.BRANCH_MAINT.website,
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();

            return data;
        }

        public bool RemoveBranch(long BranchID, string lastupdatedby)
        {
            var data = (from u in this.context.BRANCH_MASTER
                        where u.branch_id == BranchID
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

    }
}
