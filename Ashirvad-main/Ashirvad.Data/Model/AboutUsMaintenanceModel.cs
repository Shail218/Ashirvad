using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class AboutUsMaintenanceModel
    {
        public AboutUsEntity AboutusInfo { get; set; } = new AboutUsEntity();
        public List<AboutUsEntity> AboutusData { get; set; }
        public AboutUsDetailEntity detailInfo { get; set; }
        public List<AboutUsDetailEntity> detailData { get; set; }
    }
}
