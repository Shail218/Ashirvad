using Ashirvad.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class UserEntity
    {
        public long UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientSecret { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }
        public Common.Enums.UserType UserType { get; set; }
        public string UserTypeText { get; set; }
        public long? StudentID { get; set; }
        public long? ParentID { get; set; }
        public long? StaffID { get; set; }
        public BranchEntity BranchInfo { get; set; }
        public StudentEntity StudentDetail { get; set; }
        public StudentMaint ParentDetail { get; set; }
        public StaffEntity StaffDetail { get; set; }
        public List<RolesEntity>  Roles { get; set; }
        public string JSONData { get; set; }
        public string fcm_token { get; set; }
        public List<BranchWiseRightEntity> Permission { get; set; }
    }

    public class RolesEntity
    {
        public long RoleID { get; set; }
        public long UserID { get; set; }
        public Enums.Roles Permission { get; set; }
        public string RoleName { get; set; }
        public bool HasAccess{ get; set; }
        public int PermissionValue{ get; set; }
    }
}
