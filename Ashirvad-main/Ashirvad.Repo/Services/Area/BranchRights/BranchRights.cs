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
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area
{
    public class BranchRights : ModelAccess, IBranchRightsAPI
    {

        public async Task<long> CheckRights(int RightsID, int PackageID)
        {
            long result;
            bool isExists = this.context.BRANCH_RIGHTS_MASTER.Where(s => (RightsID == 0 || s.branchrights_id != RightsID) && s.branch_id == PackageID && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }
       
        public async Task<ResponseModel> RightsMaintenance(BranchWiseRightEntity RightsInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                Model.BRANCH_RIGHTS_MASTER RightsMaster = new Model.BRANCH_RIGHTS_MASTER();
                if (CheckRights((int)RightsInfo.BranchWiseRightsID, (int)RightsInfo.BranchID).Result != -1)
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
                    RightsMaster.package_id = RightsInfo.Packageinfo.PackageID;
                    RightsMaster.row_sta_cd = RightsInfo.RowStatus.RowStatusId;
                    RightsMaster.branch_id = RightsInfo.BranchID;

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
                        //return result > 0 ? RightsInfo.BranchWiseRightsID : 0;
                        responseModel.Data = RightsInfo;
                        responseModel.Message = isUpdate == true ? "Branch Rights Updated Successfully." : "Branch Rights Inserted Successfully.";
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = isUpdate==true?"Branch Rights Not Updated.":"Branch Rights Not Inserted.";
                        responseModel.Status = false;
                    }
                    //  return this.context.SaveChanges() > 0 ? RightsInfo.BranchWiseRightsID : 0;
                }
                else
                {
                    responseModel.Message ="Branch Rights Already Exists.";
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

        public async Task<List<BranchWiseRightEntity>> GetAllRights()
        {
            var data = (from u in this.context.BRANCH_RIGHTS_MASTER
                                .Include("BRANCH_MASTER")
                       where u.row_sta_cd == 1
                       select new BranchWiseRightEntity()
                       {
                           branchinfo = new BranchEntity()
                           {
                               BranchName = u.BRANCH_MASTER.branch_name,
                               BranchID=u.branch_id

                           },
                           Packageinfo = new PackageEntity()
                           {
                               Package = u.PACKAGE_MASTER.package
                           },
                           BranchWiseRightsID = u.branchrights_id,

                       }).Distinct().OrderByDescending(a => a.BranchWiseRightsID).ToList();

            if (data?.Count > 0)
            {
                
                foreach (var item in data)
                {
                      item.list = new List<BranchWiseRightEntity>();
                      item.list = (from u in this.context.BRANCH_RIGHTS_MASTER
                     .Include("PACKAGE_MASTER")
                      .Include("BRANCH_MASTER")
                     join PM in this.context.PACKAGE_RIGHTS_MASTER on u.package_id equals PM.package_id
                     join page in this.context.PAGE_MASTER on PM.page_id equals page.page_id
                     where u.row_sta_cd == 1 && u.branch_id == item.branchinfo.BranchID
                                select new BranchWiseRightEntity()
                                {

                                    PageInfo = new PageEntity()
                                    {
                                        Page = page.page,
                                        PageID = page.page_id,
                                    },
                                    branchinfo = new BranchEntity()
                                    {
                                        BranchName = page.BRANCH_MASTER.branch_name,
                                        BranchID = page.branch_id,
                                    },
                                    Packageinfo = new PackageEntity()
                                    {
                                        Package = u.PACKAGE_MASTER.package,
                                        PackageID = u.package_id,
                                    },
                                    Createstatus = PM.createstatus,
                                    Viewstatus = PM.viewstatus,
                                    Deletestatus = PM.deletestatus,
                                    BranchWiseRightsID = u.branchrights_id,
                                    Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                                }).ToList();

                }

            }
            return data;
        }

        public async Task<List<BranchWiseRightEntity>> GetAllCustomRights(DataTableAjaxPostModel model)
        {
            bool Isasc = model.order[0].dir == "desc" ? false : true;
            long count = (from u in this.context.BRANCH_RIGHTS_MASTER
                                .Include("BRANCH_MASTER") orderby u.branchrights_id descending
                          where u.row_sta_cd == 1
                          select new BranchWiseRightEntity()
                          {
                              branchinfo = new BranchEntity()
                              {
                                  BranchName = u.BRANCH_MASTER.branch_name,
                                  BranchID = u.branch_id
                              },
                              Packageinfo = new PackageEntity()
                              {
                                  Package = u.PACKAGE_MASTER.package
                              },
                              BranchWiseRightsID = u.branchrights_id
                          }).Distinct().Count();
            var data = (from u in this.context.BRANCH_RIGHTS_MASTER
                                .Include("BRANCH_MASTER")
                        orderby u.branchrights_id descending
                        where u.row_sta_cd == 1 && (model.search.value == null
                        || model.search.value == ""
                        || u.PACKAGE_MASTER.package.ToLower().Contains(model.search.value)
                        || u.BRANCH_MASTER.branch_name.ToLower().Contains(model.search.value))
                        select new BranchWiseRightEntity()
                        {
                            branchinfo = new BranchEntity()
                            {
                                BranchName = u.BRANCH_MASTER.branch_name,
                                BranchID = u.branch_id
                            },
                            Packageinfo = new PackageEntity()
                            {
                                Package = u.PACKAGE_MASTER.package
                            },
                            BranchWiseRightsID = u.branchrights_id,
                            Count = count
                        }).Distinct()
                        .OrderByDescending(a => a.BranchWiseRightsID)
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();
            foreach (var item in data)
            {
                item.list = (from u in this.context.BRANCH_RIGHTS_MASTER
                     .Include("PACKAGE_MASTER")
                      .Include("BRANCH_MASTER")
                             join PM in this.context.PACKAGE_RIGHTS_MASTER on u.package_id equals PM.package_id
                             join page in this.context.PAGE_MASTER on PM.page_id equals page.page_id
                             where u.row_sta_cd == 1 && u.branch_id == item.branchinfo.BranchID
                             select new BranchWiseRightEntity()
                             {
                                 PageInfo = new PageEntity()
                                 {
                                     Page = page.page,
                                     PageID = page.page_id,
                                 },
                                 branchinfo = new BranchEntity()
                                 {
                                     BranchName = page.BRANCH_MASTER.branch_name,
                                     BranchID = page.branch_id,
                                 },
                                 Packageinfo = new PackageEntity()
                                 {
                                     Package = u.PACKAGE_MASTER.package,
                                     PackageID = u.package_id,
                                 },
                                 Createstatus = PM.createstatus,
                                 Viewstatus = PM.viewstatus,
                                 Deletestatus = PM.deletestatus,
                                 BranchWiseRightsID = u.branchrights_id,
                                 Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                             }).ToList();
            }
            return data;
        }

        public async Task<BranchWiseRightEntity> GetRightsByRightsID(long RightsID)
        {
            var data = (from u in this.context.BRANCH_RIGHTS_MASTER
                         .Include("PACKAGE_MASTER")
                         .Include("BRANCH_MASTER")
                        where u.row_sta_cd == 1 && u.branchrights_id == RightsID
                        select new BranchWiseRightEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },


                            BranchWiseRightsID = u.branchrights_id,
                            Packageinfo = new PackageEntity()
                            {
                                Package = u.PACKAGE_MASTER.package,
                                PackageID = u.package_id,
                            },
                            branchinfo = new BranchEntity()
                            {
                                BranchName = u.BRANCH_MASTER.branch_name,
                                BranchID = u.branch_id,
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).FirstOrDefault();

            return data;
        }

        public ResponseModel RemoveRights(long RightsID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var data = (from u in this.context.BRANCH_RIGHTS_MASTER
                            where u.branchrights_id == RightsID
                            select u).ToList();
                if (data != null)
                {
                    this.context.BRANCH_RIGHTS_MASTER.RemoveRange(data);
                    this.context.SaveChanges();
                    responseModel.Message = "Branch Rights Removed Successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.Message = "Branch Rights Not Found.";
                    responseModel.Status = true;
                }
            }
            catch(Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<List<BranchWiseRightEntity>> GetAllRightsUniqData(long PackageRightID)
        {
            var data = (from u in this.context.PACKAGE_RIGHTS_MASTER
                        .Include("PAGE_MASTER")
                        where u.row_sta_cd == 1 && u.package_id == PackageRightID
                        select new BranchWiseRightEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            PackageRightinfo = new PackageRightEntity()
                            {

                                PackageRightsId = u.packagerights_id,
                                Createstatus = u.createstatus,
                                Viewstatus = u.viewstatus,
                                Deletestatus = u.deletestatus,

                            },
                            PageInfo = new PageEntity()
                            {
                                Page = u.PAGE_MASTER.page,
                                PageID = u.page_id,
                            },

                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).ToList();

            return data;
        }

        public async Task<List<BranchWiseRightEntity>> GetAllRightsByBranch(long PackageRightID)
        {
            var data = (from u in this.context.BRANCH_RIGHTS_MASTER                       
                        .Include("PACKAGE_MASTER")
                         .Include("BRANCH_MASTER")
                        join PM in this.context.PACKAGE_RIGHTS_MASTER on u.package_id equals PM.package_id
                        join page in this.context.PAGE_MASTER on PM.page_id equals page.page_id
                        orderby PM.PAGE_MASTER.page
                        where u.row_sta_cd == 1 && u.branch_id == PackageRightID && PM.row_sta_cd == 1 && page.row_sta_cd == 1
                        select new BranchWiseRightEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            PackageRightinfo = new PackageRightEntity()
                            {

                                PackageRightsId = PM.packagerights_id,
                                Createstatus = PM.createstatus,
                                Viewstatus = PM.viewstatus,
                                Deletestatus = PM.deletestatus,

                            },
                            PageInfo = new PageEntity()
                            {
                                Page = PM.PAGE_MASTER.page,
                                PageID = PM.page_id,
                                Createstatus = PM.createstatus,
                                Viewstatus = PM.viewstatus,
                                Deletestatus = PM.deletestatus,
                            },

                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).ToList();

            return data;
        }
    }
}
