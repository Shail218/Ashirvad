using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class LinkEntity
    {
        public long UniqueID { get; set; }
        public BranchEntity Branch { get; set; }
        public long StandardID { get; set; }
        public string StandardName { get; set; }
        public string LinkDesc { get; set; }
        public string LinkURL { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }
        public string Title { get; set; }
        public int LinkType { get; set; }
        public long Count { get; set; }
    }
}
