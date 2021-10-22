using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
   public class PageMaintenanceModel
    {
        public PageEntity PageInfo { get; set; }
        public List<PageEntity> PageData { get; set; }
    }
}
