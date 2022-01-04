using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ashirvad.Data
{
    public class CircularEntity
    {
        public long CircularId { get; set; }
        public long Count { get; set; }
        public string CircularTitle { get; set; }
        public string CircularDescription { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public byte[] CircularImage { get; set; }
        public string CircularImageText { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
