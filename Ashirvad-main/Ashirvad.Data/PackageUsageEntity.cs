using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class PackageUsageEntity
    {
        public int activeStudent { get; set; }
        public int inActiveStudent { get; set; }
        public long? totalPackageSize { get; set; }
        public string packageType { get; set; }
    }
}
