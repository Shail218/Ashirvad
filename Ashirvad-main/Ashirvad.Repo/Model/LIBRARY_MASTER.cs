
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
    
public partial class LIBRARY_MASTER
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public LIBRARY_MASTER()
    {

        this.APPROVAL_MASTER = new HashSet<APPROVAL_MASTER>();

    }


    public long library_id { get; set; }

    public Nullable<long> branch_id { get; set; }

    public string video_link { get; set; }

    public string library_title { get; set; }

    public string thumbnail_img { get; set; }

    public string thumbnail_path { get; set; }

    public Nullable<int> type { get; set; }

    public string doc_desc { get; set; }

    public Nullable<int> row_sta_cd { get; set; }

    public Nullable<long> trans_id { get; set; }

    public string library_image { get; set; }

    public string library_path { get; set; }

    public Nullable<long> category_id { get; set; }

    public Nullable<int> library_type { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<APPROVAL_MASTER> APPROVAL_MASTER { get; set; }

    public virtual CATEGORY_MASTER CATEGORY_MASTER { get; set; }

}

}
