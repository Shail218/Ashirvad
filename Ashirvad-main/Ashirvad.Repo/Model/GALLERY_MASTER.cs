
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
    
public partial class GALLERY_MASTER
{

    public long unique_id { get; set; }

    public long branch_id { get; set; }

    public string file_name { get; set; }

    public string file_path { get; set; }

    public string remarks { get; set; }

    public int row_sta_cd { get; set; }

    public long trans_id { get; set; }

    public int uplaod_type { get; set; }



    public virtual BRANCH_MASTER BRANCH_MASTER { get; set; }

}

}
