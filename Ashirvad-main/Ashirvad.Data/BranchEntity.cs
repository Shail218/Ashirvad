using Ashirvad.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ashirvad.Data
{
    public class BranchEntity
    {
        public BranchEntity()
        {
            this.RowStatus = new RowStatusEntity();
            this.BranchMaint = new BranchMaint();
            this.Transaction = new TransactionEntity();
        }
        public long BranchID { get; set; }
        public long Count { get; set; }
        public string BranchName { get; set; }
        public string AboutUs { get; set; }
        public string ContactNo { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public BranchMaint BranchMaint { get; set; }
        public string ImagePath { get; set; }
        public int BranchType { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }

        public Enums.BoardType board { get; set; }
        public string boardtype { get; set; }
        public string aliasName { get; set; }
    }

    public class BranchMaint
    {
        public long BranchId { get; set; }
        public bool HasImage { get; set; }
        public byte[] BranchLogo { get; set; }
        public string BranchLogoExt { get; set; }
        public string HeaderLogoExt { get; set; }
        public byte[] HeaderLogo { get; set; }
        public string Website { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
}
