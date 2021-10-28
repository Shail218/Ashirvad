using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
   public class BranchWiseRightEntity
    {
        public long BranchWiseRightsID { get; set; }
        public long BranchID { get; set; }
        public PackageRightEntity PackageRightinfo { get; set; } = new PackageRightEntity();
        public PackageEntity Packageinfo { get; set; } = new PackageEntity();
        public BranchEntity branchinfo { get; set; } = new BranchEntity();
        public bool Createstatus { get; set; }
        public bool Editstatus { get; set; }
        public bool Deletestatus { get; set; }
        public bool Viewstatus { get; set; }
        public string JasonData { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public PageEntity PageInfo { get; set; } = new PageEntity();
        public List<BranchWiseRightEntity> list { get; set; } = new List<BranchWiseRightEntity>();
        public List<PageEntity> PageList { get; set; } = new List<PageEntity>();
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
    }
}
