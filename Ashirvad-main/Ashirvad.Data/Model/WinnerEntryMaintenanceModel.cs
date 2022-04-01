using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
     public class WinnerEntryMaintenanceModel
    {
        public WinnerEntryEntity WinnerEntryInfo { get; set; }
        public List<WinnerEntryEntity> WinnerEntryData { get; set; }
    }
}
