using Ashirvad.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class StaffEntity : ResponseModel
    {
        public long StaffID { get; set; }
        public string Name { get; set; }
        public string Education { get; set; }
        public DateTime? DOB { get; set; }
        public Enums.Gender Gender { get; set; }
        public Enums.UserType Userrole { get; set; }
        public string Address { get; set; }
        public DateTime? ApptDT  { get; set; }
        public DateTime? JoinDT { get; set; }
        public DateTime? LeavingDT { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public long UserID { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public BranchEntity BranchInfo { get; set; }
    }
}
