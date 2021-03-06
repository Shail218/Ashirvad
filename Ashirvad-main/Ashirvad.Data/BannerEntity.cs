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
        public long Count { get; set; }
        public List<BannerTypeEntity> BannerType { get; set; }
        public string BannerTypeText { get; set; }
        public BranchEntity BranchInfo { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public BannerTypeEntity bannerTypeEntity { get; set; }
        public TransactionEntity Transaction { get; set; }
        public byte[] BannerImage { get; set; }
        public string BannerImageText { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public string JSONData { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int BranchType { get; set; }

    }

    public class BannerTypeEntity
    {
        public long ID { get; set; }
        public string TypeText { get; set; }
        public int TypeID { get; set; }
    }
}
