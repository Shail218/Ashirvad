using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class BranchClassMaintenanceModel
    {
        public BranchClassEntity BranchClassInfo { get; set; }
        public List<BranchClassEntity> BranchClassData { get; set; } = new List<BranchClassEntity>();
    }
}
