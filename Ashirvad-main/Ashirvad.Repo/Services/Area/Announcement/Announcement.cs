using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Announcement
{
    public class Announcement : ModelAccess, IAnnouncementAPI
    {
        public async Task<long> AnnouncementMaintenance(AnnouncementEntity annInfo)
        {
            Model.ANNOUNCE_MASTER annMaster = new Model.ANNOUNCE_MASTER();
            bool isUpdate = true;
            var data = (from ann in this.context.ANNOUNCE_MASTER
                        where ann.announce_id == annInfo.AnnouncementID
                        select ann).FirstOrDefault();
            if (data == null)
            {
                data = new Model.ANNOUNCE_MASTER();
                isUpdate = false;
            }
            else
            {
                annMaster = data;
                annInfo.TransactionData.TransactionId = data.trans_id;
            }

            annMaster.row_sta_cd = annInfo.RowStatusData.RowStatusId;
            annMaster.trans_id = this.AddTransactionData(annInfo.TransactionData);
            annMaster.branch_id = annInfo.BranchData != null ? (long?)annInfo.BranchData.BranchID : null;
            annMaster.announce_text = annInfo.AnnouncementText;
            if (!isUpdate)
            {
                this.context.ANNOUNCE_MASTER.Add(annMaster);
            }

            var annID = this.context.SaveChanges() > 0 ? annMaster.announce_id : 0;
            return annID;
        }

        public async Task<List<AnnouncementEntity>> GetAllAnnouncement(long branchID)
        {
            try
            {
                var data = (from u in this.context.ANNOUNCE_MASTER
                            join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id into tempBranch
                            from branch in tempBranch.DefaultIfEmpty()
                            where (0 == branchID || u.branch_id == null || u.branch_id == 0 || (u.branch_id.HasValue && u.branch_id.Value == branchID)) && u.row_sta_cd == 1
                            select new AnnouncementEntity()
                            {
                                RowStatusData = new RowStatusEntity()
                                {
                                    RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                    RowStatusId = (int)u.row_sta_cd
                                },
                                AnnouncementText = u.announce_text,
                                AnnouncementID = u.announce_id,
                                BranchData = new BranchEntity() { BranchID = branch != null ? branch.branch_id : 0, BranchName = branch != null ? branch.branch_name : "All Branch" },
                                TransactionData = new TransactionEntity() { TransactionId = u.trans_id }
                            }).ToList();

                return data;
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

        public async Task<AnnouncementEntity> GetAnnouncementsByAnnouncementID(long annID)
        {
            var data = (from u in this.context.ANNOUNCE_MASTER
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id into tempBranch
                        from branch in tempBranch.DefaultIfEmpty()
                        where u.announce_id == annID
                        select new AnnouncementEntity()
                        {
                            RowStatusData = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            AnnouncementText = u.announce_text,
                            AnnouncementID = u.announce_id,
                            BranchData = new BranchEntity() { BranchID = branch != null ? branch.branch_id : 0, BranchName = branch != null ? branch.branch_name : "All Branch" },
                            TransactionData = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();
            return data;
        }

        public bool RemoveAnnouncement(long annID, string lastupdatedby)
        {
            var data = (from u in this.context.ANNOUNCE_MASTER
                        where u.announce_id == annID
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
