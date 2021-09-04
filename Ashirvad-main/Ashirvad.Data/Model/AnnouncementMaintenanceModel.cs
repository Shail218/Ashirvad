using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class AnnouncementMaintenanceModel
    {
        public AnnouncementEntity AnnouncementInfo { get; set; }
        public List<AnnouncementEntity> AnnouncementData { get; set; }
    }
}
