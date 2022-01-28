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
    
    public partial class STD_MASTER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public STD_MASTER()
        {
            this.LINK_MASTER = new HashSet<LINK_MASTER>();
            this.LIBRARY_STD_MASTER = new HashSet<LIBRARY_STD_MASTER>();
            this.HOMEWORK_MASTER = new HashSet<HOMEWORK_MASTER>();
        }
    
        public long std_id { get; set; }
        public long branch_id { get; set; }
        public string standard { get; set; }
        public Nullable<long> class_dtl_id { get; set; }
        public int row_sta_cd { get; set; }
        public long trans_id { get; set; }
    
        public virtual BRANCH_MASTER BRANCH_MASTER { get; set; }
        public virtual CLASS_DTL_MASTER CLASS_DTL_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LINK_MASTER> LINK_MASTER { get; set; }
        public virtual TRANSACTION_MASTER TRANSACTION_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LIBRARY_STD_MASTER> LIBRARY_STD_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOMEWORK_MASTER> HOMEWORK_MASTER { get; set; }
    }
}
