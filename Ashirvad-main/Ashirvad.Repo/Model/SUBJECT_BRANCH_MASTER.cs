
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
    
public partial class SUBJECT_BRANCH_MASTER
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public SUBJECT_BRANCH_MASTER()
    {

        this.SUBJECT_DTL_MASTER = new HashSet<SUBJECT_DTL_MASTER>();

    }


    public long subject_id { get; set; }

    public string subject_name { get; set; }

    public Nullable<int> row_sta_cd { get; set; }

    public long trans_id { get; set; }



    public virtual TRANSACTION_MASTER TRANSACTION_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<SUBJECT_DTL_MASTER> SUBJECT_DTL_MASTER { get; set; }

}

}