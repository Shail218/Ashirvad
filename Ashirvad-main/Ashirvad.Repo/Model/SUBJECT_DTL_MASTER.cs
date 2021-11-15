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
    
    public partial class SUBJECT_DTL_MASTER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SUBJECT_DTL_MASTER()
        {
            this.SUBJECT_MASTER = new HashSet<SUBJECT_MASTER>();
        }
    
        public long subject_dtl_id { get; set; }
        public long branch_id { get; set; }
        public long subject_id { get; set; }
        public long course_dtl_id { get; set; }
        public long class_dtl_id { get; set; }
        public Nullable<bool> is_subject { get; set; }
        public Nullable<int> row_sta_cd { get; set; }
        public long trans_id { get; set; }
    
        public virtual BRANCH_MASTER BRANCH_MASTER { get; set; }
        public virtual CLASS_DTL_MASTER CLASS_DTL_MASTER { get; set; }
        public virtual SUBJECT_BRANCH_MASTER SUBJECT_BRANCH_MASTER { get; set; }
        public virtual TRANSACTION_MASTER TRANSACTION_MASTER { get; set; }
        public virtual COURSE_DTL_MASTER COURSE_DTL_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SUBJECT_MASTER> SUBJECT_MASTER { get; set; }
    }
}
