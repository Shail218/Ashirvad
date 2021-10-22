using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
   public class PackageMaintenanceModel
    {
        public PackageEntity PackageInfo { get; set; }
        public List<PackageEntity> PackageData { get; set; }
    }
}
