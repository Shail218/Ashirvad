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
    
    public partial class SUBJECT_MASTER
    {
        public long subject_id { get; set; }
        public string subject { get; set; }
        public long branch_id { get; set; }
        public Nullable<long> subject_dtl_id { get; set; }
        public int row_sta_cd { get; set; }
        public long trans_id { get; set; }
    
        public virtual SUBJECT_DTL_MASTER SUBJECT_DTL_MASTER { get; set; }
        public virtual TRANSACTION_MASTER TRANSACTION_MASTER { get; set; }
        public virtual BRANCH_MASTER BRANCH_MASTER { get; set; }
    }
}
