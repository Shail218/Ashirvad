using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Faculty;
using Ashirvad.Repo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Faculty
{
    public class Faculty : ModelAccess, IFacultyAPI
    {
        public async Task<long> CheckFaculty(long staffid, long courseid, long classid, long subjectid, long branchid, long facultyid)
        {
            long result;
            bool isExists = this.context.FACULTY_MASTER.Where(s => (facultyid == 0 || s.faculty_id != facultyid) && s.branch_id == branchid && s.staff_id == staffid && s.course_dtl_id == courseid && s.class_dtl_id == classid && s.subject_dtl_id == subjectid && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<long> FacultyMaintenance(FacultyEntity facultyInfo)
        {
            Model.FACULTY_MASTER facultymaster = new Model.FACULTY_MASTER();
            if (CheckFaculty(facultyInfo.staff.StaffID, facultyInfo.BranchCourse.course_dtl_id, facultyInfo.BranchClass.Class_dtl_id,
                facultyInfo.branchSubject.Subject_dtl_id, facultyInfo.BranchInfo.BranchID, facultyInfo.FacultyID).Result != -1)
            {
                bool isUpdate = true;
                var data = (from facul in this.context.FACULTY_MASTER
                            where facul.faculty_id == facultyInfo.FacultyID
                            select facul).FirstOrDefault();
                if (data == null)
                {
                    data = new Model.FACULTY_MASTER();
                    isUpdate = false;
                }
                else
                {
                    facultymaster = data;
                    isUpdate = true;
                    facultyInfo.Transaction.TransactionId = data.trans_id;

                }
                facultymaster.staff_id = facultyInfo.staff.StaffID;
                facultymaster.course_dtl_id = facultyInfo.BranchCourse.course_dtl_id;
                facultymaster.class_dtl_id = facultyInfo.BranchClass.Class_dtl_id;
                facultymaster.subject_dtl_id = facultyInfo.branchSubject.Subject_dtl_id;
                facultymaster.description = facultyInfo.Descripation;
                facultymaster.file_name = facultyInfo.FacultyContentFileName;
                facultymaster.file_path = facultyInfo.FilePath;
                facultymaster.row_sta_cd = facultyInfo.RowStatus.RowStatusId;
                facultymaster.trans_id = this.AddTransactionData(facultyInfo.Transaction);
                facultymaster.branch_id = facultyInfo.BranchInfo.BranchID;
                this.context.FACULTY_MASTER.Add(facultymaster);
                if (isUpdate)
                {
                    this.context.Entry(facultymaster).State = System.Data.Entity.EntityState.Modified;
                }

                return this.context.SaveChanges() > 0 ? facultymaster.faculty_id : 0;
            }

            return -1;
        }

        public async Task<List<FacultyEntity>> GetAllFaculty(long branchID)
        {
            var data = (from u in this.context.FACULTY_MASTER
                          .Include("COURSE_DTL_MASTER")
                          .Include("CLASS_DTL_MASTER")
                          .Include("SUBJECT_DTL_MASTER")
                          .Include("BRANCH_MASTER")
                          .Include("BRANCH_STAFF")
                        where branchID == 0 || u.branch_id == branchID && u.row_sta_cd == 1
                        select new FacultyEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id,
                                course = new CourseEntity()
                                {
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                }
                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id,
                                Class = new ClassEntity()
                                {
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                }
                            },
                            branchSubject = new BranchSubjectEntity()
                            {
                                Subject_dtl_id = u.subject_dtl_id,
                                Subject = new SuperAdminSubjectEntity()
                                {
                                    SubjectName = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name
                                }
                            },
                            staff = new StaffEntity()
                            {
                                StaffID = u.BRANCH_STAFF.staff_id,
                                Name = u.BRANCH_STAFF.name,
                                DOB = u.BRANCH_STAFF.dob,
                                Education = u.BRANCH_STAFF.education,
                                EmailID = u.BRANCH_STAFF.email_id,
                                Address = u.BRANCH_STAFF.address,
                                MobileNo = u.BRANCH_STAFF.mobile_no,
                            },
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity()
                            {
                                TransactionId = u.trans_id
                            },

                            FilePath = "http://highpack-001-site12.dtempurl.com" + u.file_path,
                            FacultyContentFileName = u.file_name,
                            FacultyID = u.faculty_id,
                            Descripation = u.description,
                        }).ToList();

            return data;
        }

        public async Task<List<FacultyEntity>> GetAllFaculty(long branchID, int typeID)
        {
            var data = (from u in this.context.FACULTY_MASTER
                         .Include("COURSE_DTL_MASTER")
                          .Include("CLASS_DTL_MASTER")
                          .Include("SUBJECT_DTL_MASTER")
                         .Include("BRANCH_MASTER")
                         .Include("BRANCH_STAFF")
                        where branchID == 0 || u.branch_id == branchID && u.row_sta_cd == 1
                        select new FacultyEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id,
                                course = new CourseEntity()
                                {
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                }
                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id,
                                Class = new ClassEntity()
                                {
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                }
                            },
                            branchSubject = new BranchSubjectEntity()
                            {
                                Subject_dtl_id = u.subject_dtl_id,
                                Subject = new SuperAdminSubjectEntity()
                                {
                                    SubjectName = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name
                                }
                            },
                            staff = new StaffEntity()
                            {
                                StaffID = u.BRANCH_STAFF.staff_id,
                                Name = u.BRANCH_STAFF.name
                            },
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity()
                            {
                                TransactionId = u.trans_id
                            },
                            FacultyID = u.faculty_id,
                            HeaderImageText = u.file_name,
                            FilePath = u.file_path,
                        }).ToList();

            return data;
        }

        public async Task<FacultyEntity> GetFacultyByFacultyID(long facultyID)
        {
            var data = (from u in this.context.FACULTY_MASTER
                       .Include("COURSE_DTL_MASTER")
                          .Include("CLASS_DTL_MASTER")
                          .Include("SUBJECT_DTL_MASTER")
                         .Include("BRANCH_MASTER")
                         .Include("BRANCH_STAFF")
                        where u.faculty_id == facultyID
                        select new FacultyEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            FacultyID = u.faculty_id,
                            Descripation = u.description,
                            FacultyContentFileName = u.file_name,
                            FilePath = u.file_path,
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id,
                                course = new CourseEntity()
                                {
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                }
                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id,
                                Class = new ClassEntity()
                                {
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                }
                            },
                            branchSubject = new BranchSubjectEntity()
                            {
                                Subject_dtl_id = u.subject_dtl_id,
                                Subject = new SuperAdminSubjectEntity()
                                {
                                    SubjectName = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name
                                }
                            },
                            staff = new StaffEntity()
                            {
                                StaffID = u.BRANCH_STAFF.staff_id,
                                Name = u.BRANCH_STAFF.name
                            },
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity()
                            {
                                TransactionId = u.trans_id
                            },
                        }).FirstOrDefault();
            return data;
        }

        public bool RemoveFaculty(long faculID, string lastupdatedby)
        {
            var data = (from u in this.context.FACULTY_MASTER
                        where u.faculty_id == faculID
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
        //website
        public async Task<List<FacultyEntity>> GetAllFacultyWebsite(long branchID, long courseID, long classID, long subjectID)
        {
            List<FacultyEntity> faculties = new List<FacultyEntity>();
            faculties = (from u in this.context.getfacultydetailbybranch(classID,courseID,subjectID)
                         // .Include("BRANCH_MASTER")
                         // .Include("BRANCH_STAFF")
                         //.Include("SUBJECT_DTL_MASTER")
                         //where courseID == 0 || u.COURSE_DTL_MASTER.course_id == courseID &&
                         //classID == 0 || u.CLASS_DTL_MASTER.class_id == classID &&
                         //subjectID == 0 || u.SUBJECT_DTL_MASTER.subject_id == subjectID && u.row_sta_cd == 1
                         select new FacultyEntity()
                         {
                             BranchInfo = new BranchEntity()
                             {
                                // branchid = u.branch_master.branch_id,
                                 BranchName = u.branch_name
                             },
                             staff = new StaffEntity()
                             {
                               //  StaffID = u.staff_id,
                                 Name = u.name
                             },
                             branchSubject = new BranchSubjectEntity()
                             {
                                 Subject_dtl_id = u.subject_id,
                                 
                                 Subject = new SuperAdminSubjectEntity()
                                 {
                                     SubjectName = u.subject_name
                                 }
                             },
                             FacultyID = u.faculty_id,
                             FilePath = "http://highpack-001-site12.dtempurl.com" + u.file_path,
                            // FacultyContentFileName=u.file_name,
                         }).ToList();
            foreach (var item in faculties)
            {
                item.facultylist = (from u in this.context.getfacultydetailbybranch(classID,courseID,subjectID)
                         //.Include("COURSE_DTL_MASTER")
                         //.Include("CLASS_DTL_MASTER")
                         //.Include("SUBJECT_DTL_MASTER")
                         //.Include("BRANCH_MASTER")
                         //.Include("BRANCH_STAFF")
                         //           where branchID == 0 || u.branch_id == item.BranchInfo.BranchID &&
                         //              courseID == 0 || u.COURSE_DTL_MASTER.course_id == courseID &&
                         //              classID == 0 || u.CLASS_DTL_MASTER.class_id == classID &&
                         //              subjectID == 0 || u.SUBJECT_DTL_MASTER.subject_id == subjectID
                         //              && u.row_sta_cd == 1
                                    select new FacultyEntity()
                                    {
                                        //RowStatus = new RowStatusEntity()
                                        //{
                                        //    RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                        //    RowStatusId = u.row_sta_cd
                                        //},
                                        //BranchCourse = new BranchCourseEntity()
                                        //{
                                        //    course_dtl_id = u.course_dtl_id,
                                        //    course = new CourseEntity()
                                        //    {
                                        //        CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                        //    }
                                        //},
                                        //BranchClass = new BranchClassEntity()
                                        //{
                                        //    Class_dtl_id = u.class_dtl_id,
                                        //    Class = new ClassEntity()
                                        //    {
                                        //        ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                        //    }
                                        //},
                                        //branchSubject = new BranchSubjectEntity()
                                        //{
                                        //    Subject_dtl_id = u.subject_dtl_id,
                                        //    Subject = new SuperAdminSubjectEntity()
                                        //    {
                                        //        SubjectName = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name
                                        //    }
                                        //},
                                        //staff = new StaffEntity()
                                        //{
                                        //    StaffID = u.staff_id,
                                        //    Name = u.name,
                                            //DOB = u.BRANCH_STAFF.dob,
                                            //Education = u.BRANCH_STAFF.education,
                                            //EmailID = u.BRANCH_STAFF.email_id,
                                            //Address = u.BRANCH_STAFF.address,
                                            //MobileNo = u.BRANCH_STAFF.mobile_no,
                                       // },
                                        //BranchInfo = new BranchEntity()
                                        //{
                                        //    BranchID = u.BRANCH_MASTER.branch_id,
                                        //    BranchName = u.BRANCH_MASTER.branch_name
                                        //},
                                        //Transaction = new TransactionEntity()
                                        //{
                                        //    TransactionId = u.trans_id
                                        //},

                                        FilePath = "http://highpack-001-site12.dtempurl.com"+ u.file_path,
                                      //  FacultyContentFileName = u.file_name,
                                      //  FacultyID = u.faculty_id,
                                      //  Descripation = u.description,
                                    }).ToList();
            }



            return faculties;
        }

        public async Task<List<FacultyEntity>> GetFacultyDetail(long facultyID)
        {
            var data = (from u in this.context.FACULTY_MASTER
                          .Include("COURSE_DTL_MASTER")
                          .Include("CLASS_DTL_MASTER")
                          .Include("SUBJECT_DTL_MASTER")
                          .Include("BRANCH_MASTER")
                          .Include("BRANCH_STAFF")
                        where facultyID == 0 || u.faculty_id == facultyID && u.row_sta_cd == 1
                        select new FacultyEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id,
                                course = new CourseEntity()
                                {
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                }
                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id,
                                Class = new ClassEntity()
                                {
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                }
                            },
                            branchSubject = new BranchSubjectEntity()
                            {
                                Subject_dtl_id = u.subject_dtl_id,
                                Subject = new SuperAdminSubjectEntity()
                                {
                                    SubjectName = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name
                                }
                            },
                            staff = new StaffEntity()
                            {
                                StaffID = u.BRANCH_STAFF.staff_id,
                                Name = u.BRANCH_STAFF.name,
                                DOB = u.BRANCH_STAFF.dob,
                                Education = u.BRANCH_STAFF.education,
                                EmailID = u.BRANCH_STAFF.email_id,
                                Address = u.BRANCH_STAFF.address,
                                MobileNo = u.BRANCH_STAFF.mobile_no,
                            },
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity()
                            {
                                TransactionId = u.trans_id
                            },

                            FilePath = "http://highpack-001-site12.dtempurl.com" + u.file_path,
                            FacultyContentFileName = u.file_name,
                            FacultyID = u.faculty_id,
                            Descripation = u.description,
                        }).ToList();

            return data;
        }

    }
}
