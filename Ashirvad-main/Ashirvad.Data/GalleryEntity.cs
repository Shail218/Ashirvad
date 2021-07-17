using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ashirvad.Data
{
    public class GalleryEntity
    {
        public long UniqueID { get; set; }
        public BranchEntity Branch { get; set; }
        public byte[] FileInfo { get; set; }
        public string FileEncoded { get; set; }
        public string Remarks { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }
        public int GalleryType { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
