using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class RoleMaintenanceModel
    {
        public RoleEntity RoleInfo { get; set; } = new RoleEntity();
        public List<RoleEntity> RoleData { get; set; } = new List<RoleEntity>();
    }
}
