using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.RoleRights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area.RoleRights
{
   public class RoleRightsAPI : ModelAccess, IRoleRightsAPI
    {

        public async Task<long> CheckRights(int RightsID, int RoleID, int PageID)
        {
            long result;
            bool isExists = this.context.ROLE_RIGHTS_MASTER.Where(s => (RightsID == 0 || s.rolerights_id != RightsID) && s.role_id == RoleID && s.page_id == PageID && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }
        public async Task<ResponseModel> RightsMaintenance(RoleRightsEntity RightsInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                Model.ROLE_RIGHTS_MASTER RightsMaster = new Model.ROLE_RIGHTS_MASTER();
                if (CheckRights((int)RightsInfo.RoleRightsId, (int)RightsInfo.Roleinfo.RoleID, (int)RightsInfo.PageInfo.PageID).Result != -1)
                {
                    bool isUpdate = true;
                    var data = (from Role in this.context.ROLE_RIGHTS_MASTER
                                where Role.rolerights_id == RightsInfo.RoleRightsId
                                select new
                                {
                                    RightsMaster = Role
                                }).FirstOrDefault();
                    if (data == null)
                    {
                        RightsMaster = new Model.ROLE_RIGHTS_MASTER();
                        isUpdate = false;
                    }
                    else
                    {
                        RightsMaster = data.RightsMaster;
                        RightsInfo.Transaction.TransactionId = data.RightsMaster.trans_id;
                    }

                    RightsMaster.role_id = RightsInfo.Roleinfo.RoleID;
                    RightsMaster.page_id = RightsInfo.PageInfo.PageID;
                    // RightsMaster.row_sta_cd = RightsInfo.RowStatus.RowStatus == Enums.RowStatus.Active ? (int)Enums.RowStatus.Active : (int)Enums.RowStatus.Inactive;
                    RightsMaster.row_sta_cd = RightsInfo.RowStatus.RowStatusId;
                    RightsMaster.createstatus = RightsInfo.Createstatus;
                    RightsMaster.viewstatus = RightsInfo.Viewstatus;
                    RightsMaster.deletestatus = RightsInfo.Deletestatus;
                    RightsMaster.trans_id = RightsInfo.Transaction.TransactionId > 0 ? RightsInfo.Transaction.TransactionId : this.AddTransactionData(RightsInfo.Transaction);

                    //  RightsMaster.trans_id = this.AddTransactionData(RightsInfo.Transaction);
                    this.context.ROLE_RIGHTS_MASTER.Add(RightsMaster);
                    if (isUpdate)
                    {
                        this.context.Entry(RightsMaster).State = System.Data.Entity.EntityState.Modified;
                    }
                    var result = this.context.SaveChanges();
                    if (result > 0)
                    {
                        RightsInfo.RoleRightsId = RightsMaster.rolerights_id;
                        //var result2 = RoleDetailMaintenance(RoleInfo).Result;
                        //return result > 0 ? RightsInfo.RoleRightsId : 0;
                        responseModel.Data = RightsInfo;
                        responseModel.Message = isUpdate == true ? "Role Rights Updated Successfully." : "Role Rights Inserted Successfully.";
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = isUpdate == true ? "Role Rights Not Updated." : "Role Rights Not Inserted.";
                        responseModel.Status = false;
                    }
                    //return this.context.SaveChanges() > 0 ? RightsInfo.RoleRightsId : 0;
                }
                else
                {
                    responseModel.Message = "Role Rights Already Exists.";
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

        public async Task<List<RoleRightsEntity>> GetAllRights()
        {
            var data = (from u in this.context.ROLE_RIGHTS_MASTER
                        .Include("Role_MASTER")
                        .Include("PAGE_MASTER")
                        where u.row_sta_cd == 1
                        select new RoleRightsEntity()
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
                            Roleinfo = new RoleEntity()
                            {
                                RoleName = u.ROLE_MASTER.role_name,
                                RoleID = u.ROLE_MASTER.role_id
                            },
                            Createstatus = u.createstatus,
                            Viewstatus = u.viewstatus,
                            Deletestatus = u.deletestatus,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).ToList();
            if (data.Count > 0)
            {
                data[0].list = (from u in this.context.ROLE_RIGHTS_MASTER
                              .Include("ROLE_MASTER")
                               .Include("PAGE_MASTER")
                                where u.row_sta_cd == 1
                                select new RoleRightsEntity()
                                {
                                    Roleinfo = new RoleEntity()
                                    {
                                        RoleName = u.ROLE_MASTER.role_name,
                                        RoleID = u.ROLE_MASTER.role_id
                                    },

                                }).Distinct().ToList();
            }
            else
            {
                RoleRightsEntity entity = new RoleRightsEntity();
                entity.list = new List<RoleRightsEntity>();
                data.Add(entity);
            }
            return data;

        } 
        public async Task<List<RoleRightsEntity>> GetAllRightsbyBranch(long branchId)
        {
            var data = (from u in this.context.ROLE_RIGHTS_MASTER
                        .Include("ROLE_MASTER")
                        .Include("PAGE_MASTER")
                        where u.row_sta_cd == 1 && u.ROLE_MASTER.branch_id==branchId
                        select new RoleRightsEntity()
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
                            Roleinfo = new RoleEntity()
                            {
                                RoleName = u.ROLE_MASTER.role_name,
                                RoleID = u.ROLE_MASTER.role_id
                            },
                            Createstatus = u.createstatus,
                            Viewstatus = u.viewstatus,
                            Deletestatus = u.deletestatus,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).ToList();
            if (data.Count > 0)
            {
                data[0].list = (from u in this.context.ROLE_RIGHTS_MASTER
                              .Include("ROLE_MASTER")
                               .Include("PAGE_MASTER")
                                where u.row_sta_cd == 1
                                select new RoleRightsEntity()
                                {
                                    Roleinfo = new RoleEntity()
                                    {
                                        RoleName = u.ROLE_MASTER.role_name,
                                        RoleID = u.ROLE_MASTER.role_id
                                    },

                                }).Distinct().ToList();
            }
            else
            {
                RoleRightsEntity entity = new RoleRightsEntity();
                entity.list = new List<RoleRightsEntity>();
                data.Add(entity);
            }
            return data;

        }

        public async Task<List<RoleRightsEntity>> GetAllCustomRights(DataTableAjaxPostModel model, long branchId)
        {
            bool Isasc = model.order[0].dir == "desc" ? false : true;
            long count = (from u in this.context.ROLE_RIGHTS_MASTER
                        .Include("ROLE_MASTER")
                        .Include("PAGE_MASTER")
                          orderby u.rolerights_id descending
                          where u.row_sta_cd == 1 && u.ROLE_MASTER.branch_id == branchId
                          select new RoleRightsEntity()
                          {
                              Roleinfo = new RoleEntity()
                              {
                                  RoleName = u.ROLE_MASTER.role_name,
                                  RoleID = u.ROLE_MASTER.role_id
                              }
                          }).Distinct().Count();
            var data = (from u in this.context.ROLE_RIGHTS_MASTER
                        .Include("ROLE_MASTER")
                        .Include("PAGE_MASTER")
                        orderby u.rolerights_id descending
                        where u.row_sta_cd == 1 && u.ROLE_MASTER.branch_id == branchId && (model.search.value == null
                        || model.search.value == ""
                        || u.ROLE_MASTER.role_name.ToLower().Contains(model.search.value))
                        select new RoleRightsEntity()
                        {
                            Roleinfo = new RoleEntity()
                            {
                                RoleName = u.ROLE_MASTER.role_name,
                                RoleID = u.ROLE_MASTER.role_id
                            },
                            Count = count
                        }).Distinct()
                        .OrderByDescending(a => a.Roleinfo.RoleID)
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();
            foreach (var item in data)
            {
                item.list = (from u in this.context.ROLE_RIGHTS_MASTER
                              .Include("ROLE_MASTER")
                               .Include("PAGE_MASTER")
                             where u.row_sta_cd == 1 && u.role_id == item.Roleinfo.RoleID
                             select new RoleRightsEntity()
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
                                 Roleinfo = new RoleEntity()
                                 {
                                     RoleName = u.ROLE_MASTER.role_name,
                                     RoleID = u.ROLE_MASTER.role_id
                                 },
                                 Createstatus = u.createstatus,
                                 Viewstatus = u.viewstatus,
                                 Deletestatus = u.deletestatus,
                                 Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                             }).ToList();
            }
            return data;
        }

        public async Task<List<RoleRightsEntity>> GetRightsByRightsID(long RightsID,long branchId)
        {
            var data = (from u in this.context.ROLE_RIGHTS_MASTER
                       .Include("Role_MASTER")
                       .Include("PAGE_MASTER")
                       join branch in this.context.BRANCH_RIGHTS_MASTER on u.ROLE_MASTER.branch_id equals branch.branch_id
                       join PM in this.context.PACKAGE_RIGHTS_MASTER on branch.package_id equals PM.package_id
                        where u.row_sta_cd == 1 && u.role_id == RightsID && u.ROLE_MASTER.branch_id==branchId && PM.page_id == u.page_id && PM.row_sta_cd==1
                        select new RoleRightsEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            PageInfo = new PageEntity()
                            {
                                Page = u.PAGE_MASTER.page,
                                PageID = u.page_id,
                                Createstatus = PM.createstatus,
                                Viewstatus = PM.viewstatus,
                                Deletestatus = PM.deletestatus,
                            },
                            RoleRightsId = u.rolerights_id,
                            Createstatus =u.createstatus,
                            Viewstatus = u.viewstatus,
                            Deletestatus = u.deletestatus,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).ToList();
            return data;
        }
   
        public async Task<RoleRightsEntity> GetRolebyID(long RightsID)
        {
            var data = (from u in this.context.ROLE_RIGHTS_MASTER
                       .Include("ROLE_MASTER")
                        where u.row_sta_cd == 1 && u.role_id == RightsID
                        select new RoleRightsEntity()
                        {
                            RoleRightsId = u.rolerights_id,
                            Roleinfo = new RoleEntity()
                            {
                                RoleID = u.ROLE_MASTER.role_id,
                                RoleName = u.ROLE_MASTER.role_name
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
                var data = (from u in this.context.ROLE_RIGHTS_MASTER
                            where u.role_id == RightsID
                            select u).ToList();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        var data_course = check.check_remove_role_rights(item.role_id).Result;
                        if (data_course.Status)
                        {
                            item.row_sta_cd = (int)Enums.RowStatus.Inactive;
                        item.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = item.trans_id, LastUpdateBy = lastupdatedby });
                        this.context.SaveChanges();
                        responseModel.Message = "Role Rights Removed Successfully.";
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
                    responseModel.Message = "Role Rights Not Found.";
                    responseModel.Status = true;
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

    }
}
