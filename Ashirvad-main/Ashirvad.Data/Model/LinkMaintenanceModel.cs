using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class LinkMaintenanceModel
    {
        public LinkEntity LinkInfo { get; set; }
        public List<LinkEntity> LinkData { get; set; }
    }
}
