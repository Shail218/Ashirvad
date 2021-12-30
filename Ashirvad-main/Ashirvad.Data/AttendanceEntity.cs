using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class AttendanceEntity
    {
        public string JsonData { get; set; }
        public long AttendanceID { get; set; }
        public BranchEntity Branch { get; set; }
        public StandardEntity Standard { get; set; }
        public int BatchTypeID { get; set; }
        public string BatchTypeText { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime From_Date { get; set; }
        public DateTime To_Date { get; set; }
        public string AttendanceDatetxt { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public StudentEntity studentEntity { get; set; }
        public long Count { get; set; }
        public List<AttendanceDetailEntity> AttendanceDetail { get; set; }
    }

    public class AttendanceDetailEntity
    {
        public long DetailID { get; set; }
        public long HeaderID { get; set; }
        public StudentEntity Student { get; set; }
        public bool IsAbsent { get; set; }
        public bool IsPresent { get; set; }
        public string Remarks { get; set; }
    }
}
