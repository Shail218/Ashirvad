
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
    
public partial class USER_ROLE
{

    public long id { get; set; }

    public long user_id { get; set; }

    public int role_id { get; set; }

    public int row_sta_cd { get; set; }

    public long trans_id { get; set; }

    public string has_priv { get; set; }



    public virtual USER_DEF USER_DEF { get; set; }

}

}
