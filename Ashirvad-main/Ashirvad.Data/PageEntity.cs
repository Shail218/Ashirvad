using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
  public  class PageEntity
    {
        public long PageID { get; set; }
        public long Count { get; set; }
        public string Page { get; set; }
        public bool Createstatus { get; set; }
        public bool Editstatus { get; set; }
        public bool Deletestatus { get; set; }
        public bool Viewstatus { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public BranchEntity BranchInfo { get; set; }
    }
}
