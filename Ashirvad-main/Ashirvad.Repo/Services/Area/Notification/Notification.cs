using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Notification;
using Ashirvad.Repo.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

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
            notificationMaster.notification_date = notificationInfo.Notification_Date;
            notificationMaster.trans_id = this.AddTransactionData(notificationInfo.Transaction);
            notificationMaster.branch_id = notificationInfo.Branch != null ? (long?)notificationInfo.Branch.BranchID : null;
            notificationMaster.notif_message = notificationInfo.NotificationMessage;
            if (!isUpdate)
            {
                this.context.NOTIFICATION_MASTER.Add(notificationMaster);
            }

            if (this.context.SaveChanges() > 0 || notificationMaster.notif_id > 0)
            {
                var notifID = notificationMaster.notif_id;
                var result = await this.AddNotificationType(notificationInfo.NotificationType, notifID);
                NotifyList(notificationInfo);
                return notifID;
            }
            return 0;
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
                        orderby u.notif_id descending
                        where (0 == branchID || u.branch_id == null || (u.branch_id.HasValue && u.branch_id.Value == branchID) && u.row_sta_cd == 1)
                        select new NotificationEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            NotificationMessage = u.notif_message,
                            NotificationID = u.notif_id,
                            Notification_Date = u.notification_date,
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
                        where (branchID == 0 || u.branch_id == 0 || u.branch_id.Value == branchID)
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
                            Notification_Date = u.notification_date,
                            Branch = new BranchEntity() { BranchID = branch != null ? branch.branch_id : 0, BranchName = branch != null ? branch.branch_name : "All Branch" },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).Distinct().OrderByDescending(a => a.NotificationID).ToList();

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
                            Notification_Date = u.notification_date,
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

        public async Task<List<NotificationEntity>> GetAllCustomNotification(DataTableAjaxPostModel model, long branchID, int typeID)
        {
            var Result = new List<NotificationEntity>();
            bool Isasc = model.order[0].dir == "desc" ? false : true;
            long count = (from u in this.context.NOTIFICATION_MASTER
                          join t in this.context.NOTIFICATION_TYPE_REL on u.notif_id equals t.notif_id
                          where (branchID == 0 || u.branch_id == 0 || u.branch_id == branchID)
                          && (typeID == 0 || t.sub_type_id == typeID) && u.row_sta_cd == 1
                          select new
                          {
                              NotificationID = u.notif_id
                          }).Distinct().Count();
            var data = (from u in this.context.NOTIFICATION_MASTER
                        join t in this.context.NOTIFICATION_TYPE_REL on u.notif_id equals t.notif_id
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id into tempBranch
                        from branch in tempBranch.DefaultIfEmpty()
                        orderby u.notif_id descending
                        where (branchID == 0 || u.branch_id == 0 || u.branch_id.Value == branchID)
                        && (0 == typeID || t.sub_type_id == typeID) && u.row_sta_cd == 1 && (model.search.value == null
                        || model.search.value == ""
                        || u.notif_message.ToLower().Contains(model.search.value)
                        || u.notification_date.ToString().ToLower().Contains(model.search.value))
                        select new NotificationEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            NotificationMessage = u.notif_message,
                            Count = count,
                            NotificationID = u.notif_id,
                            Notification_Date = u.notification_date,
                            Branch = new BranchEntity() { BranchID = branch != null ? branch.branch_id : 0, BranchName = branch != null ? branch.branch_name : "All Branch" },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        })
                        .Distinct()
                        .OrderByDescending(a => a.NotificationID)
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();

            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    string Type = "";
                    var result = this.context.NOTIFICATION_TYPE_REL.Where(z => z.notif_id == item.NotificationID)
                    .Select(y => new NotificationTypeEntity() { ID = y.unique_id, TypeID = y.sub_type_id, TypeText = y.sub_type_id == 1 ? "Admin" : y.sub_type_id == 2 ? "Teacher" : "Student" }).ToList();
                    foreach (var item1 in result)
                    {
                        Type = Type + "-" + item1.TypeText;
                    }
                    item.NotificationTypeText = Type.Substring(1);
                }
            }
            return data;
        }

        public async Task<List<NotificationEntity>> GetAllNotificationforexcel(long branchID)
        {
            var data = (from u in this.context.NOTIFICATION_MASTER.Include("NOTIFICATION_TYPE_REL")
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id into tempBranch
                        from branch in tempBranch.DefaultIfEmpty()
                        orderby u.notif_id descending
                        where (0 == branchID || u.branch_id == null || (u.branch_id.HasValue && u.branch_id.Value == branchID) && u.row_sta_cd == 1)
                        select new NotificationEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            NotificationMessage = u.notif_message,
                            NotificationID = u.notif_id,
                            Notification_Date = u.notification_date,
                            Branch = new BranchEntity() { BranchID = branch != null ? branch.branch_id : 0, BranchName = branch != null ? branch.branch_name : "All Branch" },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    string Type = "";
                    var result = this.context.NOTIFICATION_TYPE_REL.Where(z => z.notif_id == item.NotificationID).Select(y => new NotificationTypeEntity() { ID = y.unique_id, TypeID = y.sub_type_id, TypeText = y.sub_type_id == 1 ? "Admin" : y.sub_type_id == 2 ? "Teacher" : "Student" }).ToList();
                    foreach (var item1 in result)
                    {
                        Type = Type + "-" + item1.TypeText;
                    }
                    item.NotificationTypeText = Type.Substring(1);
                }
            }

            return data;
        }

        public void NotifyList(NotificationEntity notification)
        {
            List<UserEntity> userEntities = new List<UserEntity>();

            try
            {
                List<string> da = new List<string>();
                foreach(var z in notification.NotificationType)
                {
                    da.Add(z.TypeID.ToString());
                }
                if (da.Contains("1"))
                {
                    var admin = this.context.USER_DEF.Where(x => x.fcm_token != null && x.branch_id == notification.Branch.BranchID && x.row_sta_cd == 1 && x.user_type == (int)Enums.UserType.Admin).Select(x => new { x.fcm_token }).ToList();

                    if (admin?.Count > 0)
                    {
                        foreach (var i in admin)
                        {
                           
                            sendNotification(i.fcm_token,notification.NotificationMessage,notification.Notification_Date.ToString("dd/MM/yyyy"));
                        }
                    }
                    
                }
                if ( da.Contains("2"))
                {
                    var teacher = this.context.USER_DEF.Where(x => x.fcm_token != null && x.branch_id == notification.Branch.BranchID && x.row_sta_cd == 1 && x.user_type == (int)Enums.UserType.Staff).Select(x => new { x.fcm_token }).ToList();

                    if (teacher?.Count > 0)
                    {
                        foreach (var i in teacher)
                        {
                            
                            sendNotification(i.fcm_token, notification.NotificationMessage, notification.Notification_Date.ToString("dd/MM/yyyy"));
                        }
                    }
                   
                }
                if ( da.Contains("3"))
                {
                    var student = this.context.USER_DEF.Where(x => x.fcm_token != null && x.branch_id == notification.Branch.BranchID && x.row_sta_cd == 1 && (x.user_type == (int)Enums.UserType.Student || x.user_type == (int)Enums.UserType.Parent)).Select(x => new { x.fcm_token }).ToList();

                    if (student?.Count > 0 ) {
                        foreach (var i in student)
                        {
                          
                            sendNotification(i.fcm_token, notification.NotificationMessage, notification.Notification_Date.ToString("dd/MM/yyyy"));
                        }
                    }
                    
                }

                

            }
            catch (Exception ex)
            {
                //Getconnection.SiteErrorInsert(ex);

            }
        }

        public void sendNotification(string RegId,string msg,string datetext)
        {
            string ApplicationID, SENDER_ID, message, value;
            try
            {
                if (RegId != string.Empty)
                {
                    ApplicationID = "AAAA0nV50YQ:APA91bGsTpKOAt8i9QtOpZrz536HcL1GaGns4HrQ3PBfQ5rB9KJ89um4oxG71ji2laym-SRJwzFBdpPn_3MF6g8D1LPjV4IvJ23oQ240x6lPTe-yXLFWB14Kj7ChIlysjHKpUuYHtxCm";
                    SENDER_ID = "903914049924";

                    value = "Notification";  //title
                    message = "Date- "+datetext+"\n"+ msg; //message

                    string images = "";
                    WebRequest tRequest;
                    tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                    tRequest.Method = "post";
                    tRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                    tRequest.Headers.Add(string.Format("Authorization: key={0}", ApplicationID));
                    tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
                    //Data post to the Server
                    string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.title=" + value + "&data.message=" + message + "&data.img_url=" + images + "&data.page=" + "notification" + "&data.time=" + System.DateTime.Now.ToString() + "&to=" + RegId + "";
                    Console.WriteLine(postData);
                    Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    tRequest.ContentLength = byteArray.Length;
                    Stream dataStream = tRequest.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    //WebResponse tResponse = tRequest.GetResponse();
                    //dataStream = tResponse.GetResponseStream();
                    //StreamReader tReader = new StreamReader(dataStream);
                    //String sResponseFromServer = tReader.ReadToEnd(); //Get response from GCM server
                    //tReader.Close();
                    //dataStream.Close();
                    //tResponse.Close();
                }
            }
            catch(Exception ex)
            {

            }
        
        }



    }
}
