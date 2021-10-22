using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
   public class PackageRightEntity
    {
        public long PackageRightsId { get; set; }
        public PackageEntity Packageinfo { get; set; } = new PackageEntity();
        public bool Createstatus { get; set; }
        public bool Editstatus { get; set; }
        public bool Deletestatus { get; set; }
        public bool Viewstatus { get; set; }
        public string JasonData { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public PageEntity PageInfo { get; set; } = new PageEntity();
        public List<PackageRightEntity> list { get; set; } = new List<PackageRightEntity>();
        public List<PageEntity> PageList { get; set; } = new List<PageEntity>();
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public BranchEntity BranchInfo { get; set; }
    }
}
