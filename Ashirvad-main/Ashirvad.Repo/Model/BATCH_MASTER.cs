
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
    
public partial class BATCH_MASTER
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public BATCH_MASTER()
    {

        this.MARKS_MASTER = new HashSet<MARKS_MASTER>();

    }


    public long batch_id { get; set; }

    public long branch_id { get; set; }

    public long std_id { get; set; }

    public int batch_time { get; set; }

    public string mon_fri_batch_time { get; set; }

    public string sat_batch_time { get; set; }

    public string sun_batch_time { get; set; }

    public int row_sta_cd { get; set; }

    public long trans_id { get; set; }



    public virtual BRANCH_MASTER BRANCH_MASTER { get; set; }

    public virtual STD_MASTER STD_MASTER { get; set; }

    public virtual TRANSACTION_MASTER TRANSACTION_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<MARKS_MASTER> MARKS_MASTER { get; set; }

}

}
