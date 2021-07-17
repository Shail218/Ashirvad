using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class UserMaintenanceModel
    {
        public StaffEntity StaffInfo { get; set; }
        public List<StaffEntity> StaffData { get; set; }
    }
}
