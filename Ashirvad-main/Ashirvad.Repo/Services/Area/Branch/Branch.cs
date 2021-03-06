using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Branch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area.Branch
{
    public class Branch : ModelAccess, IBranchAPI
    {

        public async Task<long> CheckBranch(int BranchID, string aliasName)
        {
            long result;
            bool isExists = this.context.BRANCH_MASTER.Where(s => (BranchID == 0 || s.branch_id != BranchID) && s.alias_name == aliasName && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }
        public async Task<ResponseModel> BranchMaintenance(BranchEntity branchInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            Check_Delete check = new Check_Delete();
            Model.BRANCH_MASTER branchMaster = new Model.BRANCH_MASTER();
            try
            {
                if (CheckBranch((int)branchInfo.BranchID, branchInfo.aliasName).Result != -1)
                {
                    if (branchInfo.RowStatus.RowStatusId == (int)Enums.RowStatus.Inactive)
                    {
                        var res = check.check_remove_branch(branchInfo.BranchID).Result;
                        if (res.Status)
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
                            branchMaster.alias_name = branchInfo.aliasName;
                            branchMaster.trans_id = this.AddTransactionData(branchInfo.Transaction);
                            branchMaster.board_type = null;

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
                            branchMaster.BRANCH_MAINT.app_file_name = branchInfo.BranchMaint.AppFileName;
                            branchMaster.BRANCH_MAINT.app_file_path = branchInfo.BranchMaint.AppFilePath;
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
                            var id = this.context.SaveChanges() > 0 ? branchMaster.branch_id : 0;
                            if (id > 0)
                            {
                                // branchInfo.BranchID = branchMaster.branch_id;
                                // responseModel.Data = branchInfo;
                                if (!isUpdate)
                                {
                                    // ISSUE: reference to a compiler-generated field
                                    branchInfo.BranchID = branchMaster.branch_id;
                                    // ISSUE: reference to a compiler-generated field
                                    CreateDefaultRecord(branchInfo);
                                }
                                responseModel.Message = isUpdate == true ? "Branch Updated Successfully." : "Branch Inserted Successfully.";
                                responseModel.Status = true;
                            }
                            else
                            {
                                responseModel.Message = isUpdate == true ? "Branch Not Updated." : "Branch Not Inserted.";
                                responseModel.Status = false;
                            }
                        }
                        else
                        {
                            responseModel.Status = res.Status;
                            responseModel.Message = res.Message;
                        }
                    }
                    else
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
                        branchMaster.alias_name = branchInfo.aliasName;
                        branchMaster.trans_id = this.AddTransactionData(branchInfo.Transaction);
                        branchMaster.board_type = null;

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
                        branchMaster.BRANCH_MAINT.app_file_name = branchInfo.BranchMaint.AppFileName;
                        branchMaster.BRANCH_MAINT.app_file_path = branchInfo.BranchMaint.AppFilePath;
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
                        var id = this.context.SaveChanges() > 0 ? branchMaster.branch_id : 0;
                        if (id > 0)
                        {
                            // branchInfo.BranchID = branchMaster.branch_id;
                            // responseModel.Data = branchInfo;
                            responseModel.Message = isUpdate == true ? "Branch Updated Successfully." : "Branch Inserted Successfully.";
                            responseModel.Status = true;
                        }
                        else
                        {
                            responseModel.Message = isUpdate == true ? "Branch Not Updated." : "Branch Not Inserted.";
                            responseModel.Status = false;
                        }
                    }
                   
                }
            }catch(Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
          
            return responseModel;
        }

        public async Task<List<BranchEntity>> GetAllBranch()
        {
            var data = (from u in this.context.BRANCH_MASTER.Include("BRANCH_MAINT") orderby u.branch_id descending
                        where u.branch_type == 2 && u.row_sta_cd == 1
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
                            aliasName = u.alias_name,
                            BranchMaint = new BranchMaint()
                            {
                                BranchId = u.BRANCH_MAINT.branch_id,
                                Website = u.BRANCH_MAINT.website,
                                FileName = u.BRANCH_MAINT.file_name,
                                FilePath = "https://mastermind.org.in" + u.BRANCH_MAINT.file_path,
                                AppFileName = u.BRANCH_MAINT.app_file_name,
                                AppFilePath = "https://mastermind.org.in" + u.BRANCH_MAINT.app_file_path
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }

                        }).Take(15).ToList();

            return data;
        }

        public async Task<List<BranchEntity>> GetAllCustomBranch(DataTableAjaxPostModel model)
        {
            var Result = new List<BranchEntity>();
            bool Isasc = model.order[0].dir == "desc" ? false : true;
            long count = this.context.BRANCH_MASTER.Where(s => s.row_sta_cd == 1).Distinct().Count();
            var data = (from u in this.context.BRANCH_MASTER.Include("BRANCH_MAINT") orderby u.branch_id descending
                        where u.branch_type == 2                         
                        && (model.search.value == null
                        || model.search.value == ""
                        || u.branch_name.ToLower().Contains(model.search.value))
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
                            aliasName = u.alias_name,
                            Count =count,
                            BranchMaint = new BranchMaint()
                            {
                                BranchId = u.BRANCH_MAINT.branch_id,
                                Website = u.BRANCH_MAINT.website,
                                FileName = u.BRANCH_MAINT.file_name,
                                FilePath = "https://mastermind.org.in" + u.BRANCH_MAINT.file_path,
                                AppFileName = u.BRANCH_MAINT.app_file_name,
                                AppFilePath = "https://mastermind.org.in" + u.BRANCH_MAINT.app_file_path
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }

                        })
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();

            return data;
        }
        
        public async Task<List<BranchEntity>> GetAllBranchWithoutImage()
        {
            var data = (from u in this.context.BRANCH_MASTER.Include("BRANCH_MAINT")
                        orderby u.branch_id descending
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
                            aliasName = u.alias_name,
                            BranchMaint = new BranchMaint()
                            {
                                BranchId = u.BRANCH_MAINT.branch_id,
                                Website = u.BRANCH_MAINT.website,
                                FileName = u.BRANCH_MAINT.file_name,
                                FilePath = "https://mastermind.org.in" + u.BRANCH_MAINT.file_path,  
                                AppFileName = u.BRANCH_MAINT.app_file_name,
                                AppFilePath = "https://mastermind.org.in" + u.BRANCH_MAINT.app_file_path
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
                            aliasName = u.alias_name,
                            BranchMaint = new BranchMaint()
                            {
                                BranchId = u.BRANCH_MAINT.branch_id,
                                FilePath = u.BRANCH_MAINT.file_path,
                                FileName = u.BRANCH_MAINT.file_name,
                                //HasImage = u.BRANCH_MAINT.branch_logo != null,
                                //BranchLogo = u.BRANCH_MAINT.branch_logo,
                                //HeaderLogo = u.BRANCH_MAINT.header_logo,
                                AppFileName = u.BRANCH_MAINT.app_file_name,
                                AppFilePath =  u.BRANCH_MAINT.app_file_path,
                                Website = u.BRANCH_MAINT.website,
                            },
                            board = u.board_type == 1 ? Enums.BoardType.GujaratBoard : u.board_type == 2 ? Enums.BoardType.CBSC : Enums.BoardType.Both,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();

            return data;
        }

        public ResponseModel RemoveBranch(long BranchID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var data = (from u in this.context.BRANCH_MASTER
                            where u.branch_id == BranchID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();

                    responseModel.Message = "Branch Removed Successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.Message = "Branch Not Found";
                    responseModel.Status = false;
                }


            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            return responseModel;
           
        }

        public async Task<long> Checkagreement(long BranchID, long agreementid)
        {
            long result;
            bool isExists = this.context.BRANCH_AGREEMENT.Where(s => (agreementid == 0 || s.agreement_id != agreementid) && s.branch_id == BranchID && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<ResponseModel> AgreementMaintenance(BranchAgreementEntity agrInfo)
        {
            Model.BRANCH_AGREEMENT agrMaster = new Model.BRANCH_AGREEMENT();
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if (Checkagreement(agrInfo.BranchData.BranchID, agrInfo.AgreementID).Result != -1)
                {
                    bool isUpdate = true;
                    var data = (from upi in this.context.BRANCH_AGREEMENT
                                where upi.agreement_id == agrInfo.AgreementID
                                select upi).FirstOrDefault();
                    if (data == null)
                    {
                        data = new Model.BRANCH_AGREEMENT();
                        isUpdate = false;
                        agrMaster.serial_key = Common.Common.RandomString(10);
                    }
                    else
                    {
                        agrMaster = data;
                        if (data.serial_key == null)
                        {
                            agrMaster.serial_key = Common.Common.RandomString(10);
                        }
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
                    if (agrID >= 0)
                    {
                        agrInfo.AgreementID = agrMaster.agreement_id;
                        responseModel.Data = agrInfo;
                        responseModel.Message = isUpdate==true? "Agreement Updated Successfully." : "Agreement Inserted Successfully.";
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = isUpdate ==true? "Agreement Not Updated" : "Agreement Not Inserted";
                        responseModel.Status = false;
                    }
                }
                else
                {
                    responseModel.Message = "Agreement Already Exist.";
                    responseModel.Status = false;
                }
            }
            catch(Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            return responseModel;
            
            
        }

        public async Task<List<BranchAgreementEntity>> GetAllAgreements(long branchID)
        {
            var data = (from u in this.context.BRANCH_AGREEMENT
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id
                        orderby u.agreement_id descending
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
                            SerialKey = u.serial_key,
                            BranchData = new BranchEntity() { BranchID = b.branch_id, BranchName = b.branch_name },
                            TranscationData = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public async Task<List<BranchAgreementEntity>> GetAllCustomAgreement(DataTableAjaxPostModel model, long branchID)
        {
            var Result = new List<BranchAgreementEntity>();
            bool Isasc = model.order[0].dir == "desc" ? false : true;
            long count = (from u in this.context.BRANCH_AGREEMENT
                          join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id
                          orderby u.agreement_id descending
                          where (0 == branchID || u.branch_id == branchID)
                          select new BranchAgreementEntity() { 
                              AgreementID = u.agreement_id
                          }).Distinct().Count();
            var data = (from u in this.context.BRANCH_AGREEMENT
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id
                        orderby u.agreement_id descending
                        where (0 == branchID || u.branch_id == branchID)
                      && (model.search.value == null
                        || model.search.value == ""
                        || u.amount.ToString().ToLower().Contains(model.search.value)
                        || u.from_dt.ToString().ToLower().Contains(model.search.value)
                        || u.to_dt.ToString().ToLower().Contains(model.search.value)
                        || b.branch_name.ToString().ToLower().Contains(model.search.value))
                        orderby u.agreement_id descending
                        select new BranchAgreementEntity()
                        {
                            RowStatusData = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            AgreementFromDate = u.from_dt,
                            AgreementToDate = u.to_dt,
                            AgreementID = u.agreement_id,
                            Amount = u.amount,
                            SerialKey = u.serial_key,
                            Count = count,
                            BranchData = new BranchEntity() { BranchID = b.branch_id, BranchName = b.branch_name },
                            TranscationData = new TransactionEntity() { TransactionId = u.trans_id }
                        })
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();
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
                            SerialKey = u.serial_key,
                            BranchData = new BranchEntity() { BranchID = b.branch_id, BranchName = b.branch_name },
                            TranscationData = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();
            return data;
        }

        public ResponseModel RemoveAgreement(long agreementID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var data = (from u in this.context.BRANCH_AGREEMENT
                            where u.agreement_id == agreementID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                    responseModel.Message = "Agreement Removed Successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.Message = "Agreement Not Found";
                    responseModel.Status = false;
                }
            }
            catch(Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            return responseModel;
           
        }

        public async Task<bool> CreateDefaultRecord(BranchEntity entity)
        {
            entity.Transaction.TransactionId = 0L;
            AnnouncementEntity announcement = new AnnouncementEntity()
            {
                AnnouncementText = "Welcome! To " + entity.BranchName + ".",
                BranchData = new BranchEntity()
                {
                    BranchID = entity.BranchID
                },
                RowStatusData = new RowStatusEntity()
                {
                    RowStatusId = 1
                },
                TransactionData = entity.Transaction
            };
            BannerTypeEntity bannerTypeEntity1 = new BannerTypeEntity()
            {
                TypeID = 1
            };
            List<BannerTypeEntity> bannerTypeEntityList = new List<BannerTypeEntity>();
            bannerTypeEntityList.Add(bannerTypeEntity1);
            BannerTypeEntity bannerTypeEntity2 = new BannerTypeEntity()
            {
                TypeID = 2
            };
            bannerTypeEntityList.Add(bannerTypeEntity2);
            BannerTypeEntity bannerTypeEntity3 = new BannerTypeEntity()
            {
                TypeID = 3
            };
            bannerTypeEntityList.Add(bannerTypeEntity3);
            BannerEntity banner = new BannerEntity()
            {
                BranchInfo = new BranchEntity()
                {
                    BranchID = entity.BranchID
                },
                RowStatus = new RowStatusEntity()
                {
                    RowStatusId = 1
                },
                BannerType = bannerTypeEntityList,
                Transaction = entity.Transaction,
                BannerImageText = entity.BranchMaint.FilePath,
                FileName = entity.BranchMaint.FileName,
                FilePath = entity.BranchMaint.FilePath
            };
            AboutUsEntity aboutUsInfo = new AboutUsEntity()
            {
                BranchInfo = new BranchEntity()
                {
                    BranchID = entity.BranchID
                },
                RowStatus = new RowStatusEntity()
                {
                    RowStatusId = 1
                },
                TransactionInfo = entity.Transaction,
                AboutUsDesc = "Vision\r\nSomerville School aims to be an institution of excellence where sound ethical values, character, wisdom, leadership, service and academic achievement are encouraged in a safe and nurturing environment.\r\nMission\r\nThe Mission of the school is to provide an environment that lays emphasis on intellectual and emotional growth in which every student discovers and realizes his/her full potential and highest academic standards are achieved;\r\nWhere emphasis is laid on integrity, ethics, moral courage, hard work, responsibility and self-discipline;\r\nWhere activities focus on developing visionary, articulate and confident individuals with sensitivity and concern for the less fortunate, the environment, diversity and communal harmony, human rights and democratic values;\r\nWhere students, teachers, parents and the administration work together as a nurturing community.\r\n",
                EmailID = "test@gmail.com",
                ContactNo = "0000000000",
                WhatsAppNo = "1111111111",
                HeaderImageName = entity.BranchMaint.FileName,
                FilePath = entity.BranchMaint.FilePath
            };
            AboutUsDetailEntity detailEntity = new AboutUsDetailEntity()
            {
                BrandName = "Science Teacher A science teacher provides instruction and guidance to help students explore and understand important concepts in science, including problem-solving and how to gather evidence to support ideas or decisions. Science teachers create lesson plans, present science demonstrations, and grade tests and assignments.",
                HeaderImageText = entity.BranchMaint.FileName,
                BranchInfo = new BranchEntity()
                {
                    BranchID = entity.BranchID
                },
                FilePath = entity.BranchMaint.FilePath,
                RowStatus = new RowStatusEntity()
                {
                    RowStatusId = 1
                },
                TransactionInfo = entity.Transaction
            };
            Ashirvad.Repo.Services.Area.Announcement.Announcement c = new Ashirvad.Repo.Services.Area.Announcement.Announcement();
            Ashirvad.Repo.Services.Area.Banner.Banner b = new Ashirvad.Repo.Services.Area.Banner.Banner();
            Ashirvad.Repo.Services.Area.AboutUs.AboutUs w = new Ashirvad.Repo.Services.Area.AboutUs.AboutUs();
            ResponseModel responseModel1 = await w.AboutUsMaintenance(aboutUsInfo);
            ResponseModel responseModel2 = await w.AboutUsDetailMaintenance(detailEntity);
            ResponseModel responseModel3 = await c.AnnouncementMaintenance(announcement);
            ResponseModel responseModel4 = await b.BannerMaintenance(banner);
            bool defaultRecord = true;
            announcement = (AnnouncementEntity)null;
            banner = (BannerEntity)null;
            detailEntity = (AboutUsDetailEntity)null;
            c = (Ashirvad.Repo.Services.Area.Announcement.Announcement)null;
            b = (Ashirvad.Repo.Services.Area.Banner.Banner)null;
            w = (Ashirvad.Repo.Services.Area.AboutUs.AboutUs)null;
            return defaultRecord;
        }

    }
}
