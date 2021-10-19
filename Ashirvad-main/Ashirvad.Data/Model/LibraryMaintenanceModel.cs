using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class LibraryMaintenanceModel
    {
        public LibraryEntity LibraryInfo { get; set; }
        public List<LibraryEntity> LibraryData { get; set; }
    }
    public class LibraryNewMaintenanceModel
    {
        public LibraryEntity1 LibraryInfo { get; set; }
        public List<LibraryEntity1> LibraryData { get; set; }
    }
}
