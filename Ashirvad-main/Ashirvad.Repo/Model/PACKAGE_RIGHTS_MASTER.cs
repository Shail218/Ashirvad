//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ashirvad.Repo.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class PACKAGE_RIGHTS_MASTER
    {
        public long packagerights_id { get; set; }
        public long page_id { get; set; }
        public long package_id { get; set; }
        public Nullable<int> row_sta_cd { get; set; }
        public string about_us { get; set; }
        public string contact_no { get; set; }
        public string mobile_no { get; set; }
        public string email_id { get; set; }
        public long trans_id { get; set; }
    
        public virtual PACKAGE_MASTER PACKAGE_MASTER { get; set; }
        public virtual PAGE_MASTER PAGE_MASTER { get; set; }
        public virtual TRANSACTION_MASTER TRANSACTION_MASTER { get; set; }
    }
}
