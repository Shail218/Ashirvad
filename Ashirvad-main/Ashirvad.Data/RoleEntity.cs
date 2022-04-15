using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
   public class RoleEntity
    {
        public long RoleID { get; set; }
        public int Count { get; set; }
        public string RoleName { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public BranchEntity BranchInfo { get; set; }
    }
}
