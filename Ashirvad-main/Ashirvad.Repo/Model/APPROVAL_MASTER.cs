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
    
    public partial class APPROVAL_MASTER
    {
        public long approval_id { get; set; }
        public long library_id { get; set; }
        public long branch_id { get; set; }
        public long trans_id { get; set; }
        public int row_sta_cd { get; set; }
        public int library_status { get; set; }
    
        public virtual LIBRARY_MASTER LIBRARY_MASTER { get; set; }
        public virtual TRANSACTION_MASTER TRANSACTION_MASTER { get; set; }
        public virtual BRANCH_MASTER BRANCH_MASTER { get; set; }
    }
}
