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
    
    public partial class TEST_MASTER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEST_MASTER()
        {
            this.TEST_PAPER_REL = new HashSet<TEST_PAPER_REL>();
            this.STUDENT_ANS_SHEET = new HashSet<STUDENT_ANS_SHEET>();
        }
    
        public long test_id { get; set; }
        public long branch_id { get; set; }
        public string test_name { get; set; }
        public long std_id { get; set; }
        public int batch_time_id { get; set; }
        public long sub_id { get; set; }
        public double total_marks { get; set; }
        public System.DateTime test_dt { get; set; }
        public string test_st_time { get; set; }
        public string test_end_time { get; set; }
        public string remarks { get; set; }
        public int row_sta_cd { get; set; }
        public long trans_id { get; set; }
        public string file_name { get; set; }
    
        public virtual BRANCH_MASTER BRANCH_MASTER { get; set; }
        public virtual STD_MASTER STD_MASTER { get; set; }
        public virtual SUBJECT_MASTER SUBJECT_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEST_PAPER_REL> TEST_PAPER_REL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STUDENT_ANS_SHEET> STUDENT_ANS_SHEET { get; set; }
    }
}
