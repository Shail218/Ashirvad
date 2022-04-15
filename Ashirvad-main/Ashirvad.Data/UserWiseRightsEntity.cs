using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class UserWiseRightsEntity
    {
        public long UserWiseRightsID { get; set; }
        public long UserID { get; set; }
        public RoleRightsEntity RoleRightinfo { get; set; } = new RoleRightsEntity();
        public RoleEntity Roleinfo { get; set; } = new RoleEntity();
        public UserEntity userinfo { get; set; } = new UserEntity();
        public BranchEntity branchinfo { get; set; } = new BranchEntity();
        public bool Createstatus { get; set; }
        public bool Editstatus { get; set; }
        public bool Deletestatus { get; set; }
        public bool Viewstatus { get; set; }
        public string JasonData { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public PageEntity PageInfo { get; set; } = new PageEntity();
        public List<UserWiseRightsEntity> list { get; set; } = new List<UserWiseRightsEntity>();
        public List<PageEntity> PageList { get; set; } = new List<PageEntity>();
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public long Count { get; set; }
    }
}
