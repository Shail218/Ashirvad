using Ashirvad.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ashirvad.Data
{
   public class FacultyEntity:ResponseModel
    {

        public Enums.BoardType board { get; set; }
        public long boardtype { get; set; }
        public long FacultyID { get; set; }
        public UserEntity user { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public BranchEntity BranchInfo { get; set; }
        public StaffEntity staff { get; set; }
        public SubjectEntity subject { get; set; }
        public StandardEntity standard { get; set; }
        public string Descripation { get; set; }
        public string FacultyContentFileName { get; set; }
        public string FilePath { get; set; }
        public string HeaderImageText { get; set; }
        public BranchSubjectEntity branchSubject { get; set; }
        public BranchCourseEntity BranchCourse { get; set; }
        public BranchClassEntity BranchClass { get; set; }
        public HttpPostedFileBase FileInfo { get; set; }
    }
}
