using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.ToDo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.ToDo
{
    public class ToDo : ModelAccess, IToDoAPI
    {
        public async Task<long> ToDoMaintenance(ToDoEntity todoInfo)
        {
            Model.TODO_MASTER todo = new Model.TODO_MASTER();
            bool isUpdate = true;
            var data = (from t in this.context.TODO_MASTER
                        where t.todo_id == todoInfo.ToDoID
                        select t).FirstOrDefault();
            if (data == null)
            {
                data = new Model.TODO_MASTER();
                isUpdate = false;
            }
            else
            {
                todo = data;
                todoInfo.Transaction.TransactionId = data.trans_id;
            }

            todo.row_sta_cd = todoInfo.RowStatus.RowStatusId;
            todo.trans_id = this.AddTransactionData(todoInfo.Transaction);
            todo.branch_id = todoInfo.BranchInfo.BranchID;
            todo.reg_status = todoInfo.Registerstatus;
            todo.remark = todoInfo.Remark;
            todo.branch_id = todoInfo.BranchInfo.BranchID;
            todo.todo_desc = todoInfo.ToDoDescription;
            todo.todo_doc_name = todoInfo.ToDoFileName;
            todo.file_path = todoInfo.FilePath;
            todo.todo_dt = todoInfo.ToDoDate;
            todo.user_id = todoInfo.UserInfo.UserID;
            this.context.TODO_MASTER.Add(todo);
            if (isUpdate)
            {
                this.context.Entry(todo).State = System.Data.Entity.EntityState.Modified;
            }

            return this.context.SaveChanges() > 0 ? todo.todo_id : 0;
        }

        public async Task<List<ToDoEntity>> GetAllToDoByBranch(long branchID, long userID)
        {
            var data = (from u in this.context.TODO_MASTER
                        .Include("BRANCH_MASTER")
                        join ud in this.context.BRANCH_STAFF on u.user_id equals ud.staff_id orderby u.todo_id descending
                        where u.branch_id == branchID
                        && (0 == userID || u.user_id == userID) && u.row_sta_cd == 1
                        select new ToDoEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            FilePath = "http://highpack-001-site12.dtempurl.com" + u.file_path,
                            ToDoID = u.todo_id,
                            ToDoFileName = u.todo_doc_name,
                            ToDoDate = u.todo_dt,
                            ToDoDescription = u.todo_desc,
                            UserInfo = new UserEntity()
                            {
                                UserID = ud.staff_id,
                                Username = ud.name
                            },
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            return data;
        }

        public async Task<List<ToDoEntity>> GetAllToDoWithoutContentByBranch(long branchID, long userID)
        {
            var data = (from u in this.context.TODO_MASTER
                        .Include("BRANCH_MASTER")
                        join ud in this.context.USER_DEF on u.user_id equals ud.user_id
                        orderby u.todo_id descending
                        where u.branch_id == branchID
                        && (0 == userID || u.user_id == userID) && u.row_sta_cd == 1
                        select new ToDoEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            ToDoID = u.todo_id,
                            ToDoFileName = u.todo_doc_name,
                            ToDoDate = u.todo_dt,
                            ToDoDescription = u.todo_desc,
                            UserInfo = new UserEntity()
                            {
                                UserID = ud.user_id,
                                Username = ud.username
                            },
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public async Task<ToDoEntity> GetToDoByToDoID(long todoID)
        {
            var data = (from u in this.context.TODO_MASTER
                        .Include("BRANCH_MASTER")
                        join ud in this.context.USER_DEF on u.user_id equals ud.staff_id
                        orderby u.todo_id descending
                        where u.todo_id == todoID
                        select new ToDoEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            FilePath = u.file_path,
                            ToDoID = u.todo_id,
                            ToDoFileName = u.todo_doc_name,
                            ToDoDate = u.todo_dt,
                            ToDoDescription = u.todo_desc,
                            Remark=u.remark,
                            Registerstatus=u.reg_status,
                            UserInfo = new UserEntity()
                            {
                                UserID = (long)ud.staff_id,
                                Username = ud.username
                            },
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();
            //if (data != null)
            //{
            //    data.ToDoContentText = data.ToDoContent.Length > 0 ? Convert.ToBase64String(data.ToDoContent) : "";
            //}
            return data;
        }



        public bool RemoveToDo(long todoID, string lastupdatedby)
        {
            var data = (from u in this.context.TODO_MASTER
                        where u.todo_id == todoID
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
