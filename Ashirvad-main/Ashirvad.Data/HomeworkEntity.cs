﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ashirvad.Data
{
    public class HomeworkEntity
    {
        public long HomeworkID { get; set; }
        public BranchEntity BranchInfo { get; set; }
        public DateTime HomeworkDate { get; set; }
        public StandardEntity StandardInfo { get; set; }
        public SubjectEntity SubjectInfo { get; set; }
        public int BatchTimeID { get; set; }
        public string BatchTimeText { get; set; }
        public string Remarks { get; set; }
        public HttpPostedFileBase FileInfo { get; set; }
        public byte[] HomeworkContent { get; set; }
        public string HomeworkContentText { get; set; }
        public string HomeworkContentFileName { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
    }
}
