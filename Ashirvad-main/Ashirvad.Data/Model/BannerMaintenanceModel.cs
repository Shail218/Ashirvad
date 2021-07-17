using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class BannerMaintenanceModel
    {
        public BannerEntity BannerInfo { get; set; }
        public List<BannerEntity> BannerData { get; set; }
    }
}
