
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
    
public partial class ABOUTUS_MASTER
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ABOUTUS_MASTER()
    {

        this.ABOUTUS_DETAIL_REL = new HashSet<ABOUTUS_DETAIL_REL>();

    }


    public long aboutus_id { get; set; }

    public string website { get; set; }

    public string email_id { get; set; }

    public string contact_no { get; set; }

    public string whatsapp_no { get; set; }

    public long branch_id { get; set; }

    public byte[] header_img { get; set; }

    public string header_img_name { get; set; }

    public long trans_id { get; set; }

    public int row_sta_cd { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ABOUTUS_DETAIL_REL> ABOUTUS_DETAIL_REL { get; set; }

    public virtual BRANCH_MASTER BRANCH_MASTER { get; set; }

}

}
