using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area
{
    public class Marks : ModelAccess, IMarksAPI
    {
        public async Task<long> CheckMarks(long BranchID, long StudentID, long TestID, long MarksID)
        {
            long result;
            bool isExists = this.context.MARKS_MASTER.Where(s => (MarksID == 0 || s.marks_id != MarksID) && s.branch_id == BranchID && s.student_id == StudentID && s.test_id == TestID && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<ResponseModel> MarksMaintenance(MarksEntity MarksInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            Model.MARKS_MASTER MarksMaster = new Model.MARKS_MASTER();
            bool isUpdate = true;
            try
            {
                var data = (from Marks in this.context.MARKS_MASTER
                            where Marks.marks_id == MarksInfo.MarksID
                            select new
                            {
                                MarksMaster = Marks
                            }).FirstOrDefault();
                if (data == null)
                {
                    MarksMaster = new Model.MARKS_MASTER();
                    isUpdate = false;
                }
                else
                {
                    MarksMaster = data.MarksMaster;
                    MarksInfo.Transaction.TransactionId = data.MarksMaster.trans_id;
                }
                MarksMaster.student_id = MarksInfo.student.StudentID;
                MarksMaster.branch_id = MarksInfo.BranchInfo.BranchID;
                MarksMaster.row_sta_cd = MarksInfo.RowStatus.RowStatusId;
                MarksMaster.achive_marks = MarksInfo.AchieveMarks;
                MarksMaster.test_id = MarksInfo.testEntityInfo.TestID;
                MarksMaster.file_name = MarksInfo.MarksContentFileName;
                MarksMaster.file_path = MarksInfo.MarksFilepath;
                MarksMaster.marks_dt = MarksInfo.MarksDate;
                MarksMaster.course_dtl_id = MarksInfo.BranchCourse.course_dtl_id;
                MarksMaster.class_dtl_id = MarksInfo.BranchClass.Class_dtl_id;
                MarksMaster.subject_dtl_id = MarksInfo.BranchSubject.Subject_dtl_id;
                MarksMaster.batch_time_id = (int)MarksInfo.BatchType;
                MarksMaster.trans_id = this.AddTransactionData(MarksInfo.Transaction);
                this.context.MARKS_MASTER.Add(MarksMaster);
                if (isUpdate)
                {
                    this.context.Entry(MarksMaster).State = System.Data.Entity.EntityState.Modified;
                }
                //return this.context.SaveChanges() > 0 ? MarksMaster.marks_id : 0;
                var da = this.context.SaveChanges() > 0 || MarksMaster.marks_id > 0;
                if (da)
                {
                    MarksInfo.MarksID = MarksMaster.marks_id;
                    responseModel.Message = isUpdate == true ? "Marks Updated." : "Marks Inserted Successfully";
                    responseModel.Status = true;
                    responseModel.Data = MarksInfo;
                }
                else
                {
                    responseModel.Message = isUpdate == true ? "Marks Not Updated." : "Marks Not Inserted Successfully";
                    responseModel.Status = false;
                }
                
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            return responseModel;

        }

        public async Task<List<MarksEntity>> GetAllMarks()
        {
            var data = (from u in this.context.MARKS_MASTER orderby u.marks_id descending
                        where u.row_sta_cd == 1
                        select new MarksEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            MarksID = u.marks_id,                         
                            MarksDate= u.marks_dt,
                            AchieveMarks= u.achive_marks,
                            MarksFilepath = u.file_path,
                            MarksContentFileName = u.file_name,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id },
                            testEntityInfo = new TestEntity() { TestID = u.test_id}
                        }).ToList();

            return data;
        }

        public async Task<List<MarksEntity>> GetAllAchieveMarks(long Std, long Branch, long Batch, long MarksID)
        {
            var data = (from u in this.context.MARKS_MASTER
                        .Include("STUDENT_MASTER")
                        .Include("TEST_MASTER")
                        .Include("SUBJECT_MASTER")
                        orderby u.marks_id descending
                        where u.branch_id == Branch && u.subject_dtl_id == MarksID && (u.test_id == Std || Std == 0) && (u.batch_time_id == Batch || Batch == 0) && u.row_sta_cd == 1
                        select new MarksEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            MarksID = u.marks_id,
                            MarksDate = u.marks_dt,
                            AchieveMarks = u.achive_marks,
                            MarksContentFileName = u.file_name,
                            MarksFilepath = u.file_path,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id },
                            testEntityInfo = new TestEntity() {
                                TestID = u.test_id,
                                Marks = u.TEST_MASTER.total_marks,
                            },
                            student = new StudentEntity()
                            {
                                StudentID = u.student_id,
                                Name = u.STUDENT_MASTER.first_name + " " + u.STUDENT_MASTER.last_name
                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                }
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id.HasValue ? u.course_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                }
                            },
                            BranchSubject = new BranchSubjectEntity()
                            {
                                Subject_dtl_id = u.subject_dtl_id.HasValue ? u.subject_dtl_id.Value : 0,
                                Subject = new SuperAdminSubjectEntity()
                                {
                                    SubjectID = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_id,
                                    SubjectName = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name
                                }
                            }
                        }).ToList();

            return data;
        }

        public async Task<List<MarksEntity>> GetAllCustomMarks(DataTableAjaxPostModel model, long Std,long courseid, long Branch, long Batch, long MarksID)
        {
            var Result = new List<MarksEntity>();
            bool Isasc = true;
            if (model.order?.Count > 0)
            {
                Isasc = model.order[0].dir == "desc" ? false : true;
            }
            long count = (from u in this.context.MARKS_MASTER
                        .Include("STUDENT_MASTER")
                        .Include("TEST_MASTER")
                        .Include("SUBJECT_MASTER")
                          orderby u.marks_id descending
                          where u.branch_id == Branch && u.subject_dtl_id == MarksID &&
                          (u.test_id == Std || Std == 0) && (u.batch_time_id == Batch || Batch == 0) && u.row_sta_cd == 1 && u.course_dtl_id == courseid
                          select new MarksEntity()
                          {
                              MarksID = u.marks_id
                          }).Count();
            var data = (from u in this.context.MARKS_MASTER
                        .Include("STUDENT_MASTER")
                        .Include("TEST_MASTER")
                        .Include("SUBJECT_MASTER")
                        orderby u.marks_id descending
                        where u.branch_id == Branch && u.subject_dtl_id == MarksID && (u.test_id == Std || Std == 0) && (u.batch_time_id == Batch || Batch == 0) && u.row_sta_cd == 1 && u.course_dtl_id == courseid
                        && (model.search.value == null
                        || model.search.value == ""
                        || u.STUDENT_MASTER.first_name.ToLower().Contains(model.search.value.ToLower())
                        || u.STUDENT_MASTER.last_name.ToLower().Contains(model.search.value.ToLower())
                        || u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name.ToLower().Contains(model.search.value.ToLower())
                        || u.achive_marks.ToLower().Contains(model.search.value.ToLower()))
                        select new MarksEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            MarksID = u.marks_id,
                            MarksDate = u.marks_dt,
                            AchieveMarks = u.achive_marks,
                            MarksContentFileName = u.file_name,
                            MarksFilepath = u.file_path,
                            Count = count,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id },
                            testEntityInfo = new TestEntity()
                            {
                                TestID = u.test_id,
                                Marks = u.TEST_MASTER.total_marks,
                            },
                            student = new StudentEntity()
                            {
                                StudentID = u.student_id,
                                Name = u.STUDENT_MASTER.first_name + " " + u.STUDENT_MASTER.last_name
                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                }
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id.HasValue ? u.course_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                }
                            },
                            BranchSubject = new BranchSubjectEntity()
                            {
                                Subject_dtl_id = u.subject_dtl_id.HasValue ? u.subject_dtl_id.Value : 0,
                                Subject = new SuperAdminSubjectEntity()
                                {
                                    SubjectName = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name
                                }
                            }
                        })
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();           
            return data;
        }

        public async Task<MarksEntity> GetMarksByMarksID(long MarksID)
        {
            var data = (from u in this.context.MARKS_MASTER
                        where u.marks_id == MarksID
                        select new MarksEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            MarksID = u.marks_id,
                            MarksDate = u.marks_dt,
                            AchieveMarks = u.achive_marks,
                            MarksFilepath = u.file_path,
                            MarksContentFileName = u.file_name,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            BranchInfo= new BranchEntity() { BranchID = u.branch_id },
                            testEntityInfo = new TestEntity() { TestID = u.test_id }

                        }).FirstOrDefault();

            return data;
        }

        public ResponseModel RemoveMarks(long MarksID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var data = (from u in this.context.MARKS_MASTER
                            where u.marks_id == MarksID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                    responseModel.Message = "Marks Removed Successfully.";
                    responseModel.Status = true;
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            return responseModel;


            //return false;
        }

        public async Task<long> UpdateMarksDetails(MarksEntity marksEntity)
        {
            Model.MARKS_MASTER marks = new Model.MARKS_MASTER();
            bool isUpdate = true;
            var data = (from t in this.context.MARKS_MASTER
                        where t.marks_id == marksEntity.MarksID && t.student_id == marksEntity.student.StudentID
                        select t).ToList();


            foreach (var item in data)
            {

                item.achive_marks = marksEntity.AchieveMarks;
                this.context.MARKS_MASTER.Add(item);
                this.context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }


            return this.context.SaveChanges() > 0 ? marksEntity.MarksID : 0;
        }

        public async Task<List<MarksEntity>> GetAllStudentMarks(long BranchID,long StudentID)
        {
            var data = (from u in this.context.MARKS_MASTER
                        .Include("STUDENT_MASTER")
                        .Include("TEST_MASTER")
                        .Include("SUBJECT_MASTER")
                        orderby u.marks_id descending
                        where (u.branch_id == BranchID || BranchID == 0) && u.student_id == StudentID && u.row_sta_cd == 1
                        select new MarksEntity()
                        {
                            MarksID = u.marks_id,
                            MarksDate = u.marks_dt,                            
                            AchieveMarks = u.achive_marks,
                            MarksFilepath = "https://mastermind.org.in" + u.file_path,
                            MarksContentFileName = u.file_name,
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id },
                            testEntityInfo = new TestEntity()
                            {
                                TestID = u.test_id,
                                TestDate = u.TEST_MASTER.test_dt,
                                Marks = u.TEST_MASTER.total_marks
                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                }
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id.HasValue ? u.course_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                }
                            },
                            BranchSubject = new BranchSubjectEntity()
                            {
                                Subject_dtl_id = u.subject_dtl_id.HasValue ? u.subject_dtl_id.Value : 0,
                                Subject = new SuperAdminSubjectEntity()
                                {
                                    SubjectName = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name,
                                    SubjectID = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_id
                                }
                            }
                        }).ToList();

            return data;
        }

    }
}
