using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.ToDo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area.ToDo
{
    public class ToDo : ModelAccess, IToDoAPI
    {
        public async Task<ResponseModel> ToDoMaintenance(ToDoEntity todoInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            Model.TODO_MASTER todo = new Model.TODO_MASTER();
            bool isUpdate = true;
            try
            {
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
                var da = this.context.SaveChanges() > 0 ? todo.todo_id : 0;
                if (da > 0)
                {
                    todoInfo.ToDoID = da;
                    //responseModel.Data = todoInfo;
                    responseModel.Message = isUpdate == true ? "ToDoList Updated Successfully." : "ToDoList Inserted Successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.Message = isUpdate == true ? "ToDoList Not Updated." : "ToDoList Not Inserted.";
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
                            FilePath = "https://mastermind.org.in" + u.file_path,
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

        public async Task<List<ToDoEntity>> GetAllToDoList(long branchid)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            var ToDayDate = indianTime.ToString("yyyy/MM/dd");
            DateTime dt = DateTime.ParseExact(ToDayDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var data = (from u in this.context.TODO_MASTER
                        .Include("BRANCH_MASTER")
                        join ud in this.context.BRANCH_STAFF on u.user_id equals ud.staff_id
                        orderby u.todo_id descending
                        where u.branch_id == branchid && u.row_sta_cd == 1 && u.todo_dt == dt
                        select new ToDoEntity()
                        {
                            FilePath = "https://mastermind.org.in" + u.file_path,
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

        public async Task<List<ToDoEntity>> GetAllCustomToDo(DataTableAjaxPostModel model, long branchID)
        {
            var Result = new List<ToDoEntity>();
            bool Isasc = model.order[0].dir == "desc" ? false : true;
            long count = (from u in this.context.TODO_MASTER.Include("BRANCH_MASTER")
                          join ud in this.context.BRANCH_STAFF on u.user_id equals ud.staff_id orderby u.todo_id descending
                          where u.branch_id == branchID && u.row_sta_cd == 1
                          select new ToDoEntity()
                          {
                              ToDoID = u.todo_id
                          }).Distinct().Count();
            var data = (from u in this.context.TODO_MASTER .Include("BRANCH_MASTER")
                        join ud in this.context.BRANCH_STAFF on u.user_id equals ud.staff_id
                        where u.branch_id == branchID && u.row_sta_cd == 1
                        && (model.search.value == null
                        || model.search.value == ""
                        || u.todo_dt.ToString().ToLower().Contains(model.search.value)
                        || u.todo_desc.ToLower().Contains(model.search.value)
                        || ud.name.ToLower().Contains(model.search.value))
                        orderby u.todo_id descending
                        select new ToDoEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            FilePath = "https://mastermind.org.in" + u.file_path,
                            ToDoID = u.todo_id,
                            ToDoFileName = u.todo_doc_name,
                            ToDoDate = u.todo_dt,
                            ToDoDescription = u.todo_desc,
                            UserInfo = new UserEntity()
                            {
                                UserID = ud.staff_id,
                                Username = ud.name
                            },
                            Count = count,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        })
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();
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

        public ResponseModel RemoveToDo(long todoID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var data = (from u in this.context.TODO_MASTER
                            where u.todo_id == todoID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                    responseModel.Message = "ToDoList Removed Successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.Message = "TodoList Not Found.";
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
    }
}
