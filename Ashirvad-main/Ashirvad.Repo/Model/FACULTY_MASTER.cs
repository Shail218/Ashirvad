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
    
    public partial class FACULTY_MASTER
    {
        public long faculty_id { get; set; }
        public Nullable<long> staff_id { get; set; }
        public Nullable<long> branch_id { get; set; }
        public long course_dtl_id { get; set; }
        public long class_dtl_id { get; set; }
        public long subject_dtl_id { get; set; }
        public long trans_id { get; set; }
        public int row_sta_cd { get; set; }
        public string description { get; set; }
        public string file_name { get; set; }
        public string file_path { get; set; }
    
        public virtual BRANCH_STAFF BRANCH_STAFF { get; set; }
        public virtual CLASS_DTL_MASTER CLASS_DTL_MASTER { get; set; }
        public virtual COURSE_DTL_MASTER COURSE_DTL_MASTER { get; set; }
        public virtual SUBJECT_DTL_MASTER SUBJECT_DTL_MASTER { get; set; }
        public virtual TRANSACTION_MASTER TRANSACTION_MASTER { get; set; }
        public virtual BRANCH_MASTER BRANCH_MASTER { get; set; }
    }
}
