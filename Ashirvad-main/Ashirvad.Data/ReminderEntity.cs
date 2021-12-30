using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class ReminderEntity
    {
        public long ReminderID { get; set; }
        public BranchEntity BranchInfo { get; set; }
        public long UserID { get; set; }
        public string Username { get; set; }
        public DateTime ReminderDate { get; set; }
        public string ReminderTime { get; set; }
        public string ReminderDesc { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public long Count { get; set; }
    }
}
