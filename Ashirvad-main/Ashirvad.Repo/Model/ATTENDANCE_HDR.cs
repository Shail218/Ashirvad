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
    
    public partial class ATTENDANCE_HDR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ATTENDANCE_HDR()
        {
            this.ATTENDANCE_DTL = new HashSet<ATTENDANCE_DTL>();
        }
    
        public long attendance_hdr_id { get; set; }
        public long branch_id { get; set; }
        public long std_id { get; set; }
        public int batch_time_type { get; set; }
        public System.DateTime attendance_dt { get; set; }
        public long trans_id { get; set; }
        public int row_sta_cd { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ATTENDANCE_DTL> ATTENDANCE_DTL { get; set; }
        public virtual STD_MASTER STD_MASTER { get; set; }
        public virtual BRANCH_MASTER BRANCH_MASTER { get; set; }
    }
}
