using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class StandardMaintenanceModel
    {
        public StandardEntity StandardInfo { get; set; }
        public List<StandardEntity> StandardData { get; set; }
    }
}
