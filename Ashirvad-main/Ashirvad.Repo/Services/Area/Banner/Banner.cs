﻿using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Banner;
using Ashirvad.Repo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Banner
{
    public class Banner : ModelAccess, IBannerAPI
    {
        public async Task<long> BannerMaintenance(BannerEntity bannerInfo)
        {
            Model.BANNER_MASTER bannerMaster = new Model.BANNER_MASTER();
            bool isUpdate = true;
            var data = (from banner in this.context.BANNER_MASTER
                        where banner.banner_id == bannerInfo.BannerID
                        select banner).FirstOrDefault();
            if (data == null)
            {
                data = new Model.BANNER_MASTER();
                isUpdate = false;
            }
            else
            {
                bannerMaster = data;
                bannerInfo.Transaction.TransactionId = data.trans_id;
            }

            if (bannerInfo.BannerImage?.Length > 0)
            {
                bannerMaster.banner_img = bannerInfo.BannerImage;
            }
            else if (!string.IsNullOrEmpty(bannerInfo.BannerImageText))
            {
                bannerMaster.banner_img = Convert.FromBase64String(bannerInfo.BannerImageText);
            }
            else
            {
                bannerMaster.banner_img = null;
            }


            bannerMaster.row_sta_cd = bannerInfo.RowStatus.RowStatusId;
            bannerMaster.trans_id = this.AddTransactionData(bannerInfo.Transaction);
            bannerMaster.branch_id = bannerInfo.BranchInfo != null ? (long?)bannerInfo.BranchInfo.BranchID : null;
            if (!isUpdate)
            {
                this.context.BANNER_MASTER.Add(bannerMaster);
            }


            if (this.context.SaveChanges() > 0 || bannerMaster.banner_id > 0)
            {
                var bannerID = bannerMaster.banner_id;
                var result = await this.AddBannerType(bannerInfo.BannerType, bannerID);
                return bannerID;
            }

            return 0;
        }

        public async Task<bool> AddBannerType(List<BannerTypeEntity> bannerType, long bannerID)
        {
            var associatedData = (from i in this.context.BANNER_TYPE_REL
                                  where i.banner_id == bannerID
                                  select i).ToList();
            if (associatedData?.Count > 0)
            {
                this.context.BANNER_TYPE_REL.RemoveRange(associatedData);
            }

            foreach (var item in bannerType)
            {
                BANNER_TYPE_REL banner = new BANNER_TYPE_REL()
                {
                    banner_id = bannerID,
                    sub_type_id = item.TypeID
                };
                this.context.BANNER_TYPE_REL.Add(banner);
            }

            return this.context.SaveChanges() > 0;
        }

        public async Task<List<BannerEntity>> GetAllBanner(long branchID)
        {
            var data = (from u in this.context.BANNER_MASTER.Include("BANNER_TYPE_REL")
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id into tempB
                        from branch in tempB.DefaultIfEmpty()
                        where (0 == branchID || u.branch_id == null || (u.branch_id.HasValue && u.branch_id.Value == branchID) && u.row_sta_cd == 1)
                        select new BannerEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            BannerID = u.banner_id,
                            BannerImage = u.banner_img,
                            BranchInfo = new BranchEntity() { BranchID = branch != null ? branch.branch_id : 0, BranchName = branch != null ? branch.branch_name : "All Branch" },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    int idx = data.IndexOf(item);
                    data[idx].BannerType = this.context.BANNER_TYPE_REL.Where(z => z.banner_id == item.BannerID).Select(y => new BannerTypeEntity() { ID = y.unique_id, TypeID = y.sub_type_id, TypeText = y.sub_type_id == 1 ? "Admin" : y.sub_type_id == 2 ? "Teacher" : "Student" }).ToList();
                    data[idx].BannerImageText = data[idx].BannerImage.Length > 0 ? Convert.ToBase64String(data[idx].BannerImage) : "";
                }
            }

            return data;
        }

        public async Task<List<BannerEntity>> GetAllBanner(long branchID, int bannerTypeID)
        {
            var data = (from u in this.context.BANNER_MASTER
                        join bt in this.context.BANNER_TYPE_REL on u.banner_id equals bt.banner_id
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id into tempB
                        from branch in tempB.DefaultIfEmpty()
                        where (0 == branchID ||  u.branch_id == null || u.branch_id == 0 || (u.branch_id.HasValue && u.branch_id.Value == branchID))
                        && (0 == bannerTypeID || bt.sub_type_id == bannerTypeID) && u.row_sta_cd == 1
                        select new BannerEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            BannerID = u.banner_id,
                            BannerImage = u.banner_img,
                            BranchInfo = new BranchEntity() { BranchID = branch != null ? branch.branch_id : 0, BranchName = branch != null ? branch.branch_name : "All Branch" },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).Distinct().ToList();

            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    int idx = data.IndexOf(item);
                    data[idx].BannerType = this.context.BANNER_TYPE_REL.Where(z => z.banner_id == item.BannerID).Select(y => new BannerTypeEntity() { ID = y.unique_id, TypeID = y.sub_type_id, TypeText = y.sub_type_id == 1 ? "Admin" : y.sub_type_id == 2 ? "Teacher" : "Student" }).ToList();
                    data[idx].BannerImageText = data[idx].BannerImage.Length > 0 ? Convert.ToBase64String(data[idx].BannerImage) : "";
                }
            }

            return data;
        }

        public async Task<List<BannerEntity>> GetAllBannerWithoutImage(long branchID)
        {
            var data = (from u in this.context.BANNER_MASTER.Include("BANNER_TYPE_REL")
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id into tempB
                        from branch in tempB.DefaultIfEmpty()
                        where (0 == branchID || u.branch_id == null || (u.branch_id.HasValue && u.branch_id.Value == branchID))
                        select new BannerEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            BannerID = u.banner_id,
                            //BannerImage = u.banner_img,
                            BranchInfo = new BranchEntity() { BranchID = branch != null ? branch.branch_id : 0, BranchName = branch != null ? branch.branch_name : "All Branch" },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    int idx = data.IndexOf(item);
                    data[idx].BannerType = this.context.BANNER_TYPE_REL.Where(z => z.banner_id == item.BannerID).Select(y => new BannerTypeEntity() { ID = y.unique_id, TypeID = y.sub_type_id, TypeText = y.sub_type_id == 1 ? "Admin" : y.sub_type_id == 2 ? "Teacher" : "Student" }).ToList();
                    //data[idx].BannerImageText = data[idx].BannerImage.Length > 0 ? Convert.ToBase64String(data[idx].BannerImage) : "";
                }
            }

            return data;
        }

        public async Task<BannerEntity> GetBannerByBannerID(long bannerID)
        {
            var data = (from u in this.context.BANNER_MASTER.Include("BANNER_TYPE_REL")
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id into tempB
                        from branch in tempB.DefaultIfEmpty()
                        where u.banner_id == bannerID
                        select new BannerEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            BannerID = u.banner_id,
                            BannerImage = u.banner_img,
                            BranchInfo = new BranchEntity() { BranchID = branch != null ? branch.branch_id : 0, BranchName = branch != null ? branch.branch_name : "All Branch" },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();

            if (data != null)
            {

                data.BannerType = this.context.BANNER_TYPE_REL.Where(z => z.banner_id == data.BannerID).Select(y => new BannerTypeEntity() { ID = y.unique_id, TypeID = y.sub_type_id, TypeText = y.sub_type_id == 1 ? "Admin" : y.sub_type_id == 2 ? "Teacher" : "Student" }).ToList();
                data.BannerImageText = data.BannerImage.Length > 0 ? Convert.ToBase64String(data.BannerImage) : "";
            }

            return data;
        }

        public bool RemoveBanner(long bannerID, string lastupdatedby)
        {
            var data = (from u in this.context.BANNER_MASTER
                        where u.banner_id == bannerID
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
