using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ashirvad.Data
{
    public class LibraryEntity
    {
        public long LibraryID { get; set; }
        public long? BranchID { get; set; }
        public string LibraryTitle { get; set; }
        public string VideoLink { get; set; }
        public string ThumbnailFileName { get; set; }
        public string ThumbnailFilePath { get; set; }
        public string DocFileName { get; set; }
        public string DocFilePath { get; set; }
        public int Type { get; set; }
        public long Count { get; set; }
        public int Library_Type { get; set; }
        public long? StandardID { get; set; }
        public long? SubjectID { get; set; }
        public string StandardArray { get; set; }
        public string StandardNameArray { get; set; }
        public string Description { get; set; }
        public long CreatebyBranch { get; set; }
        public string JsonList { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }
        public LibraryDataEntity LibraryData { get; set; }
        public BranchEntity BranchData { get; set; }
        public CategoryEntity CategoryInfo { get; set; }
        public HttpPostedFileBase ThumbImageFile { get; set; }
        public HttpPostedFileBase DocFile { get; set; }
        public SubjectEntity subject { get; set; }
        public StandardEntity standard { get; set; }
        public List<LibraryEntity> libraryEntities { get; set; } = new List<LibraryEntity>();
        public long Library_Std_id { get; set; }
        public ApprovalEntity approval { get; set; }
        public List<LibraryStandardEntity> list { get; set; } = new List<LibraryStandardEntity>();
        public List<SubjectEntity> Subjectlist { get; set; }
        public List<StandardEntity> Standardlist { get; set; } = new List<StandardEntity>();
        public BranchCourseEntity BranchCourse { get; set; }
        public BranchClassEntity BranchClass { get; set; }
        public BranchSubjectEntity BranchSubject { get; set; }
    }

    public class LibraryStandardEntity
    {
        public long library_std_id { get; set; }
        public long std_id { get; set; }
        public long sub_id { get; set; }
        public string standard { get; set; }
        public string subject { get; set; }
        public long library_id { get; set; }
        public BranchCourseEntity BranchCourse { get; set; }
    }

    public class LibraryDataEntity
    {
        public long UniqueID { get; set; }
        public long LibraryID { get; set; }
        public byte[] ThumbImageContent { get; set; }
        public string ThumbImageContentText { get; set; }
        public string ThumbImageExt { get; set; }
        public string ThumbImageFileName { get; set; }
        public byte[] DocContent { get; set; }
        public string DocContentText { get; set; }
        public string DocContentFileName { get; set; }
        public string DocContentExt { get; set; }
        public HttpPostedFileBase ThumbImageFile { get; set; }
        public HttpPostedFileBase DocFile { get; set; }
    }


    public class LibraryEntity1
    {
        public long LibraryID { get; set; }
        public long NewLibraryID { get; set; }
        public string Title { get; set; }
        public string link { get; set; }
        public int Type { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }
        public BranchEntity BranchInfo { get; set; }
        public Nullable<long> branchid { get; set; }
        public CategoryEntity CategoryInfo { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public LibraryEntity LibraryEntity { get; set; }
    }

    public class LibraryImageEntity
    {
        public List<LibraryEntity1> imagelist { get; set; } = new List<LibraryEntity1>();
        public List<CategoryEntity> Categorylist { get; set; } = new List<CategoryEntity>();
    }

    public class LibraryVideoEntity
    {
        public List<LibraryEntity1> Videolist { get; set; } = new List<LibraryEntity1>();
        public List<CategoryEntity> Categorylist { get; set; } = new List<CategoryEntity>();
    }

}
