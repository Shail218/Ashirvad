using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Reminder;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area.Reminder
{
    public class Reminder : ModelAccess, IReminderAPI
    {
        public async Task<long> ReminderMaintenance(ReminderEntity reminderInfo)
        {
            Model.REMINDER_MASTER reminderMaster = new Model.REMINDER_MASTER();
            bool isUpdate = true;
            var data = (from notif in this.context.REMINDER_MASTER
                        where notif.reminder_id == reminderInfo.ReminderID
                        select notif).FirstOrDefault();
            if (data == null)
            {
                data = new Model.REMINDER_MASTER();
                isUpdate = false;
            }
            else
            {
                reminderMaster = data;
                reminderInfo.Transaction.TransactionId = data.trans_id;
            }

            reminderMaster.row_sta_cd = reminderInfo.RowStatus.RowStatusId;
            reminderMaster.trans_id = this.AddTransactionData(reminderInfo.Transaction);
            reminderMaster.branch_id = reminderInfo.BranchInfo.BranchID;
            reminderMaster.reminder_desc = reminderInfo.ReminderDesc;
            reminderMaster.reminder_dt = reminderInfo.ReminderDate;
            reminderMaster.reminder_time = reminderInfo.ReminderTime;
            reminderMaster.user_id = reminderInfo.UserID;
            if (!isUpdate)
            {
                this.context.REMINDER_MASTER.Add(reminderMaster);
            }

            var reminderID = this.context.SaveChanges() > 0 ? reminderMaster.reminder_id : 0;
            return reminderID;
        }
        
        public async Task<List<ReminderEntity>> GetAllRemindersByBranch(long branchID, long userID)
        {
            var data = (from u in this.context.REMINDER_MASTER.Include("BRANCH_MASTER")
                        join ud in this.context.USER_DEF on u.user_id equals ud.user_id
                        orderby u.reminder_id descending
                        where u.branch_id == branchID
                        && (0 == userID || u.user_id == userID) && u.row_sta_cd == 1
                        select new ReminderEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            ReminderDesc = u.reminder_desc,
                            ReminderID = u.reminder_id,
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            ReminderDate = u.reminder_dt,
                            ReminderTime = u.reminder_time,
                            UserID = u.user_id,
                            Username = ud.username
                        }).ToList();

            return data;
        }

        public async Task<List<ReminderEntity>> GetAllReminderList(long branchid)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            var ToDayDate = indianTime.ToString("yyyy-MM-dd");
            DateTime dt = DateTime.ParseExact(ToDayDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var data = (from u in this.context.REMINDER_MASTER.Include("BRANCH_MASTER")
                        join ud in this.context.USER_DEF on u.user_id equals ud.user_id
                        orderby u.reminder_id descending
                        where u.branch_id == branchid && u.row_sta_cd == 1 && u.reminder_dt == dt
                        select new ReminderEntity()
                        {
                            ReminderDesc = u.reminder_desc,
                            ReminderID = u.reminder_id,
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            ReminderDate = u.reminder_dt,
                            ReminderTime = u.reminder_time,
                            UserID = u.user_id,
                            Username = ud.username
                        }).ToList();

            return data;
        }

        public async Task<List<ReminderEntity>> GetAllCustomReminder(DataTableAjaxPostModel model, long branchID)
        {
            var Result = new List<ReminderEntity>();
            bool Isasc = model.order[0].dir == "desc" ? false : true;
            long count = this.context.REMINDER_MASTER.Where(s => s.row_sta_cd == 1 && s.branch_id == branchID).Count();
            var data = (from u in this.context.REMINDER_MASTER.Include("BRANCH_MASTER")
                        join ud in this.context.USER_DEF on u.user_id equals ud.user_id
                        where u.branch_id == branchID && u.row_sta_cd == 1
                        && (model.search.value == null
                        || model.search.value == ""
                        || u.reminder_desc.ToLower().Contains(model.search.value)
                        || u.reminder_dt.ToString().ToLower().Contains(model.search.value))
                        orderby u.reminder_id descending
                        select new ReminderEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            ReminderDesc = u.reminder_desc,
                            ReminderID = u.reminder_id,
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            ReminderDate = u.reminder_dt,
                            ReminderTime = u.reminder_time,
                            UserID = u.user_id,
                            Username = ud.username,
                            Count = count
                        })
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();
            return data;
        }

        public async Task<ReminderEntity> GetReminderByID(long reminderID)
        {
            var data = (from u in this.context.REMINDER_MASTER.Include("BRANCH_MASTER")
                        join ud in this.context.USER_DEF on u.user_id equals ud.user_id
                        where u.reminder_id == reminderID
                        select new ReminderEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            ReminderDesc = u.reminder_desc,
                            ReminderID = u.reminder_id,
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            ReminderDate = u.reminder_dt,
                            ReminderTime = u.reminder_time,
                            UserID = u.user_id,
                            Username = ud.username
                        }).FirstOrDefault();

            return data;
        }

        public bool RemoveReminder(long reminderID, string lastupdatedby)
        {
            var data = (from u in this.context.REMINDER_MASTER
                        where u.reminder_id == reminderID
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
