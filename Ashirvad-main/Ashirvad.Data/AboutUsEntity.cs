using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ashirvad.Data
{
    public class AboutUsEntity
    {
        public long AboutUsID { get; set; }
        public string AboutUsDesc { get; set; }
        public string WebsiteURL { get; set; }
        public string EmailID { get; set; }
        public string ContactNo { get; set; }
        public string WhatsAppNo { get; set; }
        public HttpPostedFileBase FileInfo { get; set; }
        public string HeaderImageText { get; set; }
        public string HeaderImageName { get; set; }
        public byte[] HeaderImage { get; set; }
        public BranchEntity BranchInfo { get; set; }
        public TransactionEntity TransactionInfo { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public AboutUsDetailEntity detailEntity { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public string FilePath { get; set; }
    }

    public class AboutUsDetailEntity
    {
        public BranchEntity BranchInfo { get; set; }
        public long DetailID { get; set; }
        public AboutUsEntity AboutUsInfo { get; set; }
        public string BrandName { get; set; }
        public HttpPostedFileBase FileInfo { get; set; }
        public string HeaderImageText { get; set; }
        public string FilePath { get; set; }
        public byte[] HeaderImage { get; set; }
        public TransactionEntity TransactionInfo { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public HttpPostedFileBase[] ImageFile { get; set; }

    }
}
