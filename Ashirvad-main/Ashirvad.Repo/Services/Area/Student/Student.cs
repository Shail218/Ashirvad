using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area.Student
{
    public class Student : ModelAccess, IStudentAPI
    {
        ResponseModel responseModel = new ResponseModel();
        public async Task<ResponseModel> CheckPackage(long BranchId)
        {
            ResponseModel response = new ResponseModel();
            long result;
            var data = (from student in this.context.BRANCH_RIGHTS_MASTER
                        where student.branch_id == BranchId
                        select new PackageEntity
                        {
                            Studentno = student.PACKAGE_MASTER.student_no
                        }).FirstOrDefault();

            long count = (from u in this.context.STUDENT_MASTER
                          orderby u.student_id descending
                          where u.branch_id == BranchId && u.row_sta_cd == (long)Enums.RowStatus.Active
                          select new StudentEntity()
                          {
                              StudentID = u.student_id
                          }).Count();

            if (data.Studentno <= count)
            {
                response.Status = false;
                response.Message = "You have reached maximum student creation limit, please contact admin for extenstion";
            }
            else
            {
                response.Status = true;
                response.Message = Convert.ToString(count - (long)data.Studentno);
            }
            return response;
        }
        public async Task<ResponseModel> StudentMaintenance(StudentEntity studentInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            Model.STUDENT_MASTER studentMaster = new Model.STUDENT_MASTER();
            Model.STUDENT_MAINT studentMaint = new Model.STUDENT_MAINT();
            //studentMaster.STUDENT_MAINT = new Model.STUDENT_MAINT();
            bool isUpdate = true;
            try
            {
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
                studentMaster.course_dtl_id = studentInfo.BranchCourse.course_dtl_id;
                studentMaster.class_dtl_id = studentInfo.BranchClass.Class_dtl_id;
                studentMaster.school_id = studentInfo.SchoolInfo.SchoolID;
                studentMaster.school_time = studentInfo.SchoolTime;
                studentMaster.batch_time = (int)studentInfo.BatchInfo.BatchType;
                studentMaster.last_yr_result = studentInfo.LastYearResult;
                studentMaster.grade = studentInfo.Grade;
                studentMaster.last_yr_class_name = studentInfo.LastYearClassName;
                studentMaster.contact_no = studentInfo.ContactNo;
                studentMaster.admission_date = studentInfo.AdmissionDate;
                studentMaster.file_name = studentInfo.FileName;
                studentMaster.file_path = studentInfo.FilePath;
                studentMaster.final_year = studentInfo.Final_Year;
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
                var da = this.context.SaveChanges() > 0 ? studentMaster.student_id : 0;
                if (da > 0)
                {
                    studentInfo.StudentID = da;
                    responseModel.Data = studentInfo;
                    responseModel.Message = isUpdate == true ? "Student Updated Successfully." : "Student Inserted Successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.Message = isUpdate == true ? "Student Not Updated." : "Student Not Inserted.";
                    responseModel.Status = false;
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            //return this.context.SaveChanges() > 0 ? studentMaster.student_id : 0;
            return responseModel;
        }
        public async Task<List<StudentEntity>> GetAllStudent(long branchID, int status)
        {
            var data = (from u in this.context.STUDENT_MASTER
                        .Include("STD_MASTER")
                        .Include("SCHOOL_MASTER")
                        .Include("BRANCH_MASTER")
                        join maint in this.context.STUDENT_MAINT on u.student_id equals maint.student_id
                        orderby u.student_id descending
                        where branchID == 0 || u.branch_id == branchID
                        && (0 == status || u.row_sta_cd == status)
                        select new StudentEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Deactive"
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
                            FilePath = "https://mastermind.org.in" + u.file_path,
                            FileName = u.file_name,
                            Final_Year = u.final_year,
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id.HasValue == true ? u.course_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                }
                            },
                            BranchClass = new BranchClassEntity() { Class_dtl_id = u.class_dtl_id.HasValue == true ? u.class_dtl_id.Value : 0, Class = new ClassEntity() { ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name } },
                            SchoolInfo = new SchoolEntity() { SchoolID = (long)u.school_id, SchoolName = u.SCHOOL_MASTER.school_name },
                            BatchInfo = new BatchEntity() { BatchTime = u.batch_time, BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : u.batch_time == 3 ? Enums.BatchType.Evening : u.batch_time == 4 ? Enums.BatchType.Morning2 : u.batch_time == 5 ? Enums.BatchType.Afternoon2 : u.batch_time == 6 ? Enums.BatchType.Evening2 : u.batch_time == 7 ? Enums.BatchType.Morning3 : u.batch_time == 8 ? Enums.BatchType.Afternoon3 : Enums.BatchType.Evening3 },
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
        public async Task<List<StudentEntity>> GetAllStudentByStd(long Std, long Branch, long Batch)
        {
            var data = (from u in this.context.STUDENT_MASTER
                        .Include("STD_MASTER")
                        .Include("SCHOOL_MASTER")
                        .Include("BRANCH_MASTER")
                        where u.class_dtl_id == Std && u.branch_id == Branch && u.batch_time == Batch && u.row_sta_cd == (long)Enums.RowStatus.Active
                        select new StudentEntity()
                        {

                            StudentID = u.student_id,
                            GrNo = u.gr_no,
                            BranchClass = new BranchClassEntity() { Class_dtl_id = u.class_dtl_id.HasValue == true ? u.class_dtl_id.Value : 0, Class = new ClassEntity() { ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name } },
                            SchoolInfo = new SchoolEntity() { SchoolID = (long)u.school_id, SchoolName = u.SCHOOL_MASTER.school_name },
                            BatchInfo = new BatchEntity() { BatchTime = u.batch_time, BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : Enums.BatchType.Evening },
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            Name = u.first_name + " " + u.last_name
                        }).ToList();

            return data;
        }
        public async Task<List<StudentEntity>> GetAllStudentByStd(long Std,long courseId, long Branch, long Batch)
        {
            var data = (from u in this.context.STUDENT_MASTER
                        .Include("STD_MASTER")
                        .Include("SCHOOL_MASTER")
                        .Include("BRANCH_MASTER")
                        where u.class_dtl_id == Std && u.branch_id == Branch && u.batch_time == Batch && u.row_sta_cd == (long)Enums.RowStatus.Active && u.course_dtl_id == courseId
                        select new StudentEntity()
                        {

                            StudentID = u.student_id,
                            GrNo = u.gr_no,
                            BranchClass = new BranchClassEntity() { Class_dtl_id = u.class_dtl_id.HasValue == true ? u.class_dtl_id.Value : 0, Class = new ClassEntity() { ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name } },
                            SchoolInfo = new SchoolEntity() { SchoolID = (long)u.school_id, SchoolName = u.SCHOOL_MASTER.school_name },
                            BatchInfo = new BatchEntity() { BatchTime = u.batch_time, BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : Enums.BatchType.Evening },
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            Name = u.first_name + " " + u.last_name
                        }).ToList();

            return data;
        }
       
        public async Task<List<StudentEntity>> GetAllCustomStudentMarks(DataTableAjaxPostModel model, long Std, long courseid, long Branch, long Batch)
        {
            var Result = new List<StudentEntity>();
            bool Isasc = true;
            if (model.order?.Count > 0)
            {
                Isasc = model.order[0].dir == "desc" ? false : true;
            }
            long count = (from u in this.context.STUDENT_MASTER
                        .Include("STD_MASTER")
                        .Include("SCHOOL_MASTER")
                        .Include("BRANCH_MASTER")
                          orderby u.student_id descending
                          where u.class_dtl_id == Std && u.branch_id == Branch && u.batch_time == Batch && u.row_sta_cd == (long)Enums.RowStatus.Active && u.course_dtl_id == courseid
                          select new StudentEntity()
                          {
                              StudentID = u.student_id
                          }).Count();
            var data = (from u in this.context.STUDENT_MASTER
                        .Include("STD_MASTER")
                        .Include("SCHOOL_MASTER")
                        .Include("BRANCH_MASTER")
                        orderby u.student_id descending
                        where u.class_dtl_id == Std && u.branch_id == Branch && u.batch_time == Batch && u.row_sta_cd == (long)Enums.RowStatus.Active && u.course_dtl_id == courseid
                        && (model.search.value == null
                        || model.search.value == ""
                        || u.first_name.ToLower().Contains(model.search.value.ToLower())
                        || u.last_name.ToLower().Contains(model.search.value.ToLower()))
                        select new StudentEntity()
                        {
                            StudentID = u.student_id,
                            GrNo = u.gr_no,
                            BranchClass = new BranchClassEntity() { Class_dtl_id = u.class_dtl_id.HasValue == true ? u.class_dtl_id.Value : 0, Class = new ClassEntity() { ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name } },
                            SchoolInfo = new SchoolEntity() { SchoolID = (long)u.school_id, SchoolName = u.SCHOOL_MASTER.school_name },
                            BatchInfo = new BatchEntity() { BatchTime = u.batch_time, BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : u.batch_time == 3 ? Enums.BatchType.Evening : u.batch_time == 4 ? Enums.BatchType.Morning2 : u.batch_time == 5 ? Enums.BatchType.Afternoon2 : u.batch_time == 6 ? Enums.BatchType.Evening2 : u.batch_time == 7 ? Enums.BatchType.Morning3 : u.batch_time == 8 ? Enums.BatchType.Afternoon3 : Enums.BatchType.Evening3 },
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id, BranchName = u.BRANCH_MASTER.branch_name },
                            Name = u.first_name + " " + u.last_name,
                            Count = count
                        })
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();
            return data;
        }
        public async Task<List<StudentEntity>> GetAllStudentWithoutContent(long branchID, int status)
        {
            var data = (from u in this.context.STUDENT_MASTER
                        .Include("STD_MASTER")
                        .Include("SCHOOL_MASTER")
                        .Include("BRANCH_MASTER")
                        join maint in this.context.STUDENT_MAINT on u.student_id equals maint.student_id
                        orderby u.student_id descending
                        where branchID == 0 || u.branch_id == branchID
                        && (0 == status || u.row_sta_cd == status)
                        select new StudentEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Deactive"
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
                            FileName = u.file_name,
                            FilePath = "https://mastermind.org.in" + u.file_path,
                            //StudImage = u.stud_img.Length > 0 ? Convert.ToBase64String(u.stud_img) : "",
                            BranchClass = new BranchClassEntity() { Class_dtl_id = u.class_dtl_id.HasValue == true ? u.class_dtl_id.Value : 0, Class = new ClassEntity() { ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name } },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id.HasValue == true ? u.course_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                }
                            },
                            SchoolInfo = new SchoolEntity() { SchoolID = (long)u.school_id, SchoolName = u.SCHOOL_MASTER.school_name },
                            BatchInfo = new BatchEntity() { BatchTime = u.batch_time, BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : u.batch_time == 3 ? Enums.BatchType.Evening : u.batch_time == 4 ? Enums.BatchType.Morning2 : u.batch_time == 5 ? Enums.BatchType.Afternoon2 : u.batch_time == 6 ? Enums.BatchType.Evening2 : u.batch_time == 7 ? Enums.BatchType.Morning3 : u.batch_time == 8 ? Enums.BatchType.Afternoon3 : Enums.BatchType.Evening3 },
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
                var res = this.context.USER_DEF.Where(s => s.student_id == item.StudentID
                && s.row_sta_cd == 1 && s.user_type == (int)Enums.UserType.Student).FirstOrDefault();
                if (res != null)
                {
                    item.StudentPassword = res.password;
                }

                var res2 = this.context.USER_DEF.Where(s => s.student_id == item.StudentID
                && s.row_sta_cd == 1 && s.user_type == (int)Enums.UserType.Parent).FirstOrDefault();
                if (res2 != null)
                {
                    item.StudentPassword2 = res2.password;
                }
                return data;
            }
            return data;
        }
        public async Task<List<StudentEntity>> GetAllStudentWithoutContentByRange(long branchID, int page, int limit)
        {
            var data = (from u in this.context.STUDENT_MASTER
                        join maint in this.context.STUDENT_MAINT on u.student_id equals maint.student_id
                        orderby u.student_id descending
                        where branchID == 0 || u.branch_id == branchID
                        select new StudentEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Deactive"
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
                            FileName = u.file_name,
                            FilePath = "https://mastermind.org.in" + u.file_path,
                            BranchClass = new BranchClassEntity() { Class_dtl_id = u.class_dtl_id.HasValue == true ? u.class_dtl_id.Value : 0, Class = new ClassEntity() { ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name } },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id.HasValue == true ? u.course_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                }
                            },
                            SchoolInfo = new SchoolEntity() { SchoolID = (long)u.school_id, SchoolName = u.SCHOOL_MASTER.school_name },
                            BatchInfo = new BatchEntity() { BatchTime = u.batch_time, BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : u.batch_time == 3 ? Enums.BatchType.Evening : u.batch_time == 4 ? Enums.BatchType.Morning2 : u.batch_time == 5 ? Enums.BatchType.Afternoon2 : u.batch_time == 6 ? Enums.BatchType.Evening2 : u.batch_time == 7 ? Enums.BatchType.Morning3 : u.batch_time == 8 ? Enums.BatchType.Afternoon3 : Enums.BatchType.Evening3 },
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
                        }).Skip(page).Take(limit).ToList();
            return data;
        }
        public async Task<List<StudentEntity>> GetAllStudentsName(long branchID, long stdid, long courseid, int batchtime)
        {
            var data = (from u in this.context.STUDENT_MASTER
                        orderby u.student_id descending
                        where u.branch_id == branchID && u.row_sta_cd == 1 && u.batch_time == batchtime && u.class_dtl_id == stdid && u.course_dtl_id == courseid
                        select new StudentEntity()
                        {
                            StudentID = u.student_id,
                            Name = u.first_name + " " + u.last_name,
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
                            FileName = u.file_name,
                            FilePath = u.file_path,
                            //StudImage = u.stud_img.Length > 0 ? Convert.ToBase64String(u.stud_img) : "",
                            BranchClass = new BranchClassEntity() { Class_dtl_id = u.class_dtl_id.HasValue == true ? u.class_dtl_id.Value : 0, Class = new ClassEntity() { ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name } },
                            SchoolInfo = new SchoolEntity() { SchoolID = (long)u.school_id, SchoolName = u.SCHOOL_MASTER.school_name },
                            BatchInfo = new BatchEntity() { BatchTime = u.batch_time, BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : u.batch_time == 3 ? Enums.BatchType.Evening : u.batch_time == 4 ? Enums.BatchType.Morning2 : u.batch_time == 5 ? Enums.BatchType.Afternoon2 : u.batch_time == 6 ? Enums.BatchType.Evening2 : u.batch_time == 7 ? Enums.BatchType.Morning3 : u.batch_time == 8 ? Enums.BatchType.Afternoon3 : Enums.BatchType.Evening3 },
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
        public ResponseModel RemoveStudent(long StudentID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {


                var data = (from u in this.context.STUDENT_MASTER
                            where u.student_id == StudentID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                    responseModel.Message = "Student Removed Successfully";
                    responseModel.Status = true;
                    //return true;
                }
                else
                {
                    responseModel.Message = "Student Not Found";
                    responseModel.Status = false;
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            //return false;
            return responseModel;
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
                            FileName = u.file_name,
                            FilePath = u.file_path,
                            StudentPassword = user.password,
                            StudentPassword2 = user.password,
                            UserID = user.user_id,
                            Final_Year = u.final_year,
                            //StudImage = u.stud_img.Length > 0 ? Convert.ToBase64String(u.stud_img) : "",
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id.HasValue == true ? u.course_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                }
                            },
                            BranchClass = new BranchClassEntity() { Class_dtl_id = u.class_dtl_id.HasValue == true ? u.class_dtl_id.Value : 0, Class = new ClassEntity() { ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name } },
                            SchoolInfo = new SchoolEntity() { SchoolID = (long)u.school_id, SchoolName = u.SCHOOL_MASTER.school_name },
                            BatchInfo = new BatchEntity() { BatchTime = u.batch_time, BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : u.batch_time == 3 ? Enums.BatchType.Evening : u.batch_time == 4 ? Enums.BatchType.Morning2 : u.batch_time == 5 ? Enums.BatchType.Afternoon2 : u.batch_time == 6 ? Enums.BatchType.Evening2 : u.batch_time == 7 ? Enums.BatchType.Morning3 : u.batch_time == 8 ? Enums.BatchType.Afternoon3 : Enums.BatchType.Evening3 },
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
           // data.StudImage = data.StudentImgByte != null && data.StudentImgByte.Length > 0 ? Convert.ToBase64String(data.StudentImgByte) : "";
            if (data != null)
            {
                if (data.UserID == null)
                {
                    data.UserID = 0;
                }
                if (data.StudentMaint.ParentID != 0)
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
        public async Task<List<StudentEntity>> GetAllCustomStudent(DataTableAjaxPostModel model, long branchID, int status)
        {
            var Result = new List<StudentEntity>();
            bool Isasc = true;
            if (model.order?.Count > 0)
            {
                Isasc = model.order[0].dir == "desc" ? false : true;
            }
            long count = this.context.STUDENT_MASTER.Where(s => (0 == status || s.row_sta_cd == status) && s.branch_id == branchID).Distinct().Count();
            var data = (from u in this.context.STUDENT_MASTER
                        .Include("STD_MASTER")
                        .Include("SCHOOL_MASTER")
                        .Include("BRANCH_MASTER")
                        join maint in this.context.STUDENT_MAINT on u.student_id equals maint.student_id
                        orderby u.student_id descending
                        where branchID == 0 || u.branch_id == branchID
                        && (0 == status || u.row_sta_cd == status)
                        && (model.search.value == null
                        || model.search.value == ""
                        || u.first_name.ToLower().Contains(model.search.value.ToLower())
                        || u.last_name.ToLower().Contains(model.search.value.ToLower())
                        || u.CLASS_DTL_MASTER.CLASS_MASTER.class_name.ToLower().Contains(model.search.value.ToLower())
                        || u.contact_no.ToLower().Contains(model.search.value.ToLower())
                        || u.admission_date.ToString().ToLower().Contains(model.search.value.ToLower()))
                        select new StudentEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Deactive"
                            },
                            StudentID = u.student_id,
                            Address = u.address,
                            DOB = u.dob,
                            FirstName = u.first_name,
                            LastName = u.last_name,
                            Name = u.first_name + " " + u.last_name,
                            MiddleName = u.middle_name,
                            ContactNo = u.contact_no,
                            LastYearResult = u.last_yr_result,
                            LastYearClassName = u.last_yr_class_name,
                            Grade = u.grade,
                            AdmissionDate = u.admission_date,
                            GrNo = u.gr_no,
                            Count = count,
                            SchoolTime = u.school_time,
                            FileName = u.file_name,
                            FilePath = "https://mastermind.org.in" + u.file_path,
                            Final_Year = u.final_year,
                            //StudImage = u.stud_img.Length > 0 ? Convert.ToBase64String(u.stud_img) : "",
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id.HasValue == true ? u.course_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                }
                            },
                            BranchClass = new BranchClassEntity() { Class_dtl_id = u.class_dtl_id.HasValue == true ? u.class_dtl_id.Value : 0, Class = new ClassEntity() { ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name } },
                            SchoolInfo = new SchoolEntity() { SchoolID = (long)u.school_id, SchoolName = u.SCHOOL_MASTER.school_name },
                            BatchInfo = new BatchEntity()
                            {
                                BatchTime = u.batch_time,
                                BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : u.batch_time == 3 ? Enums.BatchType.Evening : u.batch_time == 4 ? Enums.BatchType.Morning2 : u.batch_time == 5 ? Enums.BatchType.Afternoon2 : u.batch_time == 6 ? Enums.BatchType.Evening2 : u.batch_time == 7 ? Enums.BatchType.Morning3 : u.batch_time == 8 ? Enums.BatchType.Afternoon3 : Enums.BatchType.Evening3,
                                BatchText = u.batch_time == 1 ? "Morning" : u.batch_time == 2 ? "Afternoon" : u.batch_time == 3 ? "Evening" : u.batch_time == 4 ? "Morning2" : u.batch_time == 5 ? "Afternoon2" : u.batch_time == 6 ? "Evening2" : u.batch_time == 7 ? "Morning3" : u.batch_time == 8 ? "Afternoon3" : "Evening3"
                            },
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
                        })
                        .OrderByDescending(s => s.StudentMaint.StudentID)
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();
            foreach (var item in data)
            {
                var res = this.context.USER_DEF.Where(s => s.student_id == item.StudentID
                && s.row_sta_cd == 1 && s.user_type == (int)Enums.UserType.Student).FirstOrDefault();
                if (res != null)
                {
                    item.StudentPassword = res.password;
                }
                var res2 = this.context.USER_DEF.Where(s => s.student_id == item.StudentID
                && s.row_sta_cd == 1 && s.user_type == (int)Enums.UserType.Parent).FirstOrDefault();
                if (res2 != null)
                {
                    item.StudentPassword2 = res2.password;
                }
                return data;
            }
            return data;
        }
        public async Task<List<StudentEntity>> GetFilterStudent(long course, long classname, string finalyear, long branchID)
        {
            var data = (from u in this.context.STUDENT_MASTER
                        .Include("STD_MASTER")
                        .Include("SCHOOL_MASTER")
                        .Include("BRANCH_MASTER")
                        join maint in this.context.STUDENT_MAINT on u.student_id equals maint.student_id
                        orderby u.student_id descending
                        where (branchID == 0 || u.branch_id == branchID) &&
                        (u.row_sta_cd==1) && u.course_dtl_id==course && u.class_dtl_id==classname
                        select new StudentEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Deactive"
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
                            FilePath = "https://mastermind.org.in" + u.file_path,
                            FileName = u.file_name,
                            Final_Year = u.final_year,
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id.HasValue == true ? u.course_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                }
                            },
                            BranchClass = new BranchClassEntity() { Class_dtl_id = u.class_dtl_id.HasValue == true ? u.class_dtl_id.Value : 0, Class = new ClassEntity() { ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name } },
                            SchoolInfo = new SchoolEntity() { SchoolID = (long)u.school_id, SchoolName = u.SCHOOL_MASTER.school_name },
                            BatchInfo = new BatchEntity() { BatchTime = u.batch_time, BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : u.batch_time == 3 ? Enums.BatchType.Evening : u.batch_time == 4 ? Enums.BatchType.Morning2 : u.batch_time == 5 ? Enums.BatchType.Afternoon2 : u.batch_time == 6 ? Enums.BatchType.Evening2 : u.batch_time == 7 ? Enums.BatchType.Morning3 : u.batch_time == 8 ? Enums.BatchType.Afternoon3 : Enums.BatchType.Evening3 },
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
        
        public async Task<List<StudentEntity>> GetFilterStudentStatusWise(long course, long classname, int status, long branchID)
        {
            var data = (from u in this.context.STUDENT_MASTER
                        .Include("STD_MASTER")
                        .Include("SCHOOL_MASTER")
                        .Include("BRANCH_MASTER")
                        join maint in this.context.STUDENT_MAINT on u.student_id equals maint.student_id
                        orderby u.student_id descending
                        where (branchID == 0 || u.branch_id == branchID) &&
                        (u.row_sta_cd==status) && u.course_dtl_id==course && u.class_dtl_id==classname
                        select new StudentEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Deactive"
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
                            FilePath = "https://mastermind.org.in" + u.file_path,
                            FileName = u.file_name,
                            Final_Year = u.final_year,
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id.HasValue == true ? u.course_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                }
                            },
                            BranchClass = new BranchClassEntity() { Class_dtl_id = u.class_dtl_id.HasValue == true ? u.class_dtl_id.Value : 0, Class = new ClassEntity() { ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name } },
                            SchoolInfo = new SchoolEntity() { SchoolID = (long)u.school_id, SchoolName = u.SCHOOL_MASTER.school_name },
                            BatchInfo = new BatchEntity() { BatchTime = u.batch_time, BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : u.batch_time == 3 ? Enums.BatchType.Evening : u.batch_time == 4 ? Enums.BatchType.Morning2 : u.batch_time == 5 ? Enums.BatchType.Afternoon2 : u.batch_time == 6 ? Enums.BatchType.Evening2 : u.batch_time == 7 ? Enums.BatchType.Morning3 : u.batch_time == 8 ? Enums.BatchType.Afternoon3 : Enums.BatchType.Evening3 },
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

        public async Task<ResponseModel> ChangeStudentStatus(long StudentID, string lastupdatedby,int status)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {


                var data = (from u in this.context.STUDENT_MASTER
                            where u.student_id == StudentID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    data.row_sta_cd = status;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                    responseModel.Message = status==1?"Student Activated Successfully.": "Student Inactivated Successfully.";
                    responseModel.Status = true;
                    //return true;
                }
                else
                {
                    responseModel.Message = "Student Not Found";
                    responseModel.Status = false;
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            //return false;
            return responseModel;
        }

        public async Task<ResponseModel> StudentTransferMaintenance(StudentEntity studentInfo)
        {
            try
            {
                
                Model.STUDENT_MASTER studentMaster = new Model.STUDENT_MASTER();
                Model.STUDENT_MASTER studentMaster1 = new Model.STUDENT_MASTER();
                Model.STUDENT_MAINT studentMaint = new Model.STUDENT_MAINT();
                Model.USER_DEF userdef = new Model.USER_DEF();
                //studentMaster.STUDENT_MAINT = new Model.STUDENT_MAINT();
                bool isUpdate = true;
                var data = (from student in this.context.STUDENT_MASTER
                            join maint in this.context.STUDENT_MAINT on student.student_id equals maint.student_id
                            join userdefd in this.context.USER_DEF on student.student_id equals userdefd.student_id
                            where student.student_id == studentInfo.StudentID
                            select new
                            {
                                studentMaster = student,
                                studentMaint = maint,
                                userdef= userdefd
                            }).FirstOrDefault();
                if (data != null)
                {
                    
                    studentMaster = data.studentMaster;
                    studentMaint = data.studentMaint;
                    //var Check = CheckPackage(studentMaster.branch_id);
                    //{
                        studentInfo.Transaction.TransactionId = studentMaster.trans_id;
                        studentMaster.row_sta_cd = (int)Enums.RowStatus.Inactive;
                        studentMaster.final_year = studentInfo.Final_Year;
                        studentMaster.trans_id = this.AddTransactionData(studentInfo.Transaction);
                        this.context.STUDENT_MASTER.Add(studentMaster);
                        this.context.Entry(studentMaster).State = System.Data.Entity.EntityState.Modified;
                        var result = this.context.SaveChanges();


                        var data2 = (from student in this.context.USER_DEF
                                     where student.student_id == studentInfo.StudentID
                                     select new
                                     {
                                         studentMaster = student,

                                     }).FirstOrDefault();

                        userdef = data2.studentMaster;
                        userdef.row_sta_cd = (int)Enums.RowStatus.Inactive;
                        this.context.USER_DEF.Add(userdef);
                        this.context.Entry(userdef).State = System.Data.Entity.EntityState.Modified;
                        result = this.context.SaveChanges();



                        if (result > 0)
                        {
                            studentInfo.BranchInfo = new BranchEntity();
                            studentInfo.GrNo = studentMaster.gr_no;
                            studentInfo.FirstName = studentMaster.first_name;
                            studentInfo.MiddleName = studentMaster.middle_name;
                            studentInfo.LastName = studentMaster.last_name;
                            studentInfo.DOB = studentMaster.dob;
                            studentInfo.AdmissionDate = studentMaster.admission_date;
                            studentInfo.Address = studentMaster.address;
                            studentInfo.BranchInfo.BranchID = studentMaster.branch_id;
                            studentInfo.BranchCourse.course_dtl_id = studentInfo.BranchCourse.course_dtl_id;
                            studentInfo.BranchClass.Class_dtl_id = studentInfo.BranchClass.Class_dtl_id;
                            studentInfo.SchoolInfo.SchoolID = studentMaster.school_id.HasValue ? studentMaster.school_id.Value : 0;
                            studentInfo.SchoolTime = studentMaster.school_time;
                            studentInfo.BatchInfo.BatchType = (Enums.BatchType)studentMaster.batch_time;
                            studentInfo.LastYearResult = studentMaster.last_yr_result;
                            studentInfo.Grade = studentMaster.grade;
                            studentInfo.LastYearClassName = studentMaster.last_yr_class_name;
                            studentInfo.ContactNo = studentMaster.contact_no;
                            studentInfo.AdmissionDate = studentMaster.admission_date;
                            studentInfo.FileName = studentMaster.file_name;
                            studentInfo.FilePath = studentMaster.file_path;
                            studentInfo.Final_Year = studentMaster.final_year;
                            studentInfo.RowStatus.RowStatusId = 1;
                            studentInfo.StudentID = 0;
                            studentInfo.StudentPassword = userdef.password;
                            studentInfo.StudentMaint = new StudentMaint()
                            {
                                ParentName = studentMaint.parent_name,
                                FatherOccupation = studentMaint.father_occupation,
                                MotherOccupation = studentMaint.mother_occupation,
                                ContactNo = studentMaint.contact_no
                            };
                            var result1 = StudentMaintenance(studentInfo).Result;

                            if (result1.Status)
                            {
                                var Student = (StudentEntity)result1.Data;
                                responseModel.Status = true;
                                responseModel.Message = "Student Trasnfer Successfully!!";
                                responseModel.Data = Student.StudentID;
                            }
                            else
                            {
                                responseModel.Status = false;
                                responseModel.Message = "Failed To  Transfer Student!!";
                            }

                        }
                        else
                        {
                            responseModel.Status = false;
                            responseModel.Message = "Failed To  Transfer Student!!";

                        }
                    //}
                   
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message;
            }

            return responseModel;

        }

    }
}
