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
    
    public partial class ATTENDANCE_DTL
    {
        public long attd_dtl_id { get; set; }
        public long attd_hdr_id { get; set; }
        public long student_id { get; set; }
        public int absent_fg { get; set; }
        public int present_fg { get; set; }
        public string remarks { get; set; }
    
        public virtual ATTENDANCE_HDR ATTENDANCE_HDR { get; set; }
    }
}
