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

namespace Ashirvad.Repo.Services.Area.Branch
{
    public class PackageRights : ModelAccess, IPackageRightsAPI
    {

        public async Task<long> CheckRights(int RightsID, int packageID)
        {
            long result;
            bool isExists = this.context.PACKAGE_RIGHTS_MASTER.Where(s => (RightsID == 0 || s.packagerights_id != RightsID) &&s.package_id== packageID && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }
        public async Task<long> RightsMaintenance(PackageRightEntity RightsInfo)
        {
            Model.PACKAGE_RIGHTS_MASTER RightsMaster = new Model.PACKAGE_RIGHTS_MASTER();
            if (CheckRights((int)RightsInfo.PackageRightsId, (int)RightsInfo.Packageinfo.PackageID).Result != -1)
            {
                bool isUpdate = true;
                var data = (from package in this.context.PACKAGE_RIGHTS_MASTER
                            where package.packagerights_id == RightsInfo.PackageRightsId
                            select new
                            {
                                RightsMaster = package
                            }).FirstOrDefault();
                if (data == null)
                {
                    RightsMaster = new Model.PACKAGE_RIGHTS_MASTER();
                    isUpdate = false;
                }
                else
                {
                    RightsMaster = data.RightsMaster;
                    RightsInfo.Transaction.TransactionId = data.RightsMaster.trans_id;
                }

                RightsMaster.package_id = RightsInfo.Packageinfo.PackageID;
                RightsMaster.page_id = RightsInfo.PageInfo.PageID;                
                RightsMaster.row_sta_cd = RightsInfo.RowStatus.RowStatusId;
                RightsMaster.createstatus = RightsInfo.Createstatus;
                RightsMaster.viewstatus = RightsInfo.Viewstatus;
                RightsMaster.deletestatus = RightsInfo.Deletestatus;
                RightsMaster.trans_id = this.AddTransactionData(RightsInfo.Transaction);
                this.context.PACKAGE_RIGHTS_MASTER.Add(RightsMaster);
                if (isUpdate)
                {
                    this.context.Entry(RightsMaster).State = System.Data.Entity.EntityState.Modified;
                }
                var result = this.context.SaveChanges();
                if (result > 0)
                {
                    RightsInfo.PackageRightsId = RightsMaster.packagerights_id;
                    //var result2 = PackageDetailMaintenance(PackageInfo).Result;
                    return result > 0 ? RightsInfo.PackageRightsId : 0;
                }
                return this.context.SaveChanges() > 0 ? RightsInfo.PackageRightsId : 0;
            }
            return -1;
        }

        public async Task<List<PackageRightEntity>> GetAllRights()
        {
            var data = (from u in this.context.PACKAGE_RIGHTS_MASTER
                        .Include("PACKAGE_MASTER")
                        .Include("PAGE_MASTER")                        
                        where u.row_sta_cd == 1
                        select new PackageRightEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            PageInfo = new PageEntity()
                            {
                                Page = u.PAGE_MASTER.page,
                                PageID = u.page_id
                            },
                            Createstatus =u.createstatus,
                            Viewstatus = u.viewstatus,
                            Deletestatus = u.deletestatus,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },                            
                        }).ToList();

            return data;

        }

        
        public async Task<PackageRightEntity> GetRightsByRightsID(long RightsID)
        {
            var data = (from u in this.context.PACKAGE_RIGHTS_MASTER
                       .Include("PACKAGE_MASTER")
                       .Include("PAGE_MASTER")                       
                        where u.row_sta_cd == 1 && u.packagerights_id== RightsID
                        select new PackageRightEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            PageInfo = new PageEntity()
                            {
                                Page = u.PAGE_MASTER.page,
                                PageID = u.page_id
                            },
                            Createstatus = u.createstatus,
                            Viewstatus = u.viewstatus ,
                            Deletestatus = u.deletestatus,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).FirstOrDefault();
            return data;
        }

        public bool RemoveRights(long RightsID, string lastupdatedby)
        {
            var data = (from u in this.context.PACKAGE_RIGHTS_MASTER
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

      

    }
}
