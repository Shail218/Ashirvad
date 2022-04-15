using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
   public class UserWiseRightMaintenanceModel
    {
        public UserWiseRightsEntity UserWiseRightInfo { get; set; }
        public List<UserWiseRightsEntity> BranchWiseRightData { get; set; }
    }
}
