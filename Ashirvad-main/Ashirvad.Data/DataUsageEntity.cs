using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class DataUsageEntity
    {
        public long BranchID { get; set; }
        public decimal? Usage { get; set; }
        public string Identifier { get; set; }
    }
}
