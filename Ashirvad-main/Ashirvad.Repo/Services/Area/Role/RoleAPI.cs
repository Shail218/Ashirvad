using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area
{
    public class RoleAPI: ModelAccess, IRoleAPI
    {
        public async Task<long> CheckRole(string name, long Id,long branchId)
        {
            long result;
            bool isExists = this.context.ROLE_MASTER.Where(s => (Id == 0 || s.role_id != Id) && s.role_name == name && s.branch_id== branchId && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<ResponseModel> RoleMaintenance(RoleEntity roleInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                Model.ROLE_MASTER roleMaster = new Model.ROLE_MASTER();
                if (CheckRole(roleInfo.RoleName, roleInfo.RoleID,roleInfo.BranchInfo.BranchID).Result != -1)
                {
                    bool isUpdate = true;
                    var data = (from role in this.context.ROLE_MASTER
                                where role.role_id == roleInfo.RoleID
                                select role).FirstOrDefault();
                    if (data == null)
                    {
                        roleMaster = new Model.ROLE_MASTER();

                        isUpdate = false;
                    }
                    else
                    {
                        roleMaster = data;
                        roleInfo.Transaction.TransactionId = data.trans_id;
                    }

                    roleMaster.role_name = roleInfo.RoleName;
                    roleMaster.branch_id = roleInfo.BranchInfo.BranchID;
                    roleMaster.row_sta_cd = roleInfo.RowStatus.RowStatusId;
                    roleMaster.trans_id = this.AddTransactionData(roleInfo.Transaction);
                    this.context.ROLE_MASTER.Add(roleMaster);
                    if (isUpdate)
                    {
                        this.context.Entry(roleMaster).State = System.Data.Entity.EntityState.Modified;
                    }
                    var da = this.context.SaveChanges() > 0 ? roleMaster.role_id : 0;
                    if (da > 0)
                    {
                        roleInfo.RoleID = da;
                        responseModel.Data = roleInfo;
                        responseModel.Message = isUpdate == true ? "Role Updated Successfully." : "Role Inserted Successfully.";
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = isUpdate == true ? "Role Not Updated." : "Role Not Inserted.";
                        responseModel.Status = false;
                    }
                }
                else
                {
                    responseModel.Message = "Role Already Exists.";
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

        public async Task<List<RoleEntity>> GetAllRoles(long branchID)
        {
            var data = (from u in this.context.ROLE_MASTER
                        orderby u.role_id descending
                        where u.branch_id == branchID && u.row_sta_cd == 1
                        select new RoleEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            RoleName = u.role_name,
                            RoleID = u.role_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public async Task<List<RoleEntity>> GetAllCustomRole(Common.Common.DataTableAjaxPostModel model, long branchID)
        {
            var Count = this.context.ROLE_MASTER.Where(a => a.branch_id == branchID && a.row_sta_cd == 1).Count();
            var data = (from u in this.context.ROLE_MASTER
                        orderby u.role_id descending
                        where u.branch_id == branchID && u.row_sta_cd == 1
                        select new RoleEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            RoleName = u.role_name,
                            RoleID = u.role_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Count = Count,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }
      
        public async Task<List<RoleEntity>> GetAllRoles()
        {
            var data = (from u in this.context.ROLE_MASTER
                        orderby u.role_id descending
                        where u.row_sta_cd == 1
                        select new RoleEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            RoleName = u.role_name,
                            RoleID = u.role_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public ResponseModel RemoveRole(long RoleID, string lastupdatedby)
        {
            Check_Delete check = new Check_Delete();
            ResponseModel responseModel = new ResponseModel();
            string message = "";
            try
            {

                var data = (from u in this.context.ROLE_MASTER
                            where u.role_id == RoleID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    var data_course = check.check_remove_role(data.role_id).Result;
                    if (data_course.Status)
                    {
                        data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                        data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                        this.context.SaveChanges();
                        // return true;
                        responseModel.Status = true;
                        responseModel.Message = "Role Removed Successfully.";
                    }
                    else
                    {
                        responseModel.Message = data_course.Message;
                        responseModel.Status = false;
                    }
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "Role Not Found.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
            return responseModel;

            //return false;
        }

        public async Task<RoleEntity> GetRoleByID(long RoleID)
        {
            var data = (from u in this.context.ROLE_MASTER
                        where u.role_id == RoleID
                        select new RoleEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            RoleName = u.role_name,
                            RoleID = u.role_id,
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
