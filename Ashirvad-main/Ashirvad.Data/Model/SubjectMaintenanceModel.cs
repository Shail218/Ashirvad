using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class SubjectMaintenanceModel
    {
        public SubjectEntity SubjectInfo { get; set; }
        public List<SubjectEntity> SubjectData { get; set; }
    }
}
