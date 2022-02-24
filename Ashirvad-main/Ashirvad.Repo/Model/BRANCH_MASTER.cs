
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
    
public partial class BRANCH_MASTER
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public BRANCH_MASTER()
    {

        this.ABOUTUS_DETAIL_REL = new HashSet<ABOUTUS_DETAIL_REL>();

        this.BRANCH_AGREEMENT = new HashSet<BRANCH_AGREEMENT>();

        this.BRANCH_LICENSE = new HashSet<BRANCH_LICENSE>();

        this.BRANCH_RIGHTS_MASTER = new HashSet<BRANCH_RIGHTS_MASTER>();

        this.BRANCH_STAFF = new HashSet<BRANCH_STAFF>();

        this.CATEGORY_MASTER = new HashSet<CATEGORY_MASTER>();

        this.GALLERY_MASTER = new HashSet<GALLERY_MASTER>();

        this.HOMEWORK_MASTER_DTL = new HashSet<HOMEWORK_MASTER_DTL>();

        this.PAGE_MASTER = new HashSet<PAGE_MASTER>();

        this.REMINDER_MASTER = new HashSet<REMINDER_MASTER>();

        this.SCHOOL_MASTER = new HashSet<SCHOOL_MASTER>();

        this.STUDENT_ANS_SHEET = new HashSet<STUDENT_ANS_SHEET>();

        this.STUDENT_PAYMENT_MASTER = new HashSet<STUDENT_PAYMENT_MASTER>();

        this.TODO_MASTER = new HashSet<TODO_MASTER>();

        this.UPI_MASTER = new HashSet<UPI_MASTER>();

        this.CLASS_DTL_MASTER = new HashSet<CLASS_DTL_MASTER>();

        this.SUBJECT_DTL_MASTER = new HashSet<SUBJECT_DTL_MASTER>();

        this.COURSE_DTL_MASTER = new HashSet<COURSE_DTL_MASTER>();

        this.FACULTY_MASTER = new HashSet<FACULTY_MASTER>();

        this.NEW_LIBRARY_MASTER = new HashSet<NEW_LIBRARY_MASTER>();

        this.APPROVAL_MASTER = new HashSet<APPROVAL_MASTER>();

        this.ABOUTUS_MASTER = new HashSet<ABOUTUS_MASTER>();

        this.BATCH_MASTER = new HashSet<BATCH_MASTER>();

        this.ATTENDANCE_HDR = new HashSet<ATTENDANCE_HDR>();

        this.FEE_STRUCTURE_MASTER = new HashSet<FEE_STRUCTURE_MASTER>();

        this.PRACTICE_PAPER = new HashSet<PRACTICE_PAPER>();

        this.LINK_MASTER = new HashSet<LINK_MASTER>();

        this.MARKS_MASTER = new HashSet<MARKS_MASTER>();

        this.TEST_MASTER = new HashSet<TEST_MASTER>();

        this.TEST_MASTER_DTL = new HashSet<TEST_MASTER_DTL>();

        this.HOMEWORK_MASTER = new HashSet<HOMEWORK_MASTER>();

        this.STD_MASTER = new HashSet<STD_MASTER>();

        this.SUBJECT_MASTER = new HashSet<SUBJECT_MASTER>();

        this.STUDENT_MASTER = new HashSet<STUDENT_MASTER>();

        this.PAYMENT_MASTER = new HashSet<PAYMENT_MASTER>();

        this.PACKAGE_MASTER = new HashSet<PACKAGE_MASTER>();

    }


    public long branch_id { get; set; }

    public string branch_name { get; set; }

    public Nullable<int> row_sta_cd { get; set; }

    public string about_us { get; set; }

    public string contact_no { get; set; }

    public string mobile_no { get; set; }

    public string email_id { get; set; }

    public long trans_id { get; set; }

    public int branch_type { get; set; }

    public Nullable<int> board_type { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ABOUTUS_DETAIL_REL> ABOUTUS_DETAIL_REL { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<BRANCH_AGREEMENT> BRANCH_AGREEMENT { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<BRANCH_LICENSE> BRANCH_LICENSE { get; set; }

    public virtual BRANCH_MAINT BRANCH_MAINT { get; set; }

    public virtual TRANSACTION_MASTER TRANSACTION_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<BRANCH_RIGHTS_MASTER> BRANCH_RIGHTS_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<BRANCH_STAFF> BRANCH_STAFF { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<CATEGORY_MASTER> CATEGORY_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<GALLERY_MASTER> GALLERY_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<HOMEWORK_MASTER_DTL> HOMEWORK_MASTER_DTL { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<PAGE_MASTER> PAGE_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<REMINDER_MASTER> REMINDER_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<SCHOOL_MASTER> SCHOOL_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<STUDENT_ANS_SHEET> STUDENT_ANS_SHEET { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<STUDENT_PAYMENT_MASTER> STUDENT_PAYMENT_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<TODO_MASTER> TODO_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<UPI_MASTER> UPI_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<CLASS_DTL_MASTER> CLASS_DTL_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<SUBJECT_DTL_MASTER> SUBJECT_DTL_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<COURSE_DTL_MASTER> COURSE_DTL_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<FACULTY_MASTER> FACULTY_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<NEW_LIBRARY_MASTER> NEW_LIBRARY_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<APPROVAL_MASTER> APPROVAL_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ABOUTUS_MASTER> ABOUTUS_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<BATCH_MASTER> BATCH_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ATTENDANCE_HDR> ATTENDANCE_HDR { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<FEE_STRUCTURE_MASTER> FEE_STRUCTURE_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<PRACTICE_PAPER> PRACTICE_PAPER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<LINK_MASTER> LINK_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<MARKS_MASTER> MARKS_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<TEST_MASTER> TEST_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<TEST_MASTER_DTL> TEST_MASTER_DTL { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<HOMEWORK_MASTER> HOMEWORK_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<STD_MASTER> STD_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<SUBJECT_MASTER> SUBJECT_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<STUDENT_MASTER> STUDENT_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<PAYMENT_MASTER> PAYMENT_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<PACKAGE_MASTER> PACKAGE_MASTER { get; set; }

}

}
