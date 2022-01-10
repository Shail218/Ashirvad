using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class HomeworkchartModel
    {
        public StudentEntity studentlist { get; set; }

        public List<HomeworkDetailEntity> homeworklist { get; set; }
    }
}
