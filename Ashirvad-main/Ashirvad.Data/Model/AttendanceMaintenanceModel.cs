using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class AttendanceMaintenanceModel
    {
        public AttendanceEntity AttendanceInfo { get; set; }
        public List<StudentEntity> StudentData { get; set; }
        public List<AttendanceEntity> AttendanceData { get; set; }
    }
}
