﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ashirvad.Data
{
    public class CourseEntity
    {
        public long CourseID { get; set; }
        public string CourseName { get; set; }
        public string filepath { get; set; }
        public string filename { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
