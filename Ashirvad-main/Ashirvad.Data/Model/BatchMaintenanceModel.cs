using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class BatchMaintenanceModel
    {
        public BatchEntity BatchInfo { get; set; }
        public List<BatchEntity> BatchData { get; set; }
    }
}
