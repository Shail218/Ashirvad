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
    
    public partial class MARKS_MASTER_DTL
    {
        public long marks_master_dtl_id { get; set; }
        public long marks_id { get; set; }
        public byte[] marks_sheet_content { get; set; }
        public string marks_sheet_name { get; set; }
        public string marks_filepath { get; set; }
        public int row_sta_cd { get; set; }
        public long trans_id { get; set; }
    
        public virtual MARKS_MASTER_DTL MARKS_MASTER_DTL1 { get; set; }
        public virtual MARKS_MASTER_DTL MARKS_MASTER_DTL2 { get; set; }
        public virtual TRANSACTION_MASTER TRANSACTION_MASTER { get; set; }
        public virtual MARKS_MASTER MARKS_MASTER { get; set; }
    }
}