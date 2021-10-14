using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ashirvad.Data
{
    public class ToDoEntity
    {
        public long ToDoID { get; set; }
        public DateTime ToDoDate { get; set; }
        public BranchEntity BranchInfo { get; set; }
        public UserEntity UserInfo { get; set; }
        public string ToDoDescription { get; set; }
        public HttpPostedFileBase FileInfo { get; set; }
        public byte[] ToDoContent { get; set; }
        public string ToDoContentText { get; set; }
        public string ToDoFileName { get; set; }
        public string Remark { get; set; }
        public bool? Registerstatus { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public TransactionEntity Transaction { get; set; }
    }
}
