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
    
    public partial class TRANSACTION_MASTER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TRANSACTION_MASTER()
        {
            this.BATCH_MASTER = new HashSet<BATCH_MASTER>();
            this.BRANCH_LICENSE = new HashSet<BRANCH_LICENSE>();
            this.BRANCH_MASTER = new HashSet<BRANCH_MASTER>();
            this.BRANCH_STAFF = new HashSet<BRANCH_STAFF>();
            this.SCHOOL_MASTER = new HashSet<SCHOOL_MASTER>();
            this.STD_MASTER = new HashSet<STD_MASTER>();
            this.STUDENT_MASTER = new HashSet<STUDENT_MASTER>();
            this.SUBJECT_MASTER = new HashSet<SUBJECT_MASTER>();
            this.USER_DEF = new HashSet<USER_DEF>();
            this.FEE_STRUCTURE_MASTER = new HashSet<FEE_STRUCTURE_MASTER>();
            this.STUDENT_PAYMENT_REL = new HashSet<STUDENT_PAYMENT_REL>();
            this.UPI_MASTER = new HashSet<UPI_MASTER>();
            this.FEE_STRUCTURE_DTL = new HashSet<FEE_STRUCTURE_DTL>();
            this.HOMEWORK_MASTER_DTL = new HashSet<HOMEWORK_MASTER_DTL>();
            this.MARKS_MASTER_DTL = new HashSet<MARKS_MASTER_DTL>();
            this.CATEGORY_MASTER = new HashSet<CATEGORY_MASTER>();
            this.LIBRARY_MASTER_DTL = new HashSet<LIBRARY_MASTER_DTL>();
            this.LIBRARY1_MASTER = new HashSet<LIBRARY1_MASTER>();
            this.ABOUTUS_DETAIL_REL = new HashSet<ABOUTUS_DETAIL_REL>();
            this.PACKAGE_MASTER = new HashSet<PACKAGE_MASTER>();
            this.PAGE_MASTER = new HashSet<PAGE_MASTER>();
            this.TEST_MASTER_DTL = new HashSet<TEST_MASTER_DTL>();
            this.PACKAGE_RIGHTS_MASTER = new HashSet<PACKAGE_RIGHTS_MASTER>();
        }
    
        public long trans_id { get; set; }
        public System.DateTime created_dt { get; set; }
        public string created_by { get; set; }
        public Nullable<long> created_id { get; set; }
        public Nullable<System.DateTime> last_mod_dt { get; set; }
        public string last_mod_by { get; set; }
        public Nullable<long> last_mod_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BATCH_MASTER> BATCH_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BRANCH_LICENSE> BRANCH_LICENSE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BRANCH_MASTER> BRANCH_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BRANCH_STAFF> BRANCH_STAFF { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SCHOOL_MASTER> SCHOOL_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STD_MASTER> STD_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STUDENT_MASTER> STUDENT_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SUBJECT_MASTER> SUBJECT_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USER_DEF> USER_DEF { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FEE_STRUCTURE_MASTER> FEE_STRUCTURE_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STUDENT_PAYMENT_REL> STUDENT_PAYMENT_REL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UPI_MASTER> UPI_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FEE_STRUCTURE_DTL> FEE_STRUCTURE_DTL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOMEWORK_MASTER_DTL> HOMEWORK_MASTER_DTL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MARKS_MASTER_DTL> MARKS_MASTER_DTL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CATEGORY_MASTER> CATEGORY_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LIBRARY_MASTER_DTL> LIBRARY_MASTER_DTL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LIBRARY1_MASTER> LIBRARY1_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ABOUTUS_DETAIL_REL> ABOUTUS_DETAIL_REL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PACKAGE_MASTER> PACKAGE_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PAGE_MASTER> PAGE_MASTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TEST_MASTER_DTL> TEST_MASTER_DTL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PACKAGE_RIGHTS_MASTER> PACKAGE_RIGHTS_MASTER { get; set; }
    }
}
