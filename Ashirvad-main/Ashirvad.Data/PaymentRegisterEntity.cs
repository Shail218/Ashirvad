using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class PaymentRegisterEntity
    {
        public long payment_id { get; set; }
        public StudentEntity studentEntity { get; set; }
        public BranchEntity branchEntity { get; set; }
        public string file_path { get; set; }
        public string file_name { get; set; }
        public string remark { get; set; }
        public int payment_status { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public string status_txt { get; set; }
        public string student_remark { get; set; }
    }
}
