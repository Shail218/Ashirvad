using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class CourseMaintenanceModel
    {
        public CourseEntity CourseInfo { get; set; }
        public List<CourseEntity> CourseData { get; set; }
    }
}
