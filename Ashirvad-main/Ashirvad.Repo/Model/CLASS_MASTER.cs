
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
    
public partial class CLASS_MASTER
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public CLASS_MASTER()
    {

        this.CLASS_DTL_MASTER = new HashSet<CLASS_DTL_MASTER>();

        this.SUBJECT_BRANCH_MASTER = new HashSet<SUBJECT_BRANCH_MASTER>();

    }


    public long class_id { get; set; }

    public string class_name { get; set; }

    public Nullable<int> row_sta_cd { get; set; }

    public long trans_id { get; set; }

    public Nullable<long> course_id { get; set; }

    public string extra1 { get; set; }

    public string extra2 { get; set; }

    public string extra3 { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<CLASS_DTL_MASTER> CLASS_DTL_MASTER { get; set; }

    public virtual COURSE_MASTER COURSE_MASTER { get; set; }

    public virtual TRANSACTION_MASTER TRANSACTION_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<SUBJECT_BRANCH_MASTER> SUBJECT_BRANCH_MASTER { get; set; }

}

}
