using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Link;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Link
{
    public class LinkMGMT: ModelAccess, ILinkAPI
    {
        public async Task<long> LinkMaintenance(LinkEntity linkInfo)
        {
            Model.LINK_MASTER linkMaster = new Model.LINK_MASTER();
            bool isUpdate = true;
            var data = (from link in this.context.LINK_MASTER
                        where link.unique_id == linkInfo.UniqueID
                        select link).FirstOrDefault();
            if (data == null)
            {
                data = new Model.LINK_MASTER();
                isUpdate = false;
            }
            else
            {
                linkMaster = data;
                linkInfo.Transaction.TransactionId = data.trans_id;
            }

            linkMaster.row_sta_cd = linkInfo.RowStatus.RowStatusId;
            linkMaster.trans_id = this.AddTransactionData(linkInfo.Transaction);
            linkMaster.branch_id = linkInfo.Branch.BranchID;
            linkMaster.type = linkInfo.LinkType;
            linkMaster.title = linkInfo.Title;
            linkMaster.vid_desc = linkInfo.LinkDesc;
            linkMaster.vid_url = linkInfo.LinkURL;
            linkMaster.std_id = linkInfo.StandardID;
            if (!isUpdate)
            {
                this.context.LINK_MASTER.Add(linkMaster);
            }

            var linkID = this.context.SaveChanges() > 0 ? linkMaster.unique_id : 0;
            return linkID;
        }

        public async Task<List<LinkEntity>> GetAllLink(int type, long branchID, long stdID)
        {
            var data = (from u in this.context.LINK_MASTER
                        .Include("BRANCH_MASTER")
                        join std in this.context.STD_MASTER on u.std_id equals std.std_id
                        where u.type == type
                        && (0 == branchID || u.branch_id == branchID)
                        && (0 == stdID || u.std_id == stdID) && u.row_sta_cd == 1
                        select new LinkEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            UniqueID = u.unique_id,
                            Branch = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            LinkDesc = u.vid_desc,
                            LinkURL = u.vid_url,
                            LinkType =u.type,
                            StandardID = u.std_id,
                            StandardName = std.standard,
                            Title = u.title
                        }).ToList();

            return data;
        }
        
        public async Task<LinkEntity> GetLinkByUniqueID(long uniqueID)
        {
            var data = (from u in this.context.LINK_MASTER
                        .Include("BRANCH_MASTER")
                        join std in this.context.STD_MASTER on u.std_id equals std.std_id
                        where u.unique_id == uniqueID
                        select new LinkEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            UniqueID = u.unique_id,
                            Branch = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            LinkDesc = u.vid_desc,
                            LinkURL = u.vid_url,
                            LinkType = u.type,
                            StandardID = u.std_id,

                            StandardName = std.standard,
                            Title = u.title
                        }).FirstOrDefault();

            return data;
        }

        public bool RemoveLink(long uniqueID, string lastupdatedby)
        {
            var data = (from u in this.context.LINK_MASTER
                        where u.unique_id == uniqueID
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
