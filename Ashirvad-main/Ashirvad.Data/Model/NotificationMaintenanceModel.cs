using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class NotificationMaintenanceModel
    {
        public NotificationEntity NotificationInfo { get; set; }
        public List<NotificationEntity> NotificationData { get; set; }
    }
}
