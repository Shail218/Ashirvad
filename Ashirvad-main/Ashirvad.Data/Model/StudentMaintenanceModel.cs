using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class StudentMaintenanceModel
    {
        public StudentEntity StudentInfo { get; set; } = new StudentEntity();
        public List<StudentEntity> StudentData { get; set; } = new List<StudentEntity>();
    }
}
