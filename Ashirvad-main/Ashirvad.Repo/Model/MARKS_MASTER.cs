
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
    
public partial class MARKS_MASTER
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public MARKS_MASTER()
    {

        this.MARKS_MASTER_DTL = new HashSet<MARKS_MASTER_DTL>();

    }


    public long marks_id { get; set; }

    public System.DateTime marks_dt { get; set; }

    public long branch_id { get; set; }

    public long std_id { get; set; }

    public long sub_id { get; set; }

    public long batch_id { get; set; }

    public string remarks { get; set; }

    public string total_marks { get; set; }

    public int row_sta_cd { get; set; }

    public long trans_id { get; set; }



    public virtual BATCH_MASTER BATCH_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<MARKS_MASTER_DTL> MARKS_MASTER_DTL { get; set; }

    public virtual STD_MASTER STD_MASTER { get; set; }

    public virtual SUBJECT_MASTER SUBJECT_MASTER { get; set; }

    public virtual BRANCH_MASTER BRANCH_MASTER { get; set; }

}

}
