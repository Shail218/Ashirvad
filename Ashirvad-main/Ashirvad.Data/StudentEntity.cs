using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ashirvad.Data
{
    public class StudentEntity
    {
        public string UserName { get; set; }
        public long StudentID { get; set; }
        public string GrNo { get; set; }
        public string FirstName { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        public string Address { get; set; }
        public int? SchoolTime { get; set; }
        public int? LastYearResult { get; set; }
        public long Count { get; set; }
        public string Grade { get; set; }
        public string LastYearClassName { get; set; }
        public string ContactNo { get; set; }
        public string StudImage { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public byte[] StudentImgByte { get; set; }
        public StandardEntity StandardInfo { get; set; } = new StandardEntity();
        public SchoolEntity SchoolInfo { get; set; } = new SchoolEntity();
        public BranchEntity BranchInfo { get; set; } = new BranchEntity();
        public TransactionEntity Transaction { get; set; } = new TransactionEntity();
        public RowStatusEntity RowStatus { get; set; } = new RowStatusEntity();
        public BatchEntity BatchInfo { get; set; } = new BatchEntity();
        public StudentMaint StudentMaint { get; set; } = new StudentMaint();
        public DateTime? AdmissionDate { get; set; }
        public string StudentPassword { get; set; }
        public string StudentPassword2 { get; set; }
        public long? UserID { get; set; } = 0;
        public HttpPostedFileBase ImageFile { get; set; }
        public BranchCourseEntity BranchCourse { get; set; } = new BranchCourseEntity();
        public BranchClassEntity BranchClass { get; set; } = new BranchClassEntity();
        public string Final_Year { get; set; }
        public string birth_date { get; set; }
        public string admission_date { get; set; }
    }
    public class StudentMaint
    {
        public long ParentID { get; set; }
        public long StudentID { get; set; }
        public string ParentName { get; set; }
        public string FatherOccupation { get; set; }
        public string MotherOccupation { get; set; }
        public string ContactNo { get; set; }
        public string ParentPassword { get; set; }
        public string ParentPassword2 { get; set; }
        public long? UserID { get; set; } = 0;
    }
}
