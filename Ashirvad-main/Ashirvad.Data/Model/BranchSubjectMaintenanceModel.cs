using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class BranchSubjectMaintenanceModel
    {
        public BranchSubjectEntity BranchSubjectInfo { get; set; }
        public List<BranchSubjectEntity> BranchSubjectData { get; set; } = new List<BranchSubjectEntity>();
    }
}
