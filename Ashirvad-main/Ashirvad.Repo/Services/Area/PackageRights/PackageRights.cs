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

namespace Ashirvad.Repo.Services.Area.Branch
{
    public class PackageRights : ModelAccess, IPackageRightsAPI
    {

        public async Task<long> CheckRights(int RightsID, int packageID, int PageID)
        {
            long result;
            bool isExists = this.context.PACKAGE_RIGHTS_MASTER.Where(s => (RightsID == 0 || s.packagerights_id != RightsID) && s.package_id == packageID && s.page_id == PageID && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }
        public async Task<ResponseModel> RightsMaintenance(PackageRightEntity RightsInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                Model.PACKAGE_RIGHTS_MASTER RightsMaster = new Model.PACKAGE_RIGHTS_MASTER();
                PackageRightEntity OldRightsMaster = new PackageRightEntity();
                if (CheckRights((int)RightsInfo.PackageRightsId, (int)RightsInfo.Packageinfo.PackageID, (int)RightsInfo.PageInfo.PageID).Result != -1)
                {
                    bool isUpdate = true;
                    var data = (from package in this.context.PACKAGE_RIGHTS_MASTER
                                where package.packagerights_id == RightsInfo.PackageRightsId
                                orderby package.page_id
                                select new
                                {
                                    RightsMaster = package
                                }).FirstOrDefault();
                    var data2 = (from u in this.context.PACKAGE_RIGHTS_MASTER
                                 where u.packagerights_id == RightsInfo.PackageRightsId
                                 orderby u.page_id
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
                                     PackageRightsId = u.packagerights_id,
                                     Createstatus = u.createstatus,
                                     Viewstatus = u.viewstatus,
                                     Deletestatus = u.deletestatus,
                                 }).FirstOrDefault();
                    if (data == null)
                    {
                        RightsMaster = new Model.PACKAGE_RIGHTS_MASTER();
                        isUpdate = false;
                    }
                    else
                    {
                        OldRightsMaster = data2;
                        RightsMaster = data.RightsMaster;
                        RightsInfo.Transaction.TransactionId = data.RightsMaster.trans_id;
                    }

                    RightsMaster.package_id = RightsInfo.Packageinfo.PackageID;
                    RightsMaster.page_id = RightsInfo.PageInfo.PageID;
                    // RightsMaster.row_sta_cd = RightsInfo.RowStatus.RowStatus == Enums.RowStatus.Active ? (int)Enums.RowStatus.Active : (int)Enums.RowStatus.Inactive;
                    RightsMaster.row_sta_cd = RightsInfo.RowStatus.RowStatusId;
                    RightsMaster.createstatus = RightsInfo.Createstatus;
                    RightsMaster.viewstatus = RightsInfo.Viewstatus;
                    RightsMaster.deletestatus = RightsInfo.Deletestatus;
                    RightsMaster.trans_id = RightsInfo.Transaction.TransactionId > 0 ? RightsInfo.Transaction.TransactionId : this.AddTransactionData(RightsInfo.Transaction);

                    //  RightsMaster.trans_id = this.AddTransactionData(RightsInfo.Transaction);
                    this.context.PACKAGE_RIGHTS_MASTER.Add(RightsMaster);
                    if (isUpdate)
                    {
                        this.context.Entry(RightsMaster).State = System.Data.Entity.EntityState.Modified;
                    }
                    var result = this.context.SaveChanges();
                    if (result > 0)
                    {
                        ComparePackageRight(OldRightsMaster, RightsInfo);
                        RightsInfo.PackageRightsId = RightsMaster.packagerights_id;
                        //var result2 = PackageDetailMaintenance(PackageInfo).Result;
                        //return result > 0 ? RightsInfo.PackageRightsId : 0;
                        responseModel.Data = RightsInfo;
                        responseModel.Message = isUpdate == true ? "Package Rights Updated Successfully." : "Package Rights Inserted Successfully.";
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = isUpdate == true ? "Package Rights Not Updated." : "Package Rights Not Inserted.";
                        responseModel.Status = false;
                    }
                    //return this.context.SaveChanges() > 0 ? RightsInfo.PackageRightsId : 0;
                }
                else
                {
                    responseModel.Message = "Package Rights Already Exists.";
                    responseModel.Status = false;
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
            return responseModel;
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
                            Packageinfo = new PackageEntity()
                            {
                                Package = u.PACKAGE_MASTER.package,
                                PackageID = u.PACKAGE_MASTER.package_id
                            },
                            Createstatus = u.createstatus,
                            Viewstatus = u.viewstatus,
                            Deletestatus = u.deletestatus,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).ToList();
            if (data.Count > 0)
            {
                data[0].list = (from u in this.context.PACKAGE_RIGHTS_MASTER
                              .Include("PACKAGE_MASTER")
                               .Include("PAGE_MASTER")
                                where u.row_sta_cd == 1
                                select new PackageRightEntity()
                                {
                                    Packageinfo = new PackageEntity()
                                    {
                                        Package = u.PACKAGE_MASTER.package,
                                        PackageID = u.PACKAGE_MASTER.package_id
                                    },

                                }).Distinct().ToList();
            }
            else
            {
                PackageRightEntity entity = new PackageRightEntity();
                entity.list = new List<PackageRightEntity>();
                data.Add(entity);
            }
            return data;

        }

        public async Task<List<PackageRightEntity>> GetAllCustomRights(DataTableAjaxPostModel model)
        {
            bool Isasc = model.order[0].dir == "desc" ? false : true;
            long count = (from u in this.context.PACKAGE_RIGHTS_MASTER
                        .Include("PACKAGE_MASTER")
                        .Include("PAGE_MASTER")
                          orderby u.packagerights_id descending
                          where u.row_sta_cd == 1
                          select new PackageRightEntity()
                          {
                              Packageinfo = new PackageEntity()
                              {
                                  Package = u.PACKAGE_MASTER.package,
                                  PackageID = u.PACKAGE_MASTER.package_id
                              }
                          }).Distinct().Count();
            var data = (from u in this.context.PACKAGE_RIGHTS_MASTER
                        .Include("PACKAGE_MASTER")
                        .Include("PAGE_MASTER")
                        orderby u.packagerights_id descending
                        where u.row_sta_cd == 1 && (model.search.value == null
                        || model.search.value == ""
                        || u.PACKAGE_MASTER.package.ToLower().Contains(model.search.value))
                        select new PackageRightEntity()
                        {
                            Packageinfo = new PackageEntity()
                            {
                                Package = u.PACKAGE_MASTER.package,
                                PackageID = u.PACKAGE_MASTER.package_id
                            },
                            Count = count
                        }).Distinct()
                        .OrderByDescending(a => a.Packageinfo.PackageID)
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();
            foreach (var item in data)
            {
                item.list = (from u in this.context.PACKAGE_RIGHTS_MASTER
                              .Include("PACKAGE_MASTER")
                               .Include("PAGE_MASTER")
                             where u.row_sta_cd == 1 && u.package_id == item.Packageinfo.PackageID
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
                                 Packageinfo = new PackageEntity()
                                 {
                                     Package = u.PACKAGE_MASTER.package,
                                     PackageID = u.PACKAGE_MASTER.package_id
                                 },
                                 Createstatus = u.createstatus,
                                 Viewstatus = u.viewstatus,
                                 Deletestatus = u.deletestatus,
                                 Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                             }).ToList();
            }
            return data;
        }

        public async Task<List<PackageRightEntity>> GetRightsByRightsID(long RightsID)
        {
            var data = (from u in this.context.PACKAGE_RIGHTS_MASTER
                       .Include("PACKAGE_MASTER")
                       .Include("PAGE_MASTER")
                        where u.row_sta_cd == 1 && u.package_id == RightsID
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
                            PackageRightsId = u.packagerights_id,
                            Createstatus = u.createstatus,
                            Viewstatus = u.viewstatus,
                            Deletestatus = u.deletestatus,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).ToList();
            return data;
        }
        public async Task<PackageRightEntity> GetPackagebyID(long RightsID)
        {
            var data = (from u in this.context.PACKAGE_RIGHTS_MASTER
                       .Include("PACKAGE_MASTER")
                        where u.row_sta_cd == 1 && u.package_id == RightsID
                        select new PackageRightEntity()
                        {
                            PackageRightsId = u.packagerights_id,
                            Packageinfo = new PackageEntity()
                            {
                                PackageID = u.PACKAGE_MASTER.package_id,
                                Package = u.PACKAGE_MASTER.package
                            },

                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).FirstOrDefault();
            return data;
        }

        public ResponseModel RemoveRights(long RightsID, string lastupdatedby)
        {
            Check_Delete check = new Check_Delete();
            ResponseModel responseModel = new ResponseModel();
            string message = "";
            try
            {
                var data = (from u in this.context.PACKAGE_RIGHTS_MASTER
                            where u.package_id == RightsID
                            select u).ToList();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        var data_course = check.check_remove_package_rights(item.package_id).Result;
                        if (data_course.Status)
                        {
                            item.row_sta_cd = (int)Enums.RowStatus.Inactive;
                            item.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = item.trans_id, LastUpdateBy = lastupdatedby });
                            this.context.SaveChanges();
                            responseModel.Message = "Package Rights Removed Successfully.";
                            responseModel.Status = true;
                        }
                        else
                        {
                            responseModel.Message = data_course.Message;
                            responseModel.Status = false;
                            break;
                        }
                    }
                }
                else
                {
                    responseModel.Message = "Package Rights Not Found.";
                    responseModel.Status = false;
                }
                //return true;               
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            return responseModel;
            //return false;
        }

        public void ComparePackageRight(PackageRightEntity oldpackagerights, PackageRightEntity newpackagerights)
        {
            try
            {
                bool viewflag = false, createflag = false, deleteflag = false;
                if (oldpackagerights.PageInfo.PageID == newpackagerights.PageInfo.PageID)
                {
                    createflag = oldpackagerights.Createstatus == newpackagerights.Createstatus ? false : true;
                    deleteflag = oldpackagerights.Deletestatus == newpackagerights.Deletestatus ? false : true;
                    viewflag = oldpackagerights.Viewstatus == newpackagerights.Viewstatus ? false : true;
                }
                if (createflag || deleteflag || viewflag)
                {
                    List<Model.ROLE_RIGHTS_MASTER> roleRightList = new List<Model.ROLE_RIGHTS_MASTER>();
                    List<Model.ROLE_RIGHTS_MASTER> roleRightUpdateList = new List<Model.ROLE_RIGHTS_MASTER>();
                    var branchdata = (from u in this.context.BRANCH_RIGHTS_MASTER where (u.package_id == newpackagerights.Packageinfo.PackageID && u.row_sta_cd == 1) select u.branch_id).ToList();
                    foreach (var branchid in branchdata)
                    {
                        var roleright = (from role in this.context.ROLE_RIGHTS_MASTER where role.ROLE_MASTER.branch_id == branchid && role.page_id == newpackagerights.PageInfo.PageID select role).ToList();
                        if (roleright?.Count > 0)
                        {
                            roleRightList.AddRange(roleright);
                        }
                    }
                    if (roleRightList?.Count > 0)
                    {
                        foreach (var roleright in roleRightList)
                        {
                            if (createflag)
                            {
                                roleright.createstatus = newpackagerights.Createstatus==false
                                    ?roleright.createstatus==true
                                    ?false
                                    : roleright.createstatus
                                    : roleright.createstatus == true
                                    ? roleright.createstatus
                                    : false;
                            }
                            if (deleteflag)
                            {
                                roleright.deletestatus = newpackagerights.Deletestatus == false
                                    ? roleright.deletestatus == true
                                    ? false
                                    : roleright.deletestatus
                                    : roleright.deletestatus == true
                                    ? roleright.deletestatus
                                    : false;
                            }
                            if (viewflag)
                            {
                                roleright.viewstatus = newpackagerights.Viewstatus == false
                                    ? roleright.viewstatus == true
                                    ? false
                                    : roleright.viewstatus
                                    : roleright.viewstatus == true
                                    ? roleright.viewstatus
                                    : false;
                            }
                            this.context.Entry(roleright).State = System.Data.Entity.EntityState.Modified;
                            roleRightUpdateList.Add(roleright);
                            
                        }
                        this.context.ROLE_RIGHTS_MASTER.AddRange(roleRightUpdateList);
                        foreach(var item in roleRightUpdateList)
                        {
                            this.context.Entry(item).State = System.Data.Entity.EntityState.Modified;

                        }
                        this.context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                var s = ex.Message;
            }

        }

    }
}
