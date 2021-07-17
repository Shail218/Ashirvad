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
    }

    public class NotificationTypeEntity
    {
        public long ID { get; set; }
        public string TypeText { get; set; }
        public int TypeID { get; set; }
    }
}
