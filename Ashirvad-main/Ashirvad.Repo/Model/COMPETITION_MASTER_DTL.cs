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
    
    public partial class COMPETITION_MASTER_DTL
    {
        public long competition_dtl_id { get; set; }
        public long competition_id { get; set; }
        public long branch_id { get; set; }
        public long stud_id { get; set; }
        public byte[] competition_sheet_content { get; set; }
        public string competition_sheet_name { get; set; }
        public string competition_filepath { get; set; }
        public int status { get; set; }
        public string remarks { get; set; }
        public int row_sta_cd { get; set; }
        public long trans_id { get; set; }
        public System.DateTime submit_dt { get; set; }
    
        public virtual BRANCH_MASTER BRANCH_MASTER { get; set; }
        public virtual COMPETITION_MASTER COMPETITION_MASTER { get; set; }
        public virtual STUDENT_MASTER STUDENT_MASTER { get; set; }
        public virtual TRANSACTION_MASTER TRANSACTION_MASTER { get; set; }
    }
}
