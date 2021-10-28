using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.Repo.DataAcceessAPI.Area.Branch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Ashirvad.Repo.Services.Area
{
    public class BranchRights : ModelAccess, IBranchRightsAPI
    {

        public async Task<long> CheckRights(int RightsID, int BranchID)
        {
            long result;
            bool isExists = this.context.BRANCH_RIGHTS_MASTER.Where(s => (RightsID == 0 || s.branchrights_id != RightsID) &&s.packagerights_id== BranchID && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }
        public async Task<long> RightsMaintenance(BranchWiseRightEntity RightsInfo)
        {
            Model.BRANCH_RIGHTS_MASTER RightsMaster = new Model.BRANCH_RIGHTS_MASTER();
            if (CheckRights((int)RightsInfo.BranchWiseRightsID, (int)RightsInfo.Packageinfo.PackageRightsId).Result != -1)
            {
                bool isUpdate = true;
                var data = (from Branch in this.context.BRANCH_RIGHTS_MASTER
                            where Branch.branchrights_id == RightsInfo.BranchWiseRightsID
                            select new
                            {
                                RightsMaster = Branch
                            }).FirstOrDefault();
                if (data == null)
                {
                    RightsMaster = new Model.BRANCH_RIGHTS_MASTER();
                    isUpdate = false;
                }
                else
                {
                    RightsMaster = data.RightsMaster;
                    RightsInfo.Transaction.TransactionId = data.RightsMaster.trans_id;
                }

                RightsMaster.branchrights_id = RightsInfo.BranchWiseRightsID;
                RightsMaster.packagerights_id = RightsInfo.Packageinfo.PackageRightsId;                
                RightsMaster.row_sta_cd = RightsInfo.RowStatus.RowStatusId;
                
                RightsMaster.trans_id = this.AddTransactionData(RightsInfo.Transaction);
                this.context.BRANCH_RIGHTS_MASTER.Add(RightsMaster);
                if (isUpdate)
                {
                    this.context.Entry(RightsMaster).State = System.Data.Entity.EntityState.Modified;
                }
                var result = this.context.SaveChanges();
                if (result > 0)
                {
                    RightsInfo.BranchWiseRightsID = RightsMaster.branchrights_id;
                    //var result2 = BranchDetailMaintenance(BranchInfo).Result;
                    return result > 0 ? RightsInfo.BranchWiseRightsID : 0;
                }
                return this.context.SaveChanges() > 0 ? RightsInfo.BranchWiseRightsID : 0;
            }
            return -1;
        }

        public async Task<List<BranchWiseRightEntity>> GetAllRights()
        {
            var data = (from u in this.context.BRANCH_RIGHTS_MASTER                        
                        .Include("PACKAGE_RIGHTS_MASTER")                        
                        where u.row_sta_cd == 1
                        select new BranchWiseRightEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Packageinfo = new PackageRightEntity()
                            {
                                PageInfo = new PageEntity()
                                {
                                    Page = u.PACKAGE_RIGHTS_MASTER.PAGE_MASTER.page,
                                    PageID = u.PACKAGE_RIGHTS_MASTER.PAGE_MASTER.page_id,
                                },
                                PackageRightsId = u.packagerights_id,
                                Createstatus = u.PACKAGE_RIGHTS_MASTER.createstatus,
                                Viewstatus = u.PACKAGE_RIGHTS_MASTER.viewstatus,
                                Deletestatus = u.PACKAGE_RIGHTS_MASTER.deletestatus,

                            },
                           
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },                            
                        }).ToList();

            return data;

        }

        
        public async Task<BranchWiseRightEntity> GetRightsByRightsID(long RightsID)
        {
            var data = (from u in this.context.BRANCH_RIGHTS_MASTER
                         .Include("PACKAGE_RIGHTS_MASTER")
                        where u.row_sta_cd == 1 && u.packagerights_id== RightsID
                        select new BranchWiseRightEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Packageinfo = new PackageRightEntity()
                            {
                                PageInfo = new PageEntity()
                                {
                                    Page = u.PACKAGE_RIGHTS_MASTER.PAGE_MASTER.page,
                                    PageID = u.PACKAGE_RIGHTS_MASTER.PAGE_MASTER.page_id,
                                },
                                PackageRightsId = u.packagerights_id,
                                Createstatus = u.PACKAGE_RIGHTS_MASTER.createstatus,
                                Viewstatus = u.PACKAGE_RIGHTS_MASTER.viewstatus,
                                Deletestatus = u.PACKAGE_RIGHTS_MASTER.deletestatus,

                            },

                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).FirstOrDefault();

            return data;
        }

        public bool RemoveRights(long RightsID, string lastupdatedby)
        {
            var data = (from u in this.context.BRANCH_RIGHTS_MASTER
                        where u.packagerights_id == RightsID
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

        public Task<List<BranchWiseRightEntity>> GetAllRightsUniqData(long PackageRightID)
        {
            throw new NotImplementedException();
        }
    }
}
