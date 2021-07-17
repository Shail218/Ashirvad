using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class TestMaintenanceModel
    {
        public TestEntity TestInfo { get; set; }
        public List<TestEntity> TestData { get; set; }
        public TestPaperEntity TestPaperInfo { get; set; }
        public List<TestPaperEntity> TestPaperData { get; set; }
        public List<StudentAnswerSheetEntity> StudentAnswerSheetData { get; set; }
    }
}
