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
    
    public partial class LINK_MASTER
    {
        public long unique_id { get; set; }
        public long branch_id { get; set; }
        public string vid_desc { get; set; }
        public string vid_url { get; set; }
        public int row_sta_cd { get; set; }
        public long trans_id { get; set; }
        public string title { get; set; }
        public int type { get; set; }
        public Nullable<long> class_dtl_id { get; set; }
        public Nullable<long> course_dtl_id { get; set; }
    
        public virtual BRANCH_MASTER BRANCH_MASTER { get; set; }
        public virtual CLASS_DTL_MASTER CLASS_DTL_MASTER { get; set; }
        public virtual COURSE_DTL_MASTER COURSE_DTL_MASTER { get; set; }
    }
}
