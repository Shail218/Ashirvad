
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
    
public partial class COURSE_DTL_MASTER
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public COURSE_DTL_MASTER()
    {

        this.CLASS_DTL_MASTER = new HashSet<CLASS_DTL_MASTER>();

        this.SUBJECT_DTL_MASTER = new HashSet<SUBJECT_DTL_MASTER>();

        this.FACULTY_MASTER = new HashSet<FACULTY_MASTER>();

    }


    public long course_dtl_id { get; set; }

    public long branch_id { get; set; }

    public long course_id { get; set; }

    public Nullable<bool> is_course { get; set; }

    public Nullable<int> row_sta_cd { get; set; }

    public long trans_id { get; set; }



    public virtual BRANCH_MASTER BRANCH_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<CLASS_DTL_MASTER> CLASS_DTL_MASTER { get; set; }

    public virtual COURSE_MASTER COURSE_MASTER { get; set; }

    public virtual TRANSACTION_MASTER TRANSACTION_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<SUBJECT_DTL_MASTER> SUBJECT_DTL_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<FACULTY_MASTER> FACULTY_MASTER { get; set; }

}

}
