using Ashirvad.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class RowStatusEntity
    {
        public int RowStatusId { get; set; }
        public string RowStatusText { get; set; }
        public Enums.RowStatus RowStatus { get; set; }
    }
}
