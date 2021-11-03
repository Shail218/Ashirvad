using Ashirvad.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
   public class FacultyEntity:ResponseModel
    {

        public Enums.BoardType board { get; set; }
        public long boardtype { get; set; }
        public StandardEntity standard { get; set; }
        public SubjectEntity subject { get; set; }
        public UserEntity user { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public BranchEntity BranchInfo { get; set; }
    }
}
