using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Page
{
   public class Pages : ModelAccess, IPageAPI
    {
        public async Task<long> CheckPage(string name, long branch, long Id)
        {
            long result;
            bool isExists = this.context.PAGE_MASTER.Where(s => (Id == 0 || s.page_id != Id) && s.page == name && s.branch_id == branch && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<long> PageMaintenance(PageEntity pageInfo)
        {
            Model.PAGE_MASTER pageMaster = new Model.PAGE_MASTER();
            if (CheckPage(pageInfo.Page, pageInfo.BranchInfo.BranchID, pageInfo.PageID).Result != -1)
            {
                bool isUpdate = true;
                var data = (from page in this.context.PAGE_MASTER
                            where page.page_id == pageInfo.PageID
                            select page).FirstOrDefault();
                if (data == null)
                {
                   pageMaster = new Model.PAGE_MASTER();

                    isUpdate = false;
                }
                else
                {
                    pageMaster = data;
                    pageInfo.Transaction.TransactionId = data.trans_id;
                }

                pageMaster.page = pageInfo.Page;
                pageMaster.branch_id = pageInfo.BranchInfo.BranchID;
                pageMaster.row_sta_cd = pageInfo.RowStatus.RowStatusId;
                pageMaster.trans_id = this.AddTransactionData(pageInfo.Transaction);
                this.context.PAGE_MASTER.Add(pageMaster);
                if (isUpdate)
                {
                    this.context.Entry(pageMaster).State = System.Data.Entity.EntityState.Modified;
                }
                return this.context.SaveChanges() > 0 ? pageMaster.page_id : 0;
            }
            else
            {
                return -1;
            }

        }

        public async Task<List<PageEntity>> GetAllPages(long branchID)
        {
            var data = (from u in this.context.PAGE_MASTER
                        where u.row_sta_cd == 1/*branchID == 0 || u.branch_id == branchID && */
                        select new PageEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Page = u.page,
                            PageID = u.page_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public async Task<List<PageEntity>> GetAllPages()
        {
            var data = (from u in this.context.PAGE_MASTER
                        select new PageEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Page = u.page,
                            PageID = u.page_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public bool RemovePage(long PageID, string lastupdatedby)
        {
            var data = (from u in this.context.PAGE_MASTER
                        where u.page_id == PageID
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

        public async Task<PageEntity> GetPageByID(long pageID)
        {
            var data = (from u in this.context.PAGE_MASTER
                        where u.page_id == pageID
                        select new PageEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Page = u.page,
                            PageID = u.page_id,
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
