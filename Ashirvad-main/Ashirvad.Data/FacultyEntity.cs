using Ashirvad.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ashirvad.Data
{
    public class FacultyEntity : ResponseModel
    {
        public long FacultyID { get; set; }

        public Enums.BoardType board { get; set; }
        public string boardtype { get; set; }
        public List<FacultyEntity> facultytype { get; set; }
        public StandardEntity standard { get; set; }
        public SubjectEntity subject { get; set; }
        public StaffEntity staff { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public BranchEntity BranchInfo { get; set; }
        public string Descripation { get; set; }
        public HttpPostedFileBase FileInfo { get; set; }
        public string HeaderImageText { get; set; }
        public string FilePath { get; set; }
        public byte[] HeaderImage { get; set; }
        public string FacultyContentText { get; set; }
        public string FacultyContentFileName { get; set; }

        public List<FacultyEntity>facultylist { get; set; } = new List<FacultyEntity>();

    }

    public class FacultyTeacherEntity
    {
        FacultyEntity facultyEntity { get; set; } = new FacultyEntity();

        public List<FacultyEntity> teacherlist { get; set; } = new List<FacultyEntity>();
        public List<CategoryEntity> Categorylist { get; set; } = new List<CategoryEntity>();
    }
}
