using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class NotificationEntity
    {
        public long NotificationID { get; set; }
        public List<NotificationTypeEntity> NotificationType { get; set; }
        public BranchEntity Branch { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }
        public string NotificationMessage { get; set; }
        public DateTime Notification_Date { get; set; }
        public int BranchType { get; set; }
        public string JSONData { get; set; }
        public string NotificationTypeText { get; set; }
        public long Count { get; set; }
        public BranchCourseEntity BranchCourse { get; set; }
        public string StandardArray { get; set; }
        public string StandardNameArray { get; set; }
        public List<NotificationStandardEntity> list { get; set; } = new List<NotificationStandardEntity>();
        public List<StandardEntity> Standardlist { get; set; } = new List<StandardEntity>();
        public string stdID { get; set; }
    }
    public class NotificationStandardEntity
    {
        public long notification_std_id { get; set; }
        public long std_id { get; set; }
        public long sub_id { get; set; }
        public string standard { get; set; }
        public string subject { get; set; }
        public long notif_id { get; set; }
        public BranchCourseEntity BranchCourse { get; set; }
    }
    public class NotificationTypeEntity
    {
        public long ID { get; set; }
        public string TypeText { get; set; }
        public int TypeID { get; set; }
    }
}
