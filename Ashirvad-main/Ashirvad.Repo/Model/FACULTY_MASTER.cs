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
        public Nullable<long> subject_id { get; set; }
        public Nullable<long> std_id { get; set; }
        public Nullable<int> board_type { get; set; }
        public long trans_id { get; set; }
        public int row_sta_cd { get; set; }
        public string description { get; set; }
        public string file_name { get; set; }
        public string file_path { get; set; }
    
        public virtual STD_MASTER STD_MASTER { get; set; }
        public virtual SUBJECT_MASTER SUBJECT_MASTER { get; set; }
        public virtual TRANSACTION_MASTER TRANSACTION_MASTER { get; set; }
        public virtual BRANCH_MASTER BRANCH_MASTER { get; set; }
        public virtual BRANCH_STAFF BRANCH_STAFF { get; set; }
    }
}