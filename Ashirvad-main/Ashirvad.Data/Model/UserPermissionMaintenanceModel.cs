using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class UserPermissionMaintenanceModel
    {
        public UserEntity UserInfo { get; set; }
        public List<UserEntity> UserData { get; set; }
        public Dictionary<string, int> RolesData { get; set; }
    }
}
