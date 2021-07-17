using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class GalleryMaintenanceModel
    {
        public GalleryEntity GalleryInfo { get; set; }
        public List<GalleryEntity> GalleryData { get; set; }
    }
}
