using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ashirvad.Data
{
    public class BannerEntity
    {
        public long BannerID { get; set; }
        public List<BannerTypeEntity> BannerType { get; set; }
        public BranchEntity BranchInfo { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }
        public byte[] BannerImage { get; set; }
        public string BannerImageText { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public string JSONData { get; set; }
    }

    public class BannerTypeEntity
    {
        public long ID { get; set; }
        public string TypeText { get; set; }
        public int TypeID { get; set; }
    }
}
