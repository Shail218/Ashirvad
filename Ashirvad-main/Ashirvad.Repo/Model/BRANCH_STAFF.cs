
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
    
public partial class BRANCH_STAFF
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public BRANCH_STAFF()
    {

        this.FACULTY_MASTER = new HashSet<FACULTY_MASTER>();

        this.USER_DEF = new HashSet<USER_DEF>();

    }


    public long staff_id { get; set; }

    public string name { get; set; }

    public string education { get; set; }

    public Nullable<System.DateTime> dob { get; set; }

    public Nullable<int> gender { get; set; }

    public string address { get; set; }

    public Nullable<System.DateTime> appt_dt { get; set; }

    public Nullable<System.DateTime> join_dt { get; set; }

    public Nullable<System.DateTime> leaving_dt { get; set; }

    public string email_id { get; set; }

    public long branch_id { get; set; }

    public string mobile_no { get; set; }

    public int row_sta_cd { get; set; }

    public long trans_id { get; set; }



    public virtual BRANCH_MASTER BRANCH_MASTER { get; set; }

    public virtual TRANSACTION_MASTER TRANSACTION_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<FACULTY_MASTER> FACULTY_MASTER { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<USER_DEF> USER_DEF { get; set; }

}

}
