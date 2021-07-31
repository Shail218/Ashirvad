using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Notification;
using Ashirvad.Repo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Notification
{
    public class Notification : ModelAccess, INotificationAPI
    {
        public async Task<long> NotificationMaintenance(NotificationEntity notificationInfo)
        {
            Model.NOTIFICATION_MASTER notificationMaster = new Model.NOTIFICATION_MASTER();
            bool isUpdate = true;
            var data = (from notif in this.context.NOTIFICATION_MASTER
                        where notif.notif_id == notificationInfo.NotificationID
                        select notif).FirstOrDefault();
            if (data == null)
            {
                data = new Model.NOTIFICATION_MASTER();
                isUpdate = false;
            }
            else
            {
                notificationMaster = data;
                notificationInfo.Transaction.TransactionId = data.trans_id;
            }

            notificationMaster.row_sta_cd = notificationInfo.RowStatus.RowStatusId;
            notificationMaster.trans_id = this.AddTransactionData(notificationInfo.Transaction);
            notificationMaster.branch_id = notificationInfo.Branch != null ? (long?)notificationInfo.Branch.BranchID : null;
            notificationMaster.notif_message = notificationInfo.NotificationMessage;
            if (!isUpdate)
            {
                this.context.NOTIFICATION_MASTER.Add(notificationMaster);
            }

            var notifID = this.context.SaveChanges() > 0 ? notificationMaster.notif_id : 0;
            var result = await this.AddNotificationType(notificationInfo.NotificationType, notifID);
            return notifID;
        }

        public async Task<bool> AddNotificationType(List<NotificationTypeEntity> notifType, long notifID)
        {
            var associatedData = (from i in this.context.NOTIFICATION_TYPE_REL
                                  where i.notif_id == notifID
                                  select i).ToList();
            if (associatedData?.Count > 0)
            {
                this.context.NOTIFICATION_TYPE_REL.RemoveRange(associatedData);
            }

            foreach (var item in notifType)
            {
                NOTIFICATION_TYPE_REL notifTypeRec = new NOTIFICATION_TYPE_REL()
                {
                    notif_id = notifID,
                    sub_type_id = item.TypeID
                };
                this.context.NOTIFICATION_TYPE_REL.Add(notifTypeRec);
            }

            return this.context.SaveChanges() > 0;
        }

        public async Task<List<NotificationEntity>> GetAllNotification(long branchID)
        {
            var data = (from u in this.context.NOTIFICATION_MASTER.Include("NOTIFICATION_TYPE_REL")
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id into tempBranch
                        from branch in tempBranch.DefaultIfEmpty()
                        where (0 == branchID || u.branch_id == null || u.branch_id == 0 || (u.branch_id.HasValue && u.branch_id.Value == branchID) && u.row_sta_cd == 1)
                        select new NotificationEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            NotificationMessage = u.notif_message,
                            NotificationID = u.notif_id,
                            Branch = new BranchEntity() { BranchID = branch != null ? branch.branch_id : 0, BranchName = branch != null ? branch.branch_name : "All Branch" },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    int idx = data.IndexOf(item);
                    data[idx].NotificationType = this.context.NOTIFICATION_TYPE_REL.Where(z => z.notif_id == item.NotificationID).Select(y => new NotificationTypeEntity() { ID = y.unique_id, TypeID = y.sub_type_id, TypeText = y.sub_type_id == 1 ? "Admin" : y.sub_type_id == 2 ? "Teacher" : "Student" }).ToList();
                }
            }

            return data;
        }

        public async Task<List<NotificationEntity>> GetAllNotification(long branchID, int typeID)
        {
            var data = (from u in this.context.NOTIFICATION_MASTER
                        join t in this.context.NOTIFICATION_TYPE_REL on u.notif_id equals t.notif_id
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id into tempBranch
                        from branch in tempBranch.DefaultIfEmpty()
                        where (0 == branchID || u.branch_id == 0 || u.branch_id == null || u.branch_id.Value == branchID)
                        && (0 == typeID || t.sub_type_id == typeID) && u.row_sta_cd == 1
                        select new NotificationEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            NotificationMessage = u.notif_message,
                            NotificationID = u.notif_id,
                            Branch = new BranchEntity() { BranchID = branch != null ? branch.branch_id : 0, BranchName = branch != null ? branch.branch_name : "All Branch" },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).Distinct().ToList();

            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    int idx = data.IndexOf(item);
                    data[idx].NotificationType = this.context.NOTIFICATION_TYPE_REL.Where(z => z.notif_id == item.NotificationID).Select(y => new NotificationTypeEntity() { ID = y.unique_id, TypeID = y.sub_type_id, TypeText = y.sub_type_id == 1 ? "Admin" : y.sub_type_id == 2 ? "Teacher" : "Student" }).ToList();
                }
            }

            return data;
        }

        public async Task<NotificationEntity> GetNotificationByNotificationID(long notificationID)
        {
            var data = (from u in this.context.NOTIFICATION_MASTER.Include("NOTIFICATION_TYPE_REL")
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id into tempBranch
                        from branch in tempBranch.DefaultIfEmpty()
                        where u.notif_id == notificationID
                        select new NotificationEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            NotificationMessage = u.notif_message,
                            NotificationID = u.notif_id,
                            Branch = u.branch_id != null ? new BranchEntity() { BranchID = u.branch_id.Value, BranchName = branch != null ? branch.branch_name : "" } : null,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();

            if (data != null)
            {
                data.NotificationType = this.context.NOTIFICATION_TYPE_REL.Where(z => z.notif_id == data.NotificationID).Select(y => new NotificationTypeEntity() { ID = y.unique_id, TypeID = y.sub_type_id, TypeText = y.sub_type_id == 1 ? "Admin" : y.sub_type_id == 2 ? "Teacher" : "Student" }).ToList();
            }

            return data;
        }

        public bool RemoveNotification(long notifID, string lastupdatedby)
        {
            var data = (from u in this.context.NOTIFICATION_MASTER
                        where u.notif_id == notifID
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
