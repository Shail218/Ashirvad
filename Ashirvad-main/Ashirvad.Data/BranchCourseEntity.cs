using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class BranchCourseEntity
    {
        public long course_dtl_id { get; set; }
        public BranchEntity branch { get; set; }
        public CourseEntity course { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public bool iscourse { get; set; }
    }
}
