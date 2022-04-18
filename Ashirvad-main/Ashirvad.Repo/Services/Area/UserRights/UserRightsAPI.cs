using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.UserRights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area.UserRights
{
    public class UserRightsAPI : ModelAccess, IUserRightsAPI
    {
        public async Task<long> CheckRights(int RightsID, int UserId)
        {
            long result;
            bool isExists = this.context.USER_RIGHTS_MASTER.Where(s => (RightsID == 0 || s.user_rights_id != RightsID) && s.user_id == UserId && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<ResponseModel> RightsMaintenance(UserWiseRightsEntity RightsInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                Model.USER_RIGHTS_MASTER RightsMaster = new Model.USER_RIGHTS_MASTER();
                if (CheckRights((int)RightsInfo.UserWiseRightsID, (int)RightsInfo.UserID).Result != -1)
                {
                    bool isUpdate = true;
                    var data = (from User in this.context.USER_RIGHTS_MASTER
                                where User.user_rights_id == RightsInfo.UserWiseRightsID
                                select new
                                {
                                    RightsMaster = User
                                }).FirstOrDefault();
                    if (data == null)
                    {
                        RightsMaster = new Model.USER_RIGHTS_MASTER();
                        isUpdate = false;
                    }
                    else
                    {
                        RightsMaster = data.RightsMaster;
                        RightsInfo.Transaction.TransactionId = data.RightsMaster.trans_id;
                    }

                    RightsMaster.user_rights_id = RightsInfo.UserWiseRightsID;
                    RightsMaster.role_id = RightsInfo.Roleinfo.RoleID;
                    RightsMaster.row_sta_cd = RightsInfo.RowStatus.RowStatusId;
                    RightsMaster.user_id = RightsInfo.UserID;

                    RightsMaster.trans_id = this.AddTransactionData(RightsInfo.Transaction);
                    this.context.USER_RIGHTS_MASTER.Add(RightsMaster);
                    if (isUpdate)
                    {
                        this.context.Entry(RightsMaster).State = System.Data.Entity.EntityState.Modified;
                    }
                    var result = this.context.SaveChanges();
                    if (result > 0)
                    {
                        RightsInfo.UserWiseRightsID = RightsMaster.user_rights_id;
                        //var result2 = UserDetailMaintenance(UserInfo).Result;
                        //return result > 0 ? RightsInfo.UserWiseRightsID : 0;
                        responseModel.Data = RightsInfo;
                        responseModel.Message = isUpdate == true ? "User Rights Updated Successfully." : "User Rights Inserted Successfully.";
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = isUpdate == true ? "User Rights Not Updated." : "User Rights Not Inserted.";
                        responseModel.Status = false;
                    }
                    //  return this.context.SaveChanges() > 0 ? RightsInfo.UserWiseRightsID : 0;
                }
                else
                {
                    responseModel.Message = "User Rights Already Exists.";
                    responseModel.Status = false;
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }

            return responseModel;
        }

        public async Task<List<UserWiseRightsEntity>> GetAllRights()
        {
            var data = (from u in this.context.USER_RIGHTS_MASTER
                                .Include("BRANCH_MASTER")
                                .Include("USER_DEF")
                        where u.row_sta_cd == 1
                        select new UserWiseRightsEntity()
                        {
                            userinfo = new UserEntity()
                            {
                                Username = u.USER_DEF.username,
                                UserID = u.USER_DEF.user_id

                            },
                            Roleinfo = new RoleEntity()
                            {
                                RoleName = u.ROLE_MASTER.role_name
                            },
                            UserWiseRightsID = u.user_rights_id,

                        }).Distinct().OrderByDescending(a => a.UserWiseRightsID).ToList();

            if (data?.Count > 0)
            {

                foreach (var item in data)
                {
                    item.list = new List<UserWiseRightsEntity>();
                    item.list = (from u in this.context.USER_RIGHTS_MASTER
                   .Include("ROLE_MASTER")
                   .Include("BRANCH_MASTER")
                    .Include("USER_DEF")
                                 join PM in this.context.ROLE_RIGHTS_MASTER on u.role_id equals PM.role_id
                                 join page in this.context.PAGE_MASTER on PM.page_id equals page.page_id
                                 where u.row_sta_cd == 1 && u.user_id == item.userinfo.UserID
                                 select new UserWiseRightsEntity()
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
                                     Roleinfo = new RoleEntity()
                                     {
                                         RoleName = u.ROLE_MASTER.role_name,
                                         RoleID = u.ROLE_MASTER.role_id,
                                     },
                                     Createstatus = PM.createstatus,
                                     Viewstatus = PM.viewstatus,
                                     Deletestatus = PM.deletestatus,
                                     UserWiseRightsID = u.user_rights_id,
                                     Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                                 }).ToList();

                }

            }
            return data;
        }

        public async Task<List<UserWiseRightsEntity>> GetAllCustomRights(DataTableAjaxPostModel model, long branchId)
        {
            bool Isasc = model.order[0].dir == "desc" ? false : true;
            long count = (from u in this.context.USER_RIGHTS_MASTER
                                .Include("USER_DEF")
                                .Include("BRANCH_STAFF")
                          join staff in this.context.BRANCH_STAFF on u.USER_DEF.staff_id equals staff.staff_id
                          orderby u.user_rights_id descending
                          where u.row_sta_cd == 1 && u.ROLE_MASTER.branch_id == branchId
                          select new UserWiseRightsEntity()
                          {
                              userinfo = new UserEntity()
                              {
                                  Username = u.USER_DEF.username,
                                  UserID = u.user_id,
                                  StaffDetail = new StaffEntity()
                                  {
                                      Name = staff.name
                                  }
                              },
                              Roleinfo = new RoleEntity()
                              {
                                  RoleName = u.ROLE_MASTER.role_name
                              },
                              UserWiseRightsID = u.user_rights_id
                          }).Distinct().Count();
            var data = (from u in this.context.USER_RIGHTS_MASTER
                                .Include("USER_DEF")
                                .Include("BRANCH_STAFF")
                        join staff in this.context.BRANCH_STAFF on u.USER_DEF.staff_id equals staff.staff_id
                        orderby u.user_rights_id descending
                        where u.row_sta_cd == 1 && u.ROLE_MASTER.branch_id == branchId 
                        && (model.search.value == null
                        || model.search.value == ""
                        || u.ROLE_MASTER.role_name.ToLower().Contains(model.search.value)
                        || u.USER_DEF.username.ToLower().Contains(model.search.value))
                        select new UserWiseRightsEntity()
                        {
                            userinfo = new UserEntity()
                            {
                                Username = u.USER_DEF.username,
                                UserID = u.user_id,
                                StaffDetail = new StaffEntity()
                                {
                                    Name = staff.name
                                }
                            },
                            Roleinfo = new RoleEntity()
                            {
                                RoleName = u.ROLE_MASTER.role_name
                            },
                            UserWiseRightsID = u.user_rights_id,
                            Count = count
                        }).Distinct()
                        .OrderByDescending(a => a.UserWiseRightsID)
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();
            foreach (var item in data)
            {
                item.list = (from u in this.context.USER_RIGHTS_MASTER
                     .Include("ROLE_MASTER")
                      .Include("USER_DEF")
                      .Include("BRANCH_STAFF")
                             join staff in this.context.BRANCH_STAFF on u.USER_DEF.staff_id equals staff.staff_id
                             join PM in this.context.ROLE_RIGHTS_MASTER on u.role_id equals PM.role_id
                             join page in this.context.PAGE_MASTER on PM.page_id equals page.page_id
                             where u.row_sta_cd == 1 && u.user_id == item.userinfo.UserID
                             select new UserWiseRightsEntity()
                             {
                                 PageInfo = new PageEntity()
                                 {
                                     Page = page.page,
                                     PageID = page.page_id,
                                 },
                                 userinfo = new UserEntity()
                                 {
                                     Username = u.USER_DEF.username,
                                     UserID = u.user_id,
                                     StaffDetail = new StaffEntity()
                                     {
                                         Name = staff.name
                                     }
                                 },
                                 Roleinfo = new RoleEntity()
                                 {
                                     RoleName = u.ROLE_MASTER.role_name,
                                     RoleID = u.role_id,
                                 },
                                 Createstatus = PM.createstatus,
                                 Viewstatus = PM.viewstatus,
                                 Deletestatus = PM.deletestatus,
                                 UserWiseRightsID = u.user_rights_id,
                                 Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                             }).ToList();
            }
            return data;
        }

        public async Task<UserWiseRightsEntity> GetRightsByRightsID(long RightsID)
        {
            var data = (from u in this.context.USER_RIGHTS_MASTER
                         .Include("ROLE_MASTER")
                         .Include("BRANCH_MASTER")
                         .Include("USER_DEF")
                        where u.row_sta_cd == 1 && u.user_rights_id == RightsID
                        select new UserWiseRightsEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            userinfo = new UserEntity()
                            {
                                UserID = u.USER_DEF.user_id,
                                Username = u.USER_DEF.username
                            },
                            UserWiseRightsID = u.user_rights_id,
                            Roleinfo = new RoleEntity()
                            {
                                RoleName = u.ROLE_MASTER.role_name,
                                RoleID = u.role_id,
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
                var data = (from u in this.context.USER_RIGHTS_MASTER
                            where u.user_rights_id == RightsID
                            select u).ToList();
                if (data != null)
                {
                    this.context.USER_RIGHTS_MASTER.RemoveRange(data);
                    this.context.SaveChanges();
                    responseModel.Message = "User Rights Removed Successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.Message = "User Rights Not Found.";
                    responseModel.Status = true;
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<List<UserWiseRightsEntity>> GetAllRightsUniqData(long RoleID)
        {
            var data = (from u in this.context.ROLE_RIGHTS_MASTER
                        .Include("PAGE_MASTER")
                        where u.row_sta_cd == 1 && u.role_id == RoleID
                        select new UserWiseRightsEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            RoleRightinfo = new RoleRightsEntity()
                            {

                                RoleRightsId = u.rolerights_id,
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

        public async Task<List<UserWiseRightsEntity>> GetAllRightsByUser(long RoleID)
        {
            var data = (from u in this.context.USER_RIGHTS_MASTER
                        .Include("ROLE_MASTER")
                         .Include("BRANCH_MASTER")
                         .Include("USER_DEF")
                        join PM in this.context.ROLE_RIGHTS_MASTER on u.role_id equals PM.role_id
                        join page in this.context.PAGE_MASTER on PM.page_id equals page.page_id
                        orderby PM.PAGE_MASTER.page
                        where u.row_sta_cd == 1 && u.user_id == RoleID && PM.row_sta_cd == 1 && page.row_sta_cd == 1
                        select new UserWiseRightsEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            RoleRightinfo = new RoleRightsEntity()
                            {

                                RoleRightsId = PM.rolerights_id,
                                Createstatus = PM.createstatus,
                                Viewstatus = PM.viewstatus,
                                Deletestatus = PM.deletestatus,

                            },
                            PageInfo = new PageEntity()
                            {
                                Page = PM.PAGE_MASTER.page,
                                PageID = PM.page_id,
                            },

                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).ToList();

            return data;
        }
    }
}
