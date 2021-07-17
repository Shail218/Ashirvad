using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class ReminderMaintenanceModel
    {
        public ReminderEntity ReminderInfo { get; set; }
        public List<ReminderEntity> ReminderData { get; set; }
    }
}
