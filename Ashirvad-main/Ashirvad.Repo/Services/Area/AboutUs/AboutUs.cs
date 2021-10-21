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
            bool isExists = this.context.ABOUTUS_MASTER.Where(s => (BranchID == 0 || s.branch_id != BranchID) && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }
        public async Task<long> AboutUsMaintenance(AboutUsEntity aboutUsInfo)
        {
            Model.ABOUTUS_MASTER aboutUsMaster = new Model.ABOUTUS_MASTER();
            if (CheckBranch((int)aboutUsInfo.BranchInfo.BranchID).Result != -1)
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
                this.context.ABOUTUS_MASTER.Add(aboutUsMaster);
                if (isUpdate)
                {
                    this.context.Entry(aboutUsMaster).State = System.Data.Entity.EntityState.Modified;
                }

                var uniqueID = this.context.SaveChanges() > 0 ? aboutUsMaster.aboutus_id : 0;
                return uniqueID;
            }
            return -1;
        }

        public async Task<List<AboutUsEntity>> GetAllAboutUs(long branchID)
        {
            var data = (from u in this.context.ABOUTUS_MASTER
                        .Include("BRANCH_MASTER")
                        join Detail in this.context.ABOUTUS_DETAIL_REL on u.branch_id equals Detail.branch_id
                        where (0 == branchID || u.branch_id == branchID) && u.row_sta_cd == 1
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
                            detailEntity = new AboutUsDetailEntity()
                            {
                                DetailID = Detail.brand_id,
                                BrandName = Detail.brand_name
                                
                            },
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

        public async Task<List<AboutUsEntity>> GetAllAboutUsWithoutContent(long branchID)
        {
            var data = (from u in this.context.ABOUTUS_MASTER
                        .Include("BRANCH_MASTER")
                        where (0 == branchID || u.branch_id == branchID)
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
                            WhatsAppNo = u.whatsapp_no
                        }).ToList();

            return data;
        }

        public async Task<AboutUsEntity> GetAboutUsByUniqueID(long uniqueID)
        {
            var data = (from u in this.context.ABOUTUS_MASTER
                        .Include("BRANCH_MASTER")
                        where u.aboutus_id == uniqueID
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
                            WhatsAppNo = u.whatsapp_no
                        }).FirstOrDefault();
            if (data != null)
            {
                data.HeaderImageText = data.HeaderImage.Length > 0 ? Convert.ToBase64String(data.HeaderImage) : "";
            }

            return data;
        }

        public bool RemoveAboutUs(long uniqueID, string lastupdatedby, bool removeAboutUsDetail)
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
                return true;
            }

            return false;
        }


        #region - About Us Details -
        public async Task<long> AboutUsDetailMaintenance(AboutUsDetailEntity aboutUsDetailInfo)
        {
            Model.ABOUTUS_DETAIL_REL aboutUsDetailMaster = new Model.ABOUTUS_DETAIL_REL();
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
            return uniqueID;
        }

        public async Task<List<AboutUsDetailEntity>> GetAllAboutUsDetails(long aboutusID, long branchID)
        {
            var data = (from u in this.context.ABOUTUS_DETAIL_REL
                        .Include("ABOUTUS_MASTER")
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
            //var data = (from u in this.context.ABOUTUS_DETAIL_REL
            //            .Include("ABOUTUS_MASTER")
            //            join branch in this.context.BRANCH_MASTER on u.ABOUTUS_MASTER.branch_id equals branch.branch_id
            //            where u.brand_id == uniqueID
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
            //                DetailID = u.brand_id,
            //                HeaderImage = u.header_img
            //            }).FirstOrDefault();

            //if (data != null)
            //{
            //    data.HeaderImageText = data.HeaderImage.Length > 0 ? Convert.ToBase64String(data.HeaderImage) : "";
            //}

            return data;
        }

        public bool RemoveAboutUsDetail(long uniqueID, string lastupdatedby)
        {
            var data = (from u in this.context.ABOUTUS_DETAIL_REL
                        where u.brand_id == uniqueID
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
        #endregion
    }
}
