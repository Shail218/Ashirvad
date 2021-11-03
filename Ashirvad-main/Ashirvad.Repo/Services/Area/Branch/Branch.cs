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

        public async Task<long> CheckBranch(int BranchID, string Branchname)
        {
            long result;
            bool isExists = this.context.BRANCH_MASTER.Where(s => (BranchID == 0 || s.branch_id != BranchID) && s.branch_name == Branchname && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }
        public async Task<long> BranchMaintenance(BranchEntity branchInfo)
        {
            Model.BRANCH_MASTER branchMaster = new Model.BRANCH_MASTER();
            if (CheckBranch((int)branchInfo.BranchID, branchInfo.BranchName).Result != -1)
            {
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
                branchMaster.board_type = (int)branchInfo.board;

                this.context.BRANCH_MASTER.Add(branchMaster);
                if (isUpdate)
                {
                    this.context.Entry(branchMaster).State = System.Data.Entity.EntityState.Modified;
                }
                if (!isUpdate)
                {
                    branchMaster.BRANCH_MAINT.branch_id = branchMaster.branch_id;
                }
                branchMaster.BRANCH_MAINT.file_name = branchInfo.BranchMaint.FileName;
                branchMaster.BRANCH_MAINT.file_path = branchInfo.BranchMaint.FilePath;
                branchMaster.BRANCH_MAINT.branch_logo = null;
                branchMaster.BRANCH_MAINT.header_logo = null;
                branchMaster.BRANCH_MAINT.website = branchInfo.BranchMaint.Website;
                branchMaster.BRANCH_MAINT.branch_logo_ext = null;
                branchMaster.BRANCH_MAINT.header_logo_ext = null;
                this.context.BRANCH_MAINT.Add(branchMaster.BRANCH_MAINT);
                if (isUpdate)
                {
                    this.context.Entry(branchMaster.BRANCH_MAINT).State = System.Data.Entity.EntityState.Modified;
                }
                return this.context.SaveChanges() > 0 ? branchMaster.branch_id : 0;
            }
            return -1;
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
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            board = u.board_type == 1 ? Enums.BoardType.GujaratBoard : u.board_type == 2 ? Enums.BoardType.CBSC : Enums.BoardType.Both

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
                                FileName = u.BRANCH_MAINT.file_name,
                                FilePath = u.BRANCH_MAINT.file_path
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            board = u.board_type == 1 ? Enums.BoardType.GujaratBoard : u.board_type == 2 ? Enums.BoardType.CBSC : Enums.BoardType.Both

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
                                FilePath = u.BRANCH_MAINT.file_path,
                                FileName = u.BRANCH_MAINT.file_name,
                                //HasImage = u.BRANCH_MAINT.branch_logo != null,
                                //BranchLogo = u.BRANCH_MAINT.branch_logo,
                                //HeaderLogo = u.BRANCH_MAINT.header_logo,
                                Website = u.BRANCH_MAINT.website,
                            },
                            board = u.board_type == 1 ? Enums.BoardType.GujaratBoard : u.board_type == 2 ? Enums.BoardType.CBSC : Enums.BoardType.Both,
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

        public async Task<long> AgreementMaintenance(BranchAgreementEntity agrInfo)
        {
            Model.BRANCH_AGREEMENT agrMaster = new Model.BRANCH_AGREEMENT();
            bool isUpdate = true;
            var data = (from upi in this.context.BRANCH_AGREEMENT
                        where upi.agreement_id == agrInfo.AgreementID
                        select upi).FirstOrDefault();
            if (data == null)
            {
                data = new Model.BRANCH_AGREEMENT();
                isUpdate = false;
            }
            else
            {
                agrMaster = data;
                agrInfo.TranscationData.TransactionId = data.trans_id;
            }

            agrMaster.row_sta_cd = agrInfo.RowStatusData.RowStatusId;
            agrMaster.trans_id = this.AddTransactionData(agrInfo.TranscationData);
            agrMaster.branch_id = agrInfo.BranchData.BranchID;
            agrMaster.from_dt = agrInfo.AgreementFromDate;
            agrMaster.amount = agrInfo.Amount;
            agrMaster.to_dt = agrInfo.AgreementToDate;
            if (!isUpdate)
            {
                this.context.BRANCH_AGREEMENT.Add(agrMaster);
            }

            var agrID = this.context.SaveChanges() > 0 ? agrMaster.agreement_id : 0;
            return agrID;
        }

        public async Task<List<BranchAgreementEntity>> GetAllAgreements(long branchID)
        {
            var data = (from u in this.context.BRANCH_AGREEMENT
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id
                        where (0 == branchID || u.branch_id == branchID) //&& u.row_sta_cd == 1
                        select new BranchAgreementEntity()
                        {
                            RowStatusData = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            AgreementFromDate = u.from_dt,
                            AgreementToDate = u.to_dt,
                            AgreementID = u.agreement_id,
                            Amount = u.amount,
                            BranchData = new BranchEntity() { BranchID = b.branch_id, BranchName = b.branch_name },
                            TranscationData = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public async Task<BranchAgreementEntity> GetAgreementByID(long agreementID)
        {
            var data = (from u in this.context.BRANCH_AGREEMENT
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id
                        where u.agreement_id == agreementID
                        select new BranchAgreementEntity()
                        {
                            RowStatusData = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            AgreementFromDate = u.from_dt,
                            AgreementToDate = u.to_dt,
                            AgreementID = u.agreement_id,
                            Amount = u.amount,
                            BranchData = new BranchEntity() { BranchID = b.branch_id, BranchName = b.branch_name },
                            TranscationData = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();
            return data;
        }

        public bool RemoveAgreement(long agreementID, string lastupdatedby)
        {
            var data = (from u in this.context.BRANCH_AGREEMENT
                        where u.agreement_id == agreementID
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
