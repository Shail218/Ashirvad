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
    
    public partial class BANNER_MASTER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BANNER_MASTER()
        {
            this.BANNER_TYPE_REL = new HashSet<BANNER_TYPE_REL>();
        }
    
        public long banner_id { get; set; }
        public Nullable<long> branch_id { get; set; }
        public long trans_id { get; set; }
        public byte[] banner_img { get; set; }
        public string file_name { get; set; }
        public string file_path { get; set; }
        public int row_sta_cd { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BANNER_TYPE_REL> BANNER_TYPE_REL { get; set; }
    }
}
