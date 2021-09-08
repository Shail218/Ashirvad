﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data
{
    public class StudentEntity
    {
        public long StudentID { get; set; }
        public string GrNo { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        public string Address { get; set; }
        public int? SchoolTime { get; set; }
        public int? LastYearResult { get; set; }
        public string Grade { get; set; }
        public string LastYearClassName { get; set; }
        public string ContactNo { get; set; }
        public string StudImage { get; set; }
        public byte[] StudentImgByte { get; set; }
        public StandardEntity StandardInfo { get; set; }
        public SchoolEntity SchoolInfo { get; set; }
        public BranchEntity BranchInfo { get; set; }
        public TransactionEntity Transaction { get; set; }
        public RowStatusEntity RowStatus { get; set; }
        public BatchEntity BatchInfo { get; set; }
        public StudentMaint StudentMaint { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public string StudentPassword { get; set; }
        public long UserID { get; set; }
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
    }
}
