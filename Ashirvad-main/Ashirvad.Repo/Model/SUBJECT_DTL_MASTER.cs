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
            this.FACULTY_MASTER = new HashSet<FACULTY_MASTER>();
            this.TEST_MASTER = new HashSet<TEST_MASTER>();
            this.HOMEWORK_MASTER = new HashSet<HOMEWORK_MASTER>();
            this.LIBRARY_STD_MASTER = new HashSet<LIBRARY_STD_MASTER>();
            this.MARKS_MASTER = new HashSet<MARKS_MASTER>();
            this.PRACTICE_PAPER = new HashSet<PRACTICE_PAPER>();
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
        public virtual TRANSACTION_MASTER TRANSACTION_MASTER { get; set; }
        public virtual COURSE_DTL_MASTER COURSE_DTL_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SUBJECT_MASTER> SUBJECT_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FACULTY_MASTER> FACULTY_MASTER { get; set; }
        public virtual SUBJECT_BRANCH_MASTER SUBJECT_BRANCH_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEST_MASTER> TEST_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOMEWORK_MASTER> HOMEWORK_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LIBRARY_STD_MASTER> LIBRARY_STD_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MARKS_MASTER> MARKS_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRACTICE_PAPER> PRACTICE_PAPER { get; set; }
    }
}
