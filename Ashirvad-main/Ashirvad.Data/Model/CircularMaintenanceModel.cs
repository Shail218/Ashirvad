using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class CircularMaintenanceModel
    {
        public CircularEntity CircularEntity { get; set; }
        public List<CircularEntity> CircularEntitiesData { get; set; }
    }
}
