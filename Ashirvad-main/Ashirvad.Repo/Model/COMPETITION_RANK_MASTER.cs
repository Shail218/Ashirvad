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
    
    public partial class COMPETITION_RANK_MASTER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COMPETITION_RANK_MASTER()
        {
            this.COMPETITION_WINNER_MASTER = new HashSet<COMPETITION_WINNER_MASTER>();
        }
    
        public long competition_rank_id { get; set; }
        public System.DateTime rank_dt { get; set; }
        public long competition_id { get; set; }
        public long branch_id { get; set; }
        public long student_id { get; set; }
        public string file_path { get; set; }
        public string file_name { get; set; }
        public string competition_marks { get; set; }
        public string competition_rank { get; set; }
        public int row_sta_cd { get; set; }
        public long trans_id { get; set; }
    
        public virtual BRANCH_MASTER BRANCH_MASTER { get; set; }
        public virtual COMPETITION_MASTER COMPETITION_MASTER { get; set; }
        public virtual STUDENT_MASTER STUDENT_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COMPETITION_WINNER_MASTER> COMPETITION_WINNER_MASTER { get; set; }
    }
}
