
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
    
public partial class PAYMENT_MASTER
{

    public long payment_id { get; set; }

    public long student_id { get; set; }

    public long branch_id { get; set; }

    public string file_path { get; set; }

    public string file_name { get; set; }

    public string remark { get; set; }

    public Nullable<int> payment_status { get; set; }

    public long trans_id { get; set; }

    public int row_sta_cd { get; set; }

    public string extra1 { get; set; }

    public string extra2 { get; set; }

    public string extra3 { get; set; }



    public virtual BRANCH_MASTER BRANCH_MASTER { get; set; }

    public virtual STUDENT_MASTER STUDENT_MASTER { get; set; }

    public virtual TRANSACTION_MASTER TRANSACTION_MASTER { get; set; }

}

}
