using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
  public  class CategoryMaintenanceModel
    {
        public CategoryEntity CategoryInfo { get; set; }
        public List<CategoryEntity> CategoryData { get; set; }
    }
}
