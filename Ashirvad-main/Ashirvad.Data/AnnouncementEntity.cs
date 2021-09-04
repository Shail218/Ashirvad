using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class AnnouncementEntity
    {
        public long AnnouncementID { get; set; }
        public BranchEntity BranchData { get; set; }
        public TransactionEntity TransactionData { get; set; }
        public RowStatusEntity RowStatusData { get; set; }
        public string AnnouncementText { get; set; }
    }
}
