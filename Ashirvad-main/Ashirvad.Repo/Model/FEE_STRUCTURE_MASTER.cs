
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
    
public partial class FEE_STRUCTURE_MASTER
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public FEE_STRUCTURE_MASTER()
    {

        this.FEE_STRUCTURE_DTL = new HashSet<FEE_STRUCTURE_DTL>();

    }


    public long fee_struct_mst_id { get; set; }

    public long branch_id { get; set; }

    public string remarks { get; set; }

    public long trans_id { get; set; }

    public int row_sta_cd { get; set; }

    public Nullable<long> class_dtl_id { get; set; }

    public Nullable<long> course_dtl_id { get; set; }



    public virtual BRANCH_MASTER BRANCH_MASTER { get; set; }

    public virtual CLASS_DTL_MASTER CLASS_DTL_MASTER { get; set; }

    public virtual COURSE_DTL_MASTER COURSE_DTL_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<FEE_STRUCTURE_DTL> FEE_STRUCTURE_DTL { get; set; }

    public virtual TRANSACTION_MASTER TRANSACTION_MASTER { get; set; }

}

}
