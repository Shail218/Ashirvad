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
    
    public partial class PAGE_MASTER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PAGE_MASTER()
        {
            this.PACKAGE_RIGHTS_MASTER = new HashSet<PACKAGE_RIGHTS_MASTER>();
            this.ROLE_RIGHTS_MASTER = new HashSet<ROLE_RIGHTS_MASTER>();
        }
    
        public long page_id { get; set; }
        public string page { get; set; }
        public long branch_id { get; set; }
        public int row_sta_cd { get; set; }
        public long trans_id { get; set; }
    
        public virtual BRANCH_MASTER BRANCH_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PACKAGE_RIGHTS_MASTER> PACKAGE_RIGHTS_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ROLE_RIGHTS_MASTER> ROLE_RIGHTS_MASTER { get; set; }
    }
}
