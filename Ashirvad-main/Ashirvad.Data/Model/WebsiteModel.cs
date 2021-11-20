using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class WebsiteModel
    {

        public List<BranchCourseEntity> branchCoursesData { get; set; } = new List<BranchCourseEntity>();
        public List<BranchClassEntity> branchClassData { get; set; } = new List<BranchClassEntity>();
        public List<BranchSubjectEntity> branchSubjectData { get; set; } = new List<BranchSubjectEntity>();
    }
}
