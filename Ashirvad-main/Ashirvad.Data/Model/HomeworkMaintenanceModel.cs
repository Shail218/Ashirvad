using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class HomeworkMaintenanceModel
    {
        public HomeworkEntity HomeworkInfo { get; set; }
        public List<HomeworkEntity> HomeworkData { get; set; }
 
    }

    public class HomeworkDetailMaintenanceModel
    {
        public HomeworkDetailEntity HomeworkDetailInfo { get; set; }
        public List<HomeworkDetailEntity> HomeworkDetailData { get; set; }
    }
}
