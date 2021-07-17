using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class PaperMaintenanceModel
    {
        public PaperEntity PaperInfo { get; set; }
        public List<PaperEntity> PaperData { get; set; }
    }
}
