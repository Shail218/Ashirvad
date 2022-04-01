using Ashirvad.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class WinnerEntryEntity
    {
        public long WinnerID { get; set; }
        public long? StudentID { get; set; }
        public long? BranchID { get; set; }
        public Nullable<long> CompetitionID { get; set; }
        public string PrizeName { get; set; }
        public Nullable<int> row_sta_cd { get; set; }
        public long trans_id { get; set; }
        public string extra1 { get; set; }
        public string extra2 { get; set; }
        public string extra3 { get; set; }

        public CompetitionEntity Competition { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }
    }
}
