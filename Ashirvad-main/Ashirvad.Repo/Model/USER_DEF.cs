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
    
    public partial class USER_DEF
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USER_DEF()
        {
            this.USER_ROLE = new HashSet<USER_ROLE>();
            this.USER_RIGHTS_MASTER = new HashSet<USER_RIGHTS_MASTER>();
        }
    
        public long user_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int row_sta_cd { get; set; }
        public long trans_id { get; set; }
        public Nullable<long> staff_id { get; set; }
        public long branch_id { get; set; }
        public int user_type { get; set; }
        public Nullable<long> student_id { get; set; }
        public Nullable<long> parent_id { get; set; }
        public string fcm_token { get; set; }
        public string mobile_no { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USER_ROLE> USER_ROLE { get; set; }
        public virtual BRANCH_STAFF BRANCH_STAFF { get; set; }
        public virtual TRANSACTION_MASTER TRANSACTION_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USER_RIGHTS_MASTER> USER_RIGHTS_MASTER { get; set; }
    }
}
