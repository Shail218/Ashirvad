using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Student
{
    public class Student : ModelAccess, IStudentAPI
    {
        public async Task<long> StudentMaintenance(StudentEntity studentInfo)
        {
            Model.STUDENT_MASTER studentMaster = new Model.STUDENT_MASTER();
            Model.STUDENT_MAINT studentMaint = new Model.STUDENT_MAINT();
            //studentMaster.STUDENT_MAINT = new Model.STUDENT_MAINT();
            bool isUpdate = true;
            var data = (from student in this.context.STUDENT_MASTER.Include("STUDENT_MAINT")
                        where student.student_id == studentInfo.StudentID
                        select new
                        {
                            studentMaster = student,
                            studentMaint = student.STUDENT_MAINT
                        }).FirstOrDefault();
            if (data == null)
            {
                studentMaster = new Model.STUDENT_MASTER();
                studentMaint = new Model.STUDENT_MAINT();
                //studentMaster.STUDENT_MAINT = new Model.STUDENT_MAINT();
                isUpdate = false;
            }
            else
            {
                studentMaster = data.studentMaster;
                studentMaint = data.studentMaster.STUDENT_MAINT.FirstOrDefault();
                studentInfo.Transaction.TransactionId = data.studentMaster.trans_id;
            }

            studentMaster.gr_no = studentInfo.GrNo;
            studentMaster.first_name = studentInfo.FirstName;
            studentMaster.middle_name = studentInfo.MiddleName;
            studentMaster.last_name = studentInfo.LastName;
            studentMaster.dob = studentInfo.DOB;
            studentMaster.admission_date = studentInfo.AdmissionDate;
            studentMaster.address = studentInfo.Address;
            studentMaster.branch_id = studentInfo.BranchInfo.BranchID;
            studentMaster.std_id = studentInfo.StandardInfo.StandardID;
            studentMaster.school_id = studentInfo.SchoolInfo.SchoolID;
            studentMaster.school_time = studentInfo.SchoolTime;
            studentMaster.batch_time = (int)studentInfo.BatchInfo.BatchType;
            studentMaster.last_yr_result = studentInfo.LastYearResult;
            studentMaster.grade = studentInfo.Grade;
            studentMaster.last_yr_class_name = studentInfo.LastYearClassName;
            studentMaster.contact_no = studentInfo.ContactNo;
            studentMaster.admission_date = studentInfo.AdmissionDate;
            studentMaster.stud_img = studentInfo.StudImage != "" ? Convert.FromBase64String(studentInfo.StudImage) : null;
            studentMaster.row_sta_cd = studentInfo.RowStatus.RowStatusId;
            studentMaster.trans_id = this.AddTransactionData(studentInfo.Transaction);
            this.context.STUDENT_MASTER.Add(studentMaster);
            if (isUpdate)
            {
                this.context.Entry(studentMaster).State = System.Data.Entity.EntityState.Modified;
            }
            if (!isUpdate)
            {
                studentMaint.student_id = studentMaster.student_id;
            }
            studentMaint.parent_name = studentInfo.StudentMaint.ParentName;
            studentMaint.father_occupation = studentInfo.StudentMaint.FatherOccupation;
            studentMaint.mother_occupation = studentInfo.StudentMaint.MotherOccupation;
            studentMaint.contact_no = studentInfo.StudentMaint.ContactNo;
            this.context.STUDENT_MAINT.Add(studentMaint);
            if (isUpdate)
            {
                this.context.Entry(studentMaint).State = System.Data.Entity.EntityState.Modified;
            }
            return this.context.SaveChanges() > 0 ? studentMaster.student_id : 0;
        }

        public async Task<List<StudentEntity>> GetAllStudent(long branchID, int status)
        {
            var data = (from u in this.context.STUDENT_MASTER
                        .Include("STD_MASTER")
                        .Include("SCHOOL_MASTER")
                        .Include("BRANCH_MASTER")
                        join maint in this.context.STUDENT_MAINT on u.student_id equals maint.student_id
                        where branchID == 0 || u.branch_id == branchID
                        && (0 == status || u.row_sta_cd == status)
                        select new StudentEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            StudentID = u.student_id,
                            Address = u.address,
                            DOB = u.dob,
                            FirstName = u.first_name,
                            LastName = u.last_name,
                            MiddleName = u.middle_name,
                            ContactNo = u.contact_no,
                            LastYearResult = u.last_yr_result,
                            LastYearClassName = u.last_yr_class_name,
                            Grade = u.grade,
                            AdmissionDate = u.admission_date,
                            GrNo = u.gr_no,
                            SchoolTime = u.school_time,
                            StudentImgByte = u.stud_img,
                            //StudImage = u.stud_img.Length > 0 ? Convert.ToBase64String(u.stud_img) : "",
                            StandardInfo = new StandardEntity() { StandardID = u.std_id, Standard = u.STD_MASTER.standard },
                            SchoolInfo = new SchoolEntity() { SchoolID = (long)u.school_id, SchoolName = u.SCHOOL_MASTER.school_name },
                            BatchInfo = new BatchEntity() { BatchTime = u.batch_time, BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : Enums.BatchType.Evening },
                            StudentMaint = new StudentMaint()
                            {
                                ParentName = maint.parent_name,
                                ParentID = maint.parent_id,
                                ContactNo = maint.contact_no,
                                FatherOccupation = maint.father_occupation,
                                MotherOccupation = maint.mother_occupation,
                                StudentID = maint.student_id
                            },
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            foreach (var item in data)
            {
                data[data.IndexOf(item)].StudImage = item.StudentImgByte != null && item.StudentImgByte.Length > 0 ? Convert.ToBase64String(item.StudentImgByte) : "";
            }
            return data;
        }

        public async Task<List<StudentEntity>> GetAllStudentWithoutContent(long branchID, int status)
        {
            var data = (from u in this.context.STUDENT_MASTER
                        .Include("STD_MASTER")
                        .Include("SCHOOL_MASTER")
                        .Include("BRANCH_MASTER")
                        join maint in this.context.STUDENT_MAINT on u.student_id equals maint.student_id
                        where branchID == 0 || u.branch_id == branchID
                        && (0 == status || u.row_sta_cd == status)
                        select new StudentEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            StudentID = u.student_id,
                            Address = u.address,
                            DOB = u.dob,
                            FirstName = u.first_name,
                            LastName = u.last_name,
                            MiddleName = u.middle_name,
                            ContactNo = u.contact_no,
                            LastYearResult = u.last_yr_result,
                            LastYearClassName = u.last_yr_class_name,
                            Grade = u.grade,
                            AdmissionDate = u.admission_date,
                            GrNo = u.gr_no,
                            SchoolTime = u.school_time,
                            StudentImgByte = u.stud_img,

                            //StudImage = u.stud_img.Length > 0 ? Convert.ToBase64String(u.stud_img) : "",
                            StandardInfo = new StandardEntity() { StandardID = u.std_id, Standard = u.STD_MASTER.standard },
                            SchoolInfo = new SchoolEntity() { SchoolID = (long)u.school_id, SchoolName = u.SCHOOL_MASTER.school_name },
                            BatchInfo = new BatchEntity() { BatchTime = u.batch_time, BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : Enums.BatchType.Evening },
                            StudentMaint = new StudentMaint()
                            {
                                ParentName = maint.parent_name,
                                ParentID = maint.parent_id,
                                ContactNo = maint.contact_no,
                                FatherOccupation = maint.father_occupation,
                                MotherOccupation = maint.mother_occupation,
                                StudentID = maint.student_id
                            },
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            return data;
        }

        public async Task<List<StudentEntity>> GetAllStudent(string studName, string contactNo)
        {
            var data = (from u in this.context.STUDENT_MASTER
                        .Include("STD_MASTER")
                        .Include("SCHOOL_MASTER")
                        .Include("BRANCH_MASTER")
                        join maint in this.context.STUDENT_MAINT on u.student_id equals maint.student_id
                        where (string.IsNullOrEmpty(studName) || u.first_name == studName)
                        && (string.IsNullOrEmpty(contactNo) || u.contact_no == contactNo)
                        select new StudentEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            StudentID = u.student_id,
                            Address = u.address,
                            DOB = u.dob,
                            FirstName = u.first_name,
                            LastName = u.last_name,
                            MiddleName = u.middle_name,
                            ContactNo = u.contact_no,
                            LastYearResult = u.last_yr_result,
                            LastYearClassName = u.last_yr_class_name,
                            Grade = u.grade,
                            AdmissionDate = u.admission_date,
                            GrNo = u.gr_no,
                            SchoolTime = u.school_time,
                            StudentImgByte = u.stud_img,

                            //StudImage = u.stud_img.Length > 0 ? Convert.ToBase64String(u.stud_img) : "",
                            StandardInfo = new StandardEntity() { StandardID = u.std_id, Standard = u.STD_MASTER.standard },
                            SchoolInfo = new SchoolEntity() { SchoolID = (long)u.school_id, SchoolName = u.SCHOOL_MASTER.school_name },
                            BatchInfo = new BatchEntity() { BatchTime = u.batch_time, BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : Enums.BatchType.Evening },
                            StudentMaint = new StudentMaint()
                            {
                                ParentName = maint.parent_name,
                                ParentID = maint.parent_id,
                                ContactNo = maint.contact_no,
                                FatherOccupation = maint.father_occupation,
                                MotherOccupation = maint.mother_occupation,
                                StudentID = maint.student_id
                            },
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            foreach (var item in data)
            {
                data[data.IndexOf(item)].StudImage = item.StudentImgByte != null && item.StudentImgByte.Length > 0 ? Convert.ToBase64String(item.StudentImgByte) : "";
            }
            return data;
        }

        public bool RemoveStudent(long StudentID, string lastupdatedby)
        {
            var data = (from u in this.context.STUDENT_MASTER
                        where u.student_id == StudentID
                        select u).FirstOrDefault();
            if (data != null)
            {
                data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                this.context.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<StudentEntity> GetStudentByID(long studenID)
        {
            var data = (from u in this.context.STUDENT_MASTER
                        .Include("STD_MASTER")
                        .Include("SCHOOL_MASTER")
                        .Include("BRANCH_MASTER")
                        join maint in this.context.STUDENT_MAINT on u.student_id equals maint.student_id
                        join usr in this.context.USER_DEF on u.student_id equals usr.student_id into tempUser
                        from user in tempUser.DefaultIfEmpty()
                        where u.student_id == studenID
                        select new StudentEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            StudentID = u.student_id,
                            Address = u.address,
                            DOB = u.dob,
                            FirstName = u.first_name,
                            LastName = u.last_name,
                            MiddleName = u.middle_name,
                            ContactNo = u.contact_no,
                            LastYearResult = u.last_yr_result,
                            LastYearClassName = u.last_yr_class_name,
                            Grade = u.grade,
                            AdmissionDate = u.admission_date,
                            GrNo = u.gr_no,
                            SchoolTime = u.school_time,
                            StudentImgByte = u.stud_img,
                            StudentPassword = user.password,
                            StudentPassword2 = user.password,
                            UserID = user.user_id,
                            //StudImage = u.stud_img.Length > 0 ? Convert.ToBase64String(u.stud_img) : "",
                            StandardInfo = new StandardEntity() { StandardID = u.std_id, Standard = u.STD_MASTER.standard },
                            SchoolInfo = new SchoolEntity() { SchoolID = (long)u.school_id, SchoolName = u.SCHOOL_MASTER.school_name },
                            BatchInfo = new BatchEntity() { BatchTime = u.batch_time, BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : Enums.BatchType.Evening },
                            StudentMaint = new StudentMaint()
                            {
                                ParentName = maint.parent_name,
                                ParentID = maint.parent_id,
                                ContactNo = maint.contact_no,
                                FatherOccupation = maint.father_occupation,
                                MotherOccupation = maint.mother_occupation,
                                StudentID = maint.student_id
                            },
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();
                        data.StudImage = data.StudentImgByte != null && data.StudentImgByte.Length > 0 ? Convert.ToBase64String(data.StudentImgByte) : "";
            //foreach (var item in data)
            //{
            //    data[data.IndexOf(item)].StudImage = item.StudentImgByte != null && item.StudentImgByte.Length > 0 ? Convert.ToBase64String(item.StudentImgByte) : "";
            //}
            if (data != null)
            {
                if (data.UserID ==null)
                {
                    data.UserID = 0;
                }
                if(data.StudentMaint.ParentID != null)
                {
                    var d = (from a in this.context.USER_DEF where a.parent_id == data.StudentMaint.ParentID && a.student_id == data.StudentID select a).FirstOrDefault();
                    if (d != null)
                    {
                        data.StudentMaint.UserID = d.user_id;
                        data.StudentMaint.ParentPassword = d.password;
                        data.StudentMaint.ParentPassword2 = d.password;
                    }
                }
            }
            return data;
        }
    }
}
