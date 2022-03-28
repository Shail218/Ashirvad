using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.AboutUs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.AboutUs
{
    public class AboutUs : ModelAccess, IAboutUs
    {
        public async Task<long> CheckBranch(int BranchID)
        {
            long result;
            bool isExists = this.context.ABOUTUS_MASTER.Where(s => (s.branch_id == BranchID) && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }
        public async Task<ResponseModel> AboutUsMaintenance(AboutUsEntity aboutUsInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            Model.ABOUTUS_MASTER aboutUsMaster = new Model.ABOUTUS_MASTER();
            try
            {

                if (/*CheckBranch((int)aboutUsInfo.BranchInfo.BranchID).Result*/ 1 != -1)
                {
                    bool isUpdate = true;
                    var data = (from aboutus in this.context.ABOUTUS_MASTER
                                where aboutus.aboutus_id == aboutUsInfo.AboutUsID
                                select aboutus).FirstOrDefault();
                    if (data == null)
                    {
                        data = new Model.ABOUTUS_MASTER();
                        isUpdate = false;
                    }
                    else
                    {
                        aboutUsMaster = data;
                        aboutUsInfo.TransactionInfo.TransactionId = data.trans_id;
                    }


                    aboutUsMaster.header_img_name = aboutUsInfo.HeaderImageName;
                    aboutUsMaster.aboutus_desc = aboutUsInfo.AboutUsDesc;
                    aboutUsMaster.row_sta_cd = aboutUsInfo.RowStatus.RowStatusId;
                    aboutUsMaster.trans_id = this.AddTransactionData(aboutUsInfo.TransactionInfo);
                    aboutUsMaster.branch_id = aboutUsInfo.BranchInfo.BranchID;
                    aboutUsMaster.email_id = aboutUsInfo.EmailID;
                    aboutUsMaster.contact_no = aboutUsInfo.ContactNo;
                    aboutUsMaster.website = aboutUsInfo.WebsiteURL;
                    aboutUsMaster.whatsapp_no = aboutUsInfo.WhatsAppNo;
                    aboutUsMaster.contact_no = aboutUsInfo.ContactNo;
                    aboutUsMaster.header_img_path = aboutUsInfo.FilePath;
                    this.context.ABOUTUS_MASTER.Add(aboutUsMaster);
                    if (isUpdate)
                    {
                        this.context.Entry(aboutUsMaster).State = System.Data.Entity.EntityState.Modified;
                    }

                    var uniqueID = this.context.SaveChanges() > 0 ? aboutUsMaster.aboutus_id : 0;
                    if (uniqueID > 0)
                    {
                        aboutUsInfo.AboutUsID = uniqueID;
                        responseModel.Message = isUpdate == true ? "About Us Updated Successfully." : "About Us Inserted Successfully.";
                        responseModel.Status = true;
                        responseModel.Data = aboutUsInfo;
                    }
                    else
                    {
                        aboutUsInfo.AboutUsID = uniqueID;
                        responseModel.Message = isUpdate == true ? "About Us Not Updated." : "About Us Not Inserted.";
                        responseModel.Status = false;
                        responseModel.Data = aboutUsInfo;
                    }
                  
                }
            }
            catch(Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<List<AboutUsDetailEntity>> GetAllAboutUs(long branchID)
        {
            var data = (from u in this.context.ABOUTUS_DETAIL_REL
                        .Include("BRANCH_MASTER")
                        orderby u.brand_id descending
                        where (0 == branchID || u.branch_id == branchID) && u.row_sta_cd == 1
                        select new AboutUsDetailEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },

                            //HeaderImage = u.header_img,
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            TransactionInfo = new TransactionEntity() { TransactionId = u.trans_id },
                            HeaderImageText = u.header_img,
                            FilePath = "https://mastermind.org.in" + u.header_img_path,
                            BrandName = u.brand_name,
                            DetailID = u.brand_id
                        }).ToList();
            return data;
        }

        public async Task<List<AboutUsEntity>> GetAllAboutUsWithoutContent(long branchID)
        {
            var data = (from u in this.context.ABOUTUS_MASTER
                        .Include("BRANCH_MASTER")
                        orderby u.aboutus_id descending
                        where (0 == branchID || u.branch_id == branchID) && u.row_sta_cd == 1
                        select new AboutUsEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            AboutUsID = u.aboutus_id,
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            TransactionInfo = new TransactionEntity() { TransactionId = u.trans_id },
                            ContactNo = u.contact_no,
                            EmailID = u.email_id,
                            HeaderImageName = u.header_img_name,
                            WebsiteURL = u.website,
                            WhatsAppNo = u.whatsapp_no,
                            AboutUsDesc = u.aboutus_desc
                        }).ToList();

            return data;
        }

        public async Task<AboutUsEntity> GetAboutUsByUniqueID(long uniqueID, long BranchID = 0)
        {
            var data = (from u in this.context.ABOUTUS_MASTER
                        .Include("BRANCH_MASTER")
                        where u.branch_id == BranchID
                        select new AboutUsEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            AboutUsID = u.aboutus_id,
                            //HeaderImage = u.header_img,
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            TransactionInfo = new TransactionEntity() { TransactionId = u.trans_id },
                            ContactNo = u.contact_no,
                            EmailID = u.email_id,
                            HeaderImageName = u.header_img_name,
                            WebsiteURL = u.website,
                            WhatsAppNo = u.whatsapp_no,
                            AboutUsDesc = u.aboutus_desc,
                            FilePath = "https://mastermind.org.in" + u.header_img_path
                        }).FirstOrDefault();


            return data;
        }

        public ResponseModel RemoveAboutUs(long uniqueID, string lastupdatedby, bool removeAboutUsDetail)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var data = (from u in this.context.ABOUTUS_MASTER
                            where u.aboutus_id == uniqueID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    if (removeAboutUsDetail)
                    {
                        var tAboutUs = (from au in this.context.ABOUTUS_DETAIL_REL
                                        where au.branch_id == uniqueID
                                        select au).ToList();
                        if (tAboutUs?.Count > 0)
                        {
                            foreach (var item in tAboutUs)
                            {
                                item.row_sta_cd = (int)Enums.RowStatus.Inactive;
                                item.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = item.trans_id, LastUpdateBy = lastupdatedby });
                            }
                        }
                    }


                    data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                    responseModel.Message = "About Us Removed Successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.Message = "About Us Does not Exist.";
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


        #region - About Us Details -
        public async Task<ResponseModel> AboutUsDetailMaintenance(AboutUsDetailEntity aboutUsDetailInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            Model.ABOUTUS_DETAIL_REL aboutUsDetailMaster = new Model.ABOUTUS_DETAIL_REL();
            try
            {
                bool isUpdate = true;
                var data = (from aboutus in this.context.ABOUTUS_DETAIL_REL
                            where aboutus.brand_id == aboutUsDetailInfo.DetailID
                            select aboutus).FirstOrDefault();
                if (data == null)
                {
                    data = new Model.ABOUTUS_DETAIL_REL();
                    isUpdate = false;
                }
                else
                {
                    aboutUsDetailMaster = data;
                    aboutUsDetailInfo.TransactionInfo.TransactionId = data.trans_id;
                }
                aboutUsDetailMaster.branch_id = aboutUsDetailInfo.BranchInfo.BranchID;
                aboutUsDetailMaster.header_img = aboutUsDetailInfo.HeaderImageText;
                aboutUsDetailMaster.header_img_path = aboutUsDetailInfo.FilePath;
                aboutUsDetailMaster.row_sta_cd = aboutUsDetailInfo.RowStatus.RowStatusId;
                aboutUsDetailMaster.trans_id = this.AddTransactionData(aboutUsDetailInfo.TransactionInfo);
                aboutUsDetailMaster.brand_name = aboutUsDetailInfo.BrandName;
                this.context.ABOUTUS_DETAIL_REL.Add(aboutUsDetailMaster);
                if (isUpdate)
                {
                    this.context.Entry(aboutUsDetailMaster).State = System.Data.Entity.EntityState.Modified;
                }

                var uniqueID = this.context.SaveChanges() > 0 ? aboutUsDetailMaster.brand_id : 0;
                if (uniqueID > 0)
                {
                    responseModel.Message = isUpdate == true ? "About Us Detail Updated Successfully." : "About Us Detail Inserted Successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.Message = isUpdate == true ? "About Us Detail Not Updated." : "About Us Detail Not Inserted.";
                    responseModel.Status = false;
                }
            }
            catch(Exception ex)
            {
                responseModel.Message =ex.Message.ToString();
                responseModel.Status = false;
            }
           
            return responseModel;
        }

        public async Task<List<AboutUsDetailEntity>> GetAllAboutUsDetails(long aboutusID, long branchID)
        {
            var data = (from u in this.context.ABOUTUS_DETAIL_REL
                        .Include("ABOUTUS_MASTER")
                        orderby u.brand_id descending
                        join branch in this.context.BRANCH_MASTER on u.branch_id equals branch.branch_id
                        where (0 == branchID || u.branch_id == branchID)

                        select new AboutUsDetailEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            AboutUsInfo = new AboutUsEntity()
                            {

                                BranchInfo = new BranchEntity() { BranchID = branch.branch_id, BranchName = branch.branch_name },
                                TransactionInfo = new TransactionEntity() { TransactionId = u.trans_id },
                                //ContactNo = u.ABOUTUS_MASTER.contact_no,
                                //EmailID = u.ABOUTUS_MASTER.email_id,
                                //WebsiteURL = u.ABOUTUS_MASTER.website,
                                //WhatsAppNo = u.ABOUTUS_MASTER.whatsapp_no
                            },
                            TransactionInfo = new TransactionEntity() { TransactionId = u.trans_id },
                            BrandName = u.brand_name,
                            DetailID = u.brand_id,

                        }).ToList();

            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    int idx = data.IndexOf(item);
                    data[idx].HeaderImageText = data[idx].HeaderImage.Length > 0 ? Convert.ToBase64String(data[idx].HeaderImage) : "";
                }
            }

            return data;
        }

        public async Task<List<AboutUsDetailEntity>> GetAllAboutUsDetailWithoutContent(long aboutusID, long branchID)
        {
            List<AboutUsDetailEntity> data = new List<AboutUsDetailEntity>();
            //var data = (from u in this.context.ABOUTUS_DETAIL_REL
            //            .Include("ABOUTUS_MASTER")
            //            join branch in this.context.BRANCH_MASTER on u.ABOUTUS_MASTER.branch_id equals branch.branch_id
            //            where (0 == branchID || u.ABOUTUS_MASTER.branch_id == branchID)
            //            && (0 == aboutusID || u.aboutus_id == aboutusID)
            //            select new AboutUsDetailEntity()
            //            {
            //                RowStatus = new RowStatusEntity()
            //                {
            //                    RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
            //                    RowStatusId = (int)u.row_sta_cd
            //                },
            //                AboutUsInfo = new AboutUsEntity()
            //                {
            //                    AboutUsID = u.aboutus_id,
            //                    BranchInfo = new BranchEntity() { BranchID = branch.branch_id, BranchName = branch.branch_name },
            //                    TransactionInfo = new TransactionEntity() { TransactionId = u.ABOUTUS_MASTER.trans_id },
            //                    ContactNo = u.ABOUTUS_MASTER.contact_no,
            //                    EmailID = u.ABOUTUS_MASTER.email_id,
            //                    WebsiteURL = u.ABOUTUS_MASTER.website,
            //                    WhatsAppNo = u.ABOUTUS_MASTER.whatsapp_no
            //                },
            //                TransactionInfo = new TransactionEntity() { TransactionId = u.trans_id },
            //                BrandName = u.brand_name,
            //                DetailID = u.brand_id
            //            }).ToList();

            return data;
        }

        public async Task<AboutUsDetailEntity> GetAboutUsDetailByUniqueID(long uniqueID)
        {
            AboutUsDetailEntity data = new AboutUsDetailEntity();
            var detaildata = (from u in this.context.ABOUTUS_DETAIL_REL
                              where u.brand_id == uniqueID
                              select new AboutUsDetailEntity()
                              {
                                  RowStatus = new RowStatusEntity()
                                  {
                                      RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                      RowStatusId = (int)u.row_sta_cd
                                  },

                                  TransactionInfo = new TransactionEntity() { TransactionId = u.trans_id },
                                  BrandName = u.brand_name,
                                  DetailID = u.brand_id,
                                  FilePath = u.header_img_path,
                                  HeaderImageText = u.header_img

                              }).FirstOrDefault();
            return detaildata;
        }

        public ResponseModel RemoveAboutUsDetail(long uniqueID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try {
                var data = (from u in this.context.ABOUTUS_DETAIL_REL
                            where u.brand_id == uniqueID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                    responseModel.Status = true;
                    responseModel.Message = "About Us Detail Removed Successfully.";
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "About Us Detail Not Found.";
                }
            }
            catch(Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
            return responseModel;
        }

        public async Task<List<AboutUsDetailEntity>> GetAllAboutUsDetailsforExport(long aboutusID, long branchID)
        {
            var data = (from u in this.context.ABOUTUS_DETAIL_REL
                        .Include("ABOUTUS_MASTER")
                        orderby u.brand_id descending
                        join branch in this.context.BRANCH_MASTER on u.branch_id equals branch.branch_id
                        where (0 == branchID || u.branch_id == branchID)

                        select new AboutUsDetailEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            AboutUsInfo = new AboutUsEntity()
                            {

                                BranchInfo = new BranchEntity() { BranchID = branch.branch_id, BranchName = branch.branch_name },
                                TransactionInfo = new TransactionEntity() { TransactionId = u.trans_id },
                             
                            },
                            TransactionInfo = new TransactionEntity() { TransactionId = u.trans_id },
                            BrandName = u.brand_name,
                            DetailID = u.brand_id,

                        }).ToList();

            return data;
        }

        #endregion
    }
}
