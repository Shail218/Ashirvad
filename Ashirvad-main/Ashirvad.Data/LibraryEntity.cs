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
        public string ThumbImageName { get; set; }
        public string ThumbDocName { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public long? StandardID { get; set; }
        public long? SubjectID { get; set; }
        public string Description { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }
        public LibraryDataEntity LibraryData { get; set; }
        public BranchEntity BranchData { get; set; }
        public CategoryEntity CategoryInfo { get; set; }

    }

    public class LibraryDataEntity
    {
        public long UniqueID { get; set; }
        public long LibraryID { get; set; }
        public byte[] ThumbImageContent { get; set; }
        public string ThumbImageContentText { get; set; }
        public string ThumbImageExt{ get; set; }
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
        public long LibraryDetailID { get; set; }
        public long Type { get; set; }
        public string Title { get; set; }
        public byte[] FileContent { get; set; }
        public string link { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }  
        public BranchEntity BranchInfo { get; set; }
        public CategoryEntity CategoryInfo { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
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
