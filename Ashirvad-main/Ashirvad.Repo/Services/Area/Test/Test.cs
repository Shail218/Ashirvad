using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Test;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area.Test
{
    public class Test : ModelAccess, ITestAPI
    {

        public async Task<long> CheckTest(long BranchID, long StdID, long SubID, int BatchID, DateTime TestDate, long Testid,long CourseID)
        {
            long result;
            var date = DateTime.ParseExact(TestDate.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            bool isExists =(from u in this.context.TEST_MASTER
                            join t in this.context.TRANSACTION_MASTER on u.trans_id equals t.trans_id
                            where ((Testid == 0 || u.test_id != Testid) && u.branch_id == BranchID && u.class_dtl_id == StdID &&
             u.subject_dtl_id == SubID && u.batch_time_id == BatchID && u.test_dt == date && u.course_dtl_id == CourseID && u.row_sta_cd == 1)select u).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<ResponseModel> TestMaintenance(TestEntity testInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            Model.TEST_MASTER testMaster = new Model.TEST_MASTER();
            try
            {
                if (CheckTest(testInfo.Branch.BranchID, testInfo.BranchClass.Class_dtl_id, testInfo.BranchSubject.Subject_dtl_id, testInfo.BatchTimeID,
                testInfo.TestDate, testInfo.TestID, testInfo.BranchCourse.course_dtl_id).Result != -1)
                {
                    bool isUpdate = true;
                    var data = (from t in this.context.TEST_MASTER
                                where t.test_id == testInfo.TestID
                                select t).FirstOrDefault();
                    if (data == null)
                    {
                        data = new Model.TEST_MASTER();
                        isUpdate = false;
                    }
                    else
                    {
                        testMaster = data;
                        testInfo.Transaction.TransactionId = data.trans_id;
                    }

                    testMaster.row_sta_cd = testInfo.RowStatus.RowStatusId;
                    testMaster.trans_id = this.AddTransactionData(testInfo.Transaction);
                    testMaster.branch_id = testInfo.Branch.BranchID;
                    testMaster.class_dtl_id = testInfo.BranchClass.Class_dtl_id;
                    testMaster.course_dtl_id = testInfo.BranchCourse.course_dtl_id;
                    testMaster.subject_dtl_id = testInfo.BranchSubject.Subject_dtl_id;
                    testMaster.remarks = testInfo.Remarks;
                    testMaster.total_marks = testInfo.Marks;
                    testMaster.batch_time_id = testInfo.BatchTimeID;
                    testMaster.test_dt = testInfo.TestDate;
                    testMaster.test_st_time = testInfo.TestStartTime;
                    testMaster.test_end_time = testInfo.TestEndTime;
                    testMaster.test_name = "Demo";
                    this.context.TEST_MASTER.Add(testMaster);
                    if (isUpdate)
                    {
                        this.context.Entry(testMaster).State = System.Data.Entity.EntityState.Modified;
                    }
                    var da= this.context.SaveChanges() > 0 ? testMaster.test_id : 0;
                    if (da > 0)
                    {
                        testInfo.TestID = da;
                        responseModel.Data = testInfo;
                        responseModel.Message = isUpdate == true ? "Test Updated Successfully." : "Test Inserted Successfully.";
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = isUpdate == true ? "Test Not Updated." : "Test Not Inserted.";
                        responseModel.Status = false;
                    }
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "Test Already Exist.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }

            return responseModel;


        }

        public async Task<List<TestEntity>> GetAllTestByBranch(long branchID)
        {
            var data = (from u in this.context.TEST_MASTER
                        .Include("TEST_PAPER_REL")
                        .Include("BRANCH_MASTER")
                        .Include("CLASS_DTL_MASTER")
                        .Include("SUBJECT_DTL_MASTER")
                        join TestPaper in this.context.TEST_PAPER_REL on u.test_id equals TestPaper.test_id into tempBranch

                        orderby u.test_id descending
                        from branch in tempBranch.DefaultIfEmpty()
                        where u.branch_id == branchID && u.row_sta_cd == 1
                        select new TestEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassID = u.CLASS_DTL_MASTER.class_id,
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                                }
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseID = u.COURSE_DTL_MASTER.course_id,
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name,
                                }
                            },
                            BranchSubject = new BranchSubjectEntity()
                            {
                                Subject_dtl_id = u.subject_dtl_id.HasValue ? u.subject_dtl_id.Value : 0,
                                Subject = new SuperAdminSubjectEntity()
                                {
                                    SubjectID = u.SUBJECT_DTL_MASTER.subject_id,
                                    SubjectName = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name,
                                }
                            },
                           
                            BatchTimeID = u.batch_time_id,
                            BatchTimeText = u.batch_time_id == 1 ? "Morning" : u.batch_time_id == 2 ? "Afternoon" : u.batch_time_id == 3 ? "Evening" : u.batch_time_id == 4 ? "Morning2" : u.batch_time_id == 5 ? "Afternoon2" : u.batch_time_id == 6 ? "Evening2" : u.batch_time_id == 7 ? "Morning3" : u.batch_time_id == 8 ? "Afternoon3" : "Evening3",
                            TestID = u.test_id,
                            Remarks = u.remarks,
                            Marks = u.total_marks,
                            TestDate = u.test_dt,
                            TestEndTime = u.test_end_time,
                            TestName = u.test_name,
                            TestStartTime = u.test_st_time,
                            test = new TestPaperEntity()
                            {
                                DocContent = branch == null ? null : branch.doc_content,
                                TestPaperID = branch == null ? 0 : branch.test_paper_id,
                                PaperType = branch == null ? "" : branch.paper_type.ToString(),
                                DocLink = branch == null ? "" : branch.doc_link.ToString(),
                                FilePath = branch == null ? "" : "https://mastermind.org.in" + branch.file_path,
                                FileName = branch == null ? "" : branch.file_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public async Task<List<TestEntity>> GetAllCustomTest(DataTableAjaxPostModel model, long branchID)
        {
            var Result = new List<TestEntity>();
            bool Isasc = model.order[0].dir == "desc" ? false : true;
            long count = (from u in this.context.TEST_MASTER
                          join TestPaper in this.context.TEST_PAPER_REL on u.test_id equals TestPaper.test_id into tempBranch
                          orderby u.test_id descending
                          from branch in tempBranch.DefaultIfEmpty()
                          where u.branch_id == branchID && u.row_sta_cd == 1
                          select new TestEntity()
                          {
                              TestID = u.test_id
                          }).Distinct().Count();
            var data = (from u in this.context.TEST_MASTER
                        .Include("TEST_PAPER_REL")
                        .Include("BRANCH_MASTER")
                        .Include("CLASS_DTL_MASTER")
                        .Include("SUBJECT_DTL_MASTER")
                        join TestPaper in this.context.TEST_PAPER_REL on u.test_id equals TestPaper.test_id into tempBranch
     
                        from branch in tempBranch.DefaultIfEmpty()
                        where u.branch_id == branchID && u.row_sta_cd == 1
                        && (model.search.value == null
                        || model.search.value == ""
                        || u.CLASS_DTL_MASTER.CLASS_MASTER.class_name.ToLower().Contains(model.search.value)
                        || u.test_dt.ToString().ToLower().Contains(model.search.value)
                        || u.test_st_time.ToLower().Contains(model.search.value)
                        || u.test_end_time.ToLower().Contains(model.search.value)
                        || u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name.ToLower().Contains(model.search.value)
                        || u.total_marks.ToString().ToLower().Contains(model.search.value))
                        orderby u.test_id descending
                        select new TestEntity()
                        {
                            //RowStatus = new RowStatusEntity()
                            //{
                            //    RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                            //    RowStatusId = (int)u.row_sta_cd
                            //},
                            Branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassID = u.CLASS_DTL_MASTER.class_id,
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                                }
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseID = u.COURSE_DTL_MASTER.course_id,
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name,
                                }
                            },
                            BranchSubject = new BranchSubjectEntity()
                            {
                                Subject_dtl_id = u.subject_dtl_id.HasValue ? u.subject_dtl_id.Value : 0,
                                Subject = new SuperAdminSubjectEntity()
                                {
                                    SubjectID = u.SUBJECT_DTL_MASTER.subject_id,
                                    SubjectName = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name,
                                }
                            },
                            BatchTimeID = u.batch_time_id,
                            BatchTimeText = u.batch_time_id == 1 ? "Morning" : u.batch_time_id == 2 ? "Afternoon" : u.batch_time_id == 3 ? "Evening" : u.batch_time_id == 4 ? "Morning2" : u.batch_time_id == 5 ? "Afternoon2" : u.batch_time_id == 6 ? "Evening2" : u.batch_time_id == 7 ? "Morning3" : u.batch_time_id == 8 ? "Afternoon3" : "Evening3",
                            TestID = u.test_id,
                            Remarks = u.remarks,
                            Marks = u.total_marks,
                            TestDate = u.test_dt,
                            Count = count,
                            TestEndTime = u.test_end_time,
                            TestName = u.test_name,
                            TestStartTime = u.test_st_time,
                            test = new TestPaperEntity()
                            {
                                DocContent = branch == null ? null : branch.doc_content,
                                TestPaperID = branch == null ? 0 : branch.test_paper_id,
                                PaperType = branch == null ? "" : branch.paper_type.ToString(),
                                DocLink = branch == null ? "" : branch.doc_link.ToString(),
                                FilePath = branch == null ? "" : "https://mastermind.org.in" + branch.file_path,
                                FileName = branch == null ? "" : branch.file_name,
                                RowStatus = new RowStatusEntity()
                                {
                                    RowStatusId = branch == null ? 0 : branch.row_sta_cd
                                }
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        })
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();
            return data;
        }

        public async Task<List<TestEntity>> GetAllTestByBranchAPI(long branchID)
        {
            var data = (from u in this.context.TEST_MASTER
                        .Include("TEST_PAPER_REL")
                        .Include("BRANCH_MASTER")
                        .Include("CLASS_DTL_MASTER")
                        .Include("SUBJECT_DTL_MASTER")
                        orderby u.test_id descending
                        where u.branch_id == branchID && u.row_sta_cd == 1 
                        select new TestEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassID = u.CLASS_DTL_MASTER.class_id,
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                                }
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id.HasValue ? u.course_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseID = u.COURSE_DTL_MASTER.course_id,
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name,
                                }
                            },
                            BranchSubject = new BranchSubjectEntity()
                            {
                                Subject_dtl_id = u.subject_dtl_id.HasValue ? u.subject_dtl_id.Value : 0,
                                Subject = new SuperAdminSubjectEntity()
                                {
                                    SubjectID = u.SUBJECT_DTL_MASTER.subject_id,
                                    SubjectName = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name,
                                }
                            },
                            BatchTimeID = u.batch_time_id,
                            BatchTimeText = u.batch_time_id == 1 ? "Morning" : u.batch_time_id == 2 ? "Afternoon" : u.batch_time_id == 3 ? "Evening" : u.batch_time_id == 4 ? "Morning2" : u.batch_time_id == 5 ? "Afternoon2" : u.batch_time_id == 6 ? "Evening2" : u.batch_time_id == 7 ? "Morning3" : u.batch_time_id == 8 ? "Afternoon3" : "Evening3",
                            TestID = u.test_id,
                            Remarks = u.remarks,
                            Marks = u.total_marks,
                            TestDate = u.test_dt,
                            TestEndTime = u.test_end_time,
                            TestName = u.test_name,
                            TestStartTime = u.test_st_time,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            return data;
        }

        public async Task<List<TestEntity>> GetAllTestByBranchType(long branchID, long BatchType)
        {
            var data = (from u in this.context.TEST_MASTER
                        .Include("TEST_PAPER_REL")
                        .Include("BRANCH_MASTER")
                        .Include("CLASS_DTL_MASTER")
                        .Include("SUBJECT_DTL_MASTER")
                        join TestPaper in this.context.TEST_PAPER_REL on u.test_id equals TestPaper.test_id into tempBranch
                        orderby u.test_id descending
                        from branch in tempBranch.DefaultIfEmpty()
                        where u.branch_id == branchID && u.batch_time_id == BatchType
                        select new TestEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassID = u.CLASS_DTL_MASTER.class_id,
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                                }
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseID = u.COURSE_DTL_MASTER.course_id,
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name,
                                }
                            },
                            BranchSubject = new BranchSubjectEntity()
                            {
                                Subject_dtl_id = u.subject_dtl_id.HasValue ? u.subject_dtl_id.Value : 0,
                                Subject = new SuperAdminSubjectEntity()
                                {
                                    SubjectID = u.SUBJECT_DTL_MASTER.subject_id,
                                    SubjectName = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name,
                                }
                            },
                            BatchTimeID = u.batch_time_id,
                            BatchTimeText = u.batch_time_id == 1 ? "Morning" : u.batch_time_id == 2 ? "Afternoon" : u.batch_time_id == 3 ? "Evening" : u.batch_time_id == 4 ? "Morning2" : u.batch_time_id == 5 ? "Afternoon2" : u.batch_time_id == 6 ? "Evening2" : u.batch_time_id == 7 ? "Morning3" : u.batch_time_id == 8 ? "Afternoon3" : "Evening3",
                            TestID = u.test_id,
                            Remarks = u.remarks,
                            Marks = u.total_marks,
                            TestDate = u.test_dt,
                            TestEndTime = u.test_end_time,
                            TestName = u.test_name,
                            TestStartTime = u.test_st_time,
                            test = new TestPaperEntity()
                            {
                                DocContent = branch == null ? null : branch.doc_content,
                                TestPaperID = branch == null ? 0 : branch.test_paper_id,
                                PaperType = branch == null ? "" : branch.paper_type.ToString(),
                                DocLink = branch == null ? "" : branch.doc_link.ToString(),
                                FilePath = branch == null ? "" : "https://mastermind.org.in" + branch.file_path,
                                FileName = branch == null ? "" : branch.file_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }
        public async Task<List<TestEntity>> GetAllTestByBranchAndStandard(long branchID, long courseID,long stdID, int batchTime)
        {
            var data = (from u in this.context.TEST_MASTER
                        .Include("TEST_PAPER_REL")
                        .Include("BRANCH_MASTER")
                        .Include("CLASS_DTL_MASTER")
                        .Include("SUBJECT_DTL_MASTER")
                        orderby u.test_id descending
                        where u.branch_id == branchID && u.CLASS_DTL_MASTER.class_dtl_id == stdID 
                        && u.course_dtl_id == courseID && u.row_sta_cd == 1
                        && (batchTime == 0 || u.batch_time_id == batchTime)
                        select new TestEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassID = u.CLASS_DTL_MASTER.class_id,
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                                }
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseID = u.COURSE_DTL_MASTER.course_id,
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name,
                                }
                            },
                            BranchSubject = new BranchSubjectEntity()
                            {
                                Subject_dtl_id = u.subject_dtl_id.HasValue ? u.subject_dtl_id.Value : 0,
                                Subject = new SuperAdminSubjectEntity()
                                {
                                    SubjectID = u.SUBJECT_DTL_MASTER.subject_id,
                                    SubjectName = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name,
                                }
                            },
                            BatchTimeID = u.batch_time_id,
                            BatchTimeText = u.batch_time_id == 1 ? "Morning" : u.batch_time_id == 2 ? "Afternoon" : u.batch_time_id == 3 ? "Evening" : u.batch_time_id == 4 ? "Morning2" : u.batch_time_id == 5 ? "Afternoon2" : u.batch_time_id == 6 ? "Evening2" : u.batch_time_id == 7 ? "Morning3" : u.batch_time_id == 8 ? "Afternoon3" : "Evening3",
                            TestID = u.test_id,
                            Remarks = u.remarks,
                            Marks = u.total_marks,
                            TestDate = u.test_dt,
                            TestEndTime = u.test_end_time,
                            TestName = u.test_name,
                            TestStartTime = u.test_st_time,

                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).Distinct().OrderByDescending(a => a.TestID).ToList();

            return data;
        }

        public async Task<List<TestEntity>> TestDateDDL(long branchID, long stdID,long courseid,int batchTime)
        {
            var data = (from u in this.context.TEST_MASTER
                        orderby u.test_id descending
                        where u.branch_id == branchID && u.CLASS_DTL_MASTER.class_dtl_id == stdID
                        where u.branch_id == branchID && u.class_dtl_id == stdID && u.course_dtl_id == courseid
                        && (batchTime == 0 || u.batch_time_id == batchTime) && u.row_sta_cd==(int)Enums.RowStatus.Active
                        select new TestEntity()
                        {
                            TestDate = u.test_dt,
                        }).Distinct().ToList();

            return data;
        }

        public async Task<List<TestEntity>> GetAllTest(DateTime testDate, string searchParam)
        {
            DateTime fromDT = Convert.ToDateTime(testDate.ToShortTimeString() + " 00:00:00");
            DateTime toDT = Convert.ToDateTime(testDate.ToShortTimeString() + " 23:59:59");
            var data = (from u in this.context.TEST_MASTER
                        .Include("TEST_PAPER_REL")
                        .Include("BRANCH_MASTER")
                        .Include("CLASS_DTL_MASTER")
                        .Include("SUBJECT_DTL_MASTER")
                        where u.test_dt >= fromDT && u.test_dt <= toDT
                        && (string.IsNullOrEmpty(searchParam)
                        || u.remarks.Contains(searchParam)
                        || u.CLASS_DTL_MASTER.CLASS_MASTER.class_name.Contains(searchParam)
                        || u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name.Contains(searchParam)
                        || u.test_end_time.Contains(searchParam)
                        || u.test_name.Contains(searchParam)
                        || u.test_st_time.Contains(searchParam))
                        select new TestEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassID = u.CLASS_DTL_MASTER.class_id,
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                                }
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseID = u.COURSE_DTL_MASTER.course_id,
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name,
                                }
                            },
                            BranchSubject = new BranchSubjectEntity()
                            {
                                Subject_dtl_id = u.subject_dtl_id.HasValue ? u.subject_dtl_id.Value : 0,
                                Subject = new SuperAdminSubjectEntity()
                                {
                                    SubjectID = u.SUBJECT_DTL_MASTER.subject_id,
                                    SubjectName = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name,
                                }
                            },
                            BatchTimeID = u.batch_time_id,
                            BatchTimeText = u.batch_time_id == 1 ? "Morning" : u.batch_time_id == 2 ? "Afternoon" : u.batch_time_id == 3 ? "Evening" : u.batch_time_id == 4 ? "Morning2" : u.batch_time_id == 5 ? "Afternoon2" : u.batch_time_id == 6 ? "Evening2" : u.batch_time_id == 7 ? "Morning3" : u.batch_time_id == 8 ? "Afternoon3" : "Evening3",
                            TestID = u.test_id,
                            Remarks = u.remarks,
                            Marks = u.total_marks,
                            TestDate = u.test_dt,
                            TestEndTime = u.test_end_time,
                            TestName = u.test_name,
                            TestStartTime = u.test_st_time,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public async Task<TestEntity> GetTestByTestID(long testID)
        {
            var data = (from u in this.context.TEST_MASTER
                        .Include("TEST_PAPER_REL")
                        .Include("BRANCH_MASTER")
                        .Include("CLASS_DTL_MASTER")
                        .Include("SUBJECT_DTL_MASTER")
                        join TestPaper in this.context.TEST_PAPER_REL on u.test_id equals TestPaper.test_id into tempBranch
                        from branch in tempBranch.DefaultIfEmpty()
                        where u.test_id == testID 
                        select new TestEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassID = u.CLASS_DTL_MASTER.class_id,
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                                }
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id.HasValue ? u.course_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseID = u.COURSE_DTL_MASTER.course_id,
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name,
                                }
                            },
                            BranchSubject = new BranchSubjectEntity()
                            {
                                Subject_dtl_id = u.subject_dtl_id.HasValue ? u.subject_dtl_id.Value : 0,
                                Subject = new SuperAdminSubjectEntity()
                                {
                                    SubjectID = u.SUBJECT_DTL_MASTER.subject_id,
                                    SubjectName = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name,
                                }
                            },
                            test = new TestPaperEntity()
                            {
                                DocContent = branch == null ? null : branch.doc_content,
                                TestPaperID = branch == null ? 0 : branch.test_paper_id,
                                PaperType = branch == null ? "" : branch.paper_type.ToString(),
                                DocLink = branch == null ? "" : branch.doc_link.ToString(),
                                FilePath = branch == null ? "" : "https://mastermind.org.in" + branch.file_path,
                                FileName = branch == null ? "" : branch.file_name
                            },
                            BatchTimeID = u.batch_time_id,

                            BatchTimeText = u.batch_time_id == 1 ? "Morning" : u.batch_time_id == 2 ? "Afternoon" : u.batch_time_id == 3 ? "Evening" : u.batch_time_id == 4 ? "Morning2" : u.batch_time_id == 5 ? "Afternoon2" : u.batch_time_id == 6 ? "Evening2" : u.batch_time_id == 7 ? "Morning3" : u.batch_time_id == 8 ? "Afternoon3" : "Evening3",
                            TestID = u.test_id,
                            Remarks = u.remarks,
                            Marks = u.total_marks,
                            TestDate = u.test_dt,
                            TestEndTime = u.test_end_time,
                            TestName = u.test_name,
                            TestStartTime = u.test_st_time,

                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();
            return data;
        }

        public ResponseModel RemoveTest(long testID, string lastupdatedby, bool removePaper)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var data = (from u in this.context.TEST_MASTER
                            where u.test_id == testID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    if (removePaper)
                    {
                        var tPaper = (from paper in this.context.TEST_PAPER_REL
                                      where paper.test_id == testID
                                      select paper).ToList();
                        if (tPaper?.Count > 0)
                        {
                            foreach (var item in tPaper)
                            {
                                item.row_sta_cd = (int)Enums.RowStatus.Inactive;
                                item.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = item.trans_id, LastUpdateBy = lastupdatedby });
                            }
                        }
                    }

                    data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                    responseModel.Message = "Test Removed Successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.Message = "Test Not Found.";
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

        public async Task<ResponseModel> TestPaperMaintenance(TestPaperEntity paperInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            Model.TEST_PAPER_REL testRel = new Model.TEST_PAPER_REL();
            bool isUpdate = true;
            try
            {
                var data = (from t in this.context.TEST_PAPER_REL
                            where t.test_paper_id == paperInfo.TestPaperID
                            select t).FirstOrDefault();
                if (data == null)
                {
                    data = new Model.TEST_PAPER_REL();
                    isUpdate = false;
                }
                else
                {
                    testRel = data;
                    paperInfo.Transaction.TransactionId = data.trans_id;
                }

                testRel.row_sta_cd = paperInfo.RowStatus.RowStatusId;
                testRel.trans_id = this.AddTransactionData(paperInfo.Transaction);
                testRel.test_id = paperInfo.TestID;
                testRel.doc_content = null;
                testRel.file_name = paperInfo.FileName;
                testRel.file_path = paperInfo.FilePath;
                testRel.doc_link = paperInfo.DocLink;
                testRel.remakrs = paperInfo.Remarks;
                testRel.paper_type = paperInfo.PaperTypeID;

                this.context.TEST_PAPER_REL.Add(testRel);
                if (isUpdate)
                {
                    this.context.Entry(testRel).State = System.Data.Entity.EntityState.Modified;
                }

                var da = this.context.SaveChanges() > 0 ? testRel.test_id : 0;
                if (da > 0)
                {
                    paperInfo.TestPaperID = da;
                    responseModel.Data = paperInfo;
                    responseModel.Message = isUpdate == true ? "TestPaper Updated Successfully." : "TestPaper Inserted Successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.Message = isUpdate == true ? "TestPaper Not Updated." : "TestPaper Not Inserted.";
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

        public async Task<List<TestPaperEntity>> GetAllTestPapaerByTest(long testID)
        {
            var data = (from u in this.context.TEST_PAPER_REL
                        .Include("TEST_MASTER")
                        orderby u.test_paper_id descending
                        where u.test_id == testID
                        select new TestPaperEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            DocContent = u.doc_content,
                            DocLink = u.doc_link,
                            PaperType = u.paper_type == 1 ? "" : "",
                            PaperTypeID = u.paper_type,
                            TestID = u.test_id,
                            FileName = u.file_name,
                            FilePath = u.file_path,
                            Remarks = u.remakrs,
                            TestDate = u.TEST_MASTER.test_dt,
                            TestName = u.TEST_MASTER.test_name,
                            TestPaperID = u.test_paper_id,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            if (data?.Count > 0)
            {
                if (data[0].DocContent != null)
                {
                    foreach (var item in data)
                    {
                        int idx = data.IndexOf(item);
                        data[idx].DocContentText = Convert.ToBase64String(data[idx].DocContent);
                    }
                }
            }
            return data;
        }

        public async Task<List<TestPaperEntity>> GetAllTestPapaerByBranchStdDate(long branchID,long courseid, long stdID, DateTime dt, int batchTime)
        {
            var data = (from u in this.context.TEST_PAPER_REL
                        .Include("TEST_MASTER")
                        orderby u.test_paper_id descending
                        where u.TEST_MASTER.branch_id == branchID
                        && u.TEST_MASTER.class_dtl_id == stdID
                        && u.TEST_MASTER.test_dt == dt
                        && u.TEST_MASTER.course_dtl_id == courseid
                        && (0 == batchTime || u.TEST_MASTER.batch_time_id == batchTime)
                        select new TestPaperEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            testinfo = new TestEntity()
                            {
                                BranchClass = new BranchClassEntity()
                                {
                                    Class_dtl_id = u.TEST_MASTER.class_dtl_id.HasValue ? u.TEST_MASTER.class_dtl_id.Value : 0,
                                    Class = new ClassEntity()
                                    {
                                        ClassID = u.TEST_MASTER.CLASS_DTL_MASTER.class_id,
                                        ClassName = u.TEST_MASTER.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                                    }
                                },
                                BranchCourse = new BranchCourseEntity()
                                {
                                    course_dtl_id = u.TEST_MASTER.class_dtl_id.HasValue ? u.TEST_MASTER.class_dtl_id.Value : 0,
                                    course = new CourseEntity()
                                    {
                                        CourseID = u.TEST_MASTER.COURSE_DTL_MASTER.course_id,
                                        CourseName = u.TEST_MASTER.COURSE_DTL_MASTER.COURSE_MASTER.course_name,
                                    }
                                },
                                BranchSubject = new BranchSubjectEntity()
                                {
                                    Subject_dtl_id = u.TEST_MASTER.subject_dtl_id.HasValue ? u.TEST_MASTER.subject_dtl_id.Value : 0,
                                    Subject = new SuperAdminSubjectEntity()
                                    {
                                        SubjectID = u.TEST_MASTER.SUBJECT_DTL_MASTER.subject_id,
                                        SubjectName = u.TEST_MASTER.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name,
                                    }
                                },
                                
                                BatchTimeID = u.TEST_MASTER.batch_time_id,
                                Marks = u.TEST_MASTER.total_marks,
                                TestStartTime = u.TEST_MASTER.test_st_time,
                                TestEndTime = u.TEST_MASTER.test_end_time
                            },
                            DocContent = u.doc_content,
                            DocLink = u.doc_link,
                            PaperType = u.paper_type == 1 ? "" : "",
                            PaperTypeID = u.paper_type,
                            TestID = u.test_id,
                            FileName = u.file_name,
                            Remarks = u.remakrs,
                            TestDate = u.TEST_MASTER.test_dt,
                            TestName = u.TEST_MASTER.test_name,
                            TestPaperID = u.test_paper_id,
                            FilePath = "https://mastermind.org.in" + u.file_path,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            //if (data?.Count > 0)
            //{
            //    foreach (var item in data)
            //    {
            //        int idx = data.IndexOf(item);
            //        data[idx].DocContentText = Convert.ToBase64String(data[idx].DocContent);
            //    }
            //}
            return data;
        }

        public async Task<List<TestPaperEntity>> GetAllTestPapaerWithoutContentByTest(long testID)
        {
            var data = (from u in this.context.TEST_PAPER_REL
                        .Include("TEST_MASTER")
                        orderby u.test_paper_id descending
                        where u.test_id == testID
                        select new TestPaperEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            DocLink = u.doc_link,
                            PaperTypeID = u.paper_type,
                            TestID = u.test_id,
                            Remarks = u.remakrs,
                            FileName = u.file_name,
                            TestDate = u.TEST_MASTER.test_dt,
                            TestPaperID = u.test_paper_id,
                            FilePath = "https://mastermind.org.in" + u.file_path,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public async Task<TestPaperEntity> GetTestPaperByPaperID(long paperID)
        {
            var data = (from u in this.context.TEST_PAPER_REL
                        .Include("TEST_MASTER")
                        where u.test_paper_id == paperID
                        select new TestPaperEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            DocContent = u.doc_content,
                            DocLink = u.doc_link,
                            PaperType = u.paper_type == 1 ? "" : "",
                            PaperTypeID = u.paper_type,
                            TestID = u.test_id,
                            Remarks = u.remakrs,
                            FileName = u.file_name,
                            TestDate = u.TEST_MASTER.test_dt,
                            TestName = u.TEST_MASTER.test_name,
                            TestPaperID = u.test_paper_id,
                            FilePath = u.file_path,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();


            return data;
        }
        public async Task<List<TestEntity>> GetTestPaperChecking(long paperID)
        {
            var data = (from u in this.context.TEST_MASTER_DTL
                        .Include("TEST_MASTER")
                        orderby u.Test_master_dtl_id descending
                        where u.Test_id == paperID 
                        select new TestEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.TEST_MASTER.class_dtl_id.HasValue ? u.TEST_MASTER.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassID = u.TEST_MASTER.CLASS_DTL_MASTER.class_id,
                                    ClassName = u.TEST_MASTER.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                                }
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.TEST_MASTER.class_dtl_id.HasValue ? u.TEST_MASTER.class_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseID = u.TEST_MASTER.COURSE_DTL_MASTER.course_id,
                                    CourseName = u.TEST_MASTER.COURSE_DTL_MASTER.COURSE_MASTER.course_name,
                                }
                            },
                            BranchSubject = new BranchSubjectEntity()
                            {
                                Subject_dtl_id = u.TEST_MASTER.subject_dtl_id.HasValue ? u.TEST_MASTER.subject_dtl_id.Value : 0,
                                Subject = new SuperAdminSubjectEntity()
                                {
                                    SubjectID = u.TEST_MASTER.SUBJECT_DTL_MASTER.subject_id,
                                    SubjectName = u.TEST_MASTER.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name,
                                }
                            },
                            
                            Marks = u.TEST_MASTER.total_marks,
                            TestStartTime = u.TEST_MASTER.test_st_time,
                            TestEndTime = u.TEST_MASTER.test_end_time,
                            Remarks = u.remarks,
                            TestDate = u.TEST_MASTER.test_dt,
                            TestName = u.TEST_MASTER.test_name,

                            testdtl = new TestDetailEntity()
                            {
                                TestDetailID = u.Test_master_dtl_id,
                                BranchInfo = new BranchEntity()
                                {
                                    BranchID = u.branch_id,
                                    BranchName = u.TEST_MASTER.BRANCH_MASTER.branch_name,
                                },
                                StudentInfo = new StudentEntity()
                                {
                                    FirstName = u.STUDENT_MASTER.first_name,
                                    MiddleName = u.STUDENT_MASTER.first_name,
                                    LastName = u.STUDENT_MASTER.first_name
                                },
                                AnswerSheetName = u.Test_sheet_name,
                                FilePath = u.Test_filepath,
                                Status = u.status,
                                Remarks = u.remarks,
                                SubmitDate = u.submit_dt
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();


            return data;
        }

        public ResponseModel RemoveTestPaper(long paperID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var data = (from u in this.context.TEST_PAPER_REL
                            where u.test_paper_id == paperID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                    responseModel.Message = "TestPaper Removed Successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.Message = "TestPaper Not Found.";
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

        #region - Test Answer Sheet

        public async Task<ResponseModel> AnswerSheetMaintenance(StudentAnswerSheetEntity studAnswerSheet)
        {
            ResponseModel responseModel = new ResponseModel();
            Model.STUDENT_ANS_SHEET ansSheet = new Model.STUDENT_ANS_SHEET();
            bool isUpdate = true;
            try
            {
                var data = (from t in this.context.STUDENT_ANS_SHEET
                            where t.test_id == studAnswerSheet.TestInfo.TestID && t.stud_id == studAnswerSheet.StudentInfo.StudentID
                            select t).FirstOrDefault();
                if (data == null)
                {
                    data = new Model.STUDENT_ANS_SHEET();
                    isUpdate = false;
                }
                else
                {
                    ansSheet = new Model.STUDENT_ANS_SHEET();
                    isUpdate = false;

                }

                ansSheet.row_sta_cd = studAnswerSheet.RowStatus.RowStatusId;
                ansSheet.trans_id = this.AddTransactionData(studAnswerSheet.Transaction);
                ansSheet.test_id = studAnswerSheet.TestInfo.TestID;
                ansSheet.ans_sheet_content = null;
                ansSheet.ans_sheet_name = studAnswerSheet.AnswerSheetName;
                ansSheet.branch_id = studAnswerSheet.BranchInfo.BranchID;
                ansSheet.remarks = studAnswerSheet.Remarks;
                ansSheet.status = studAnswerSheet.Status;
                ansSheet.stud_id = studAnswerSheet.StudentInfo.StudentID;
                ansSheet.submit_dt = studAnswerSheet.SubmitDate;
                ansSheet.ans_sheet_filepath = studAnswerSheet.FilePath;
                this.context.STUDENT_ANS_SHEET.Add(ansSheet);
                if (isUpdate)
                {
                    this.context.Entry(ansSheet).State = System.Data.Entity.EntityState.Modified;
                }

                var da = this.context.SaveChanges() > 0 ? ansSheet.test_id : 0;
                if (da > 0)
                {
                    studAnswerSheet.AnsSheetID = da;
                    responseModel.Data = studAnswerSheet;
                    responseModel.Message = isUpdate == true ? "AnswerSheet Updated Successfully." : "AnswerSheet Inserted Successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.Message = isUpdate == true ? "AnswerSheet Not Updated." : "AnswerSheet Not Inserted.";
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

        public async Task<List<StudentAnswerSheetEntity>> GetAllTestAnswerSheetByTestStudent(long testID)
        {
            var data = (from u in this.context.STUDENT_ANS_SHEET
                        .Include("TEST_MASTER")
                        .Include("STUDENT_MASTER")
                        .Include("BRANCH_MASTER")
                        where u.test_id == testID
                        select new StudentAnswerSheetEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            AnswerSheetContent = u.ans_sheet_content,
                            AnsSheetID = u.ans_sheet_id,
                            AnswerSheetName = u.ans_sheet_name,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Remarks = u.remarks,
                            Status = u.status,
                            StatusText = u.status == 1 ? "Pending" : "Done",
                            StudentInfo = new StudentEntity()
                            {
                                StudentID = u.STUDENT_MASTER.student_id,
                                FirstName = u.STUDENT_MASTER.first_name,
                                LastName = u.STUDENT_MASTER.last_name
                            },
                            SubmitDate = u.submit_dt,
                            TestInfo = new TestEntity()
                            {
                                TestID = u.test_id,
                                TestDate = u.TEST_MASTER.test_dt,
                                TestName = u.TEST_MASTER.test_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    int idx = data.IndexOf(item);
                    data[idx].AnswerSheetContentText = Convert.ToBase64String(data[idx].AnswerSheetContent);
                }
            }
            return data;
        }

        public async Task<List<StudentAnswerSheetEntity>> GetallAnswerSheetData(long testID)
        {
            var data = (from u in this.context.STUDENT_ANS_SHEET
                        .Include("TEST_MASTER")
                        .Include("STUDENT_MASTER")
                        .Include("BRANCH_MASTER")
                        .Include("CLASS_DTL_MASTER")
                        where u.test_id == testID
                        select new StudentAnswerSheetEntity()
                        {


                            Remarks = u.remarks,
                            Status = u.status,
                            StatusText = u.status == 1 ? "Pending" : "Done",
                            StudentInfo = new StudentEntity()
                            {
                                StudentID = u.STUDENT_MASTER.student_id,
                                FirstName = u.STUDENT_MASTER.first_name,
                                LastName = u.STUDENT_MASTER.last_name,
                                Name = u.STUDENT_MASTER.first_name + " " + u.STUDENT_MASTER.last_name
                            },

                            SubmitDate = u.submit_dt,
                            TestInfo = new TestEntity()
                            {
                                TestID = u.test_id,
                                TestDate = u.TEST_MASTER.test_dt,
                                TestName = u.TEST_MASTER.test_name,
                                Standard = new StandardEntity()
                                {
                                    Standard = u.TEST_MASTER.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                                    StandardID = u.TEST_MASTER.CLASS_DTL_MASTER.class_dtl_id,

                                },
                            },

                        }).Distinct().ToList();
            //if (data?.Count > 0)
            //{
            //    foreach (var item in data)
            //    {
            //        int idx = data.IndexOf(item);
            //        data[idx].AnswerSheetContentText = Convert.ToBase64String(data[idx].AnswerSheetContent);
            //    }
            //}
            return data;
        }

        public async Task<List<StudentAnswerSheetEntity>> GetAllAnsSheetByTestStudentID(long testID, long studentID)
        {
            var data = (from u in this.context.STUDENT_ANS_SHEET
                       .Include("TEST_MASTER")
                       .Include("STUDENT_MASTER")
                       .Include("BRANCH_MASTER")
                        where u.test_id == testID && u.stud_id == studentID
                        select new StudentAnswerSheetEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            AnswerSheetContent = u.ans_sheet_content,
                            AnsSheetID = u.ans_sheet_id,
                            AnswerSheetName = u.ans_sheet_name,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Remarks = u.remarks,
                            Status = u.status,
                            StatusText = u.status == 1 ? "Pending" : "Done",
                            StudentInfo = new StudentEntity()
                            {
                                StudentID = u.STUDENT_MASTER.student_id,
                                FirstName = u.STUDENT_MASTER.first_name,
                                LastName = u.STUDENT_MASTER.last_name
                            },
                            SubmitDate = u.submit_dt,
                            TestInfo = new TestEntity()
                            {
                                TestID = u.test_id,
                                TestDate = u.TEST_MASTER.test_dt,
                                TestName = u.TEST_MASTER.test_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    int idx = data.IndexOf(item);
                    data[idx].AnswerSheetContentText = Convert.ToBase64String(data[idx].AnswerSheetContent);
                }
            }
            return data;
        }

        public async Task<List<StudentAnswerSheetEntity>> GetAllTestAnswerSheetWithoutContentByTestStudent(long testID)
        {
            var data = (from u in this.context.STUDENT_ANS_SHEET
                        .Include("TEST_MASTER")
                        .Include("STUDENT_MASTER")
                        .Include("BRANCH_MASTER")
                        where u.test_id == testID 
                        select new StudentAnswerSheetEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            AnsSheetID = u.ans_sheet_id,
                            AnswerSheetName = u.ans_sheet_name,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Remarks = u.remarks,
                            Status = u.status,
                            StatusText = u.status == 1 ? "Pending" : "Done",
                            StudentInfo = new StudentEntity()
                            {
                                StudentID = u.STUDENT_MASTER.student_id,
                                FirstName = u.STUDENT_MASTER.first_name,
                                LastName = u.STUDENT_MASTER.last_name
                            },
                            SubmitDate = u.submit_dt,
                            TestInfo = new TestEntity()
                            {
                                TestID = u.test_id,
                                TestDate = u.TEST_MASTER.test_dt,
                                TestName = u.TEST_MASTER.test_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            return data;
        }

        public async Task<StudentAnswerSheetEntity> GetTestAnswerSheetPaperByAnswerSheetID(long ansID)
        {
            var data = (from u in this.context.STUDENT_ANS_SHEET
                        .Include("TEST_MASTER")
                        .Include("STUDENT_MASTER")
                        .Include("BRANCH_MASTER")
                        where u.ans_sheet_id == ansID 
                        select new StudentAnswerSheetEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            AnswerSheetContent = u.ans_sheet_content,
                            AnsSheetID = u.ans_sheet_id,
                            AnswerSheetName = u.ans_sheet_name,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Remarks = u.remarks,
                            Status = u.status,
                            StatusText = u.status == 1 ? "Pending" : "Done",
                            StudentInfo = new StudentEntity()
                            {
                                StudentID = u.STUDENT_MASTER.student_id,
                                FirstName = u.STUDENT_MASTER.first_name,
                                LastName = u.STUDENT_MASTER.last_name
                            },
                            SubmitDate = u.submit_dt,
                            TestInfo = new TestEntity()
                            {
                                TestID = u.test_id,
                                TestDate = u.TEST_MASTER.test_dt,
                                TestName = u.TEST_MASTER.test_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();
            if (data != null)
            {

                data.AnswerSheetContentText = Convert.ToBase64String(data.AnswerSheetContent);
            }

            return data;
        }

        public ResponseModel RemoveAnswerSheet(long ansID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var data = (from u in this.context.STUDENT_ANS_SHEET
                            where u.ans_sheet_id == ansID
                            select u).FirstOrDefault();

                if (data != null)
                {
                    data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                    responseModel.Message = "AnswerSheet Removed Successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.Message = "AnswerSheet Not Found.";
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

        #endregion

        public async Task<ResponseModel> TestMaintenance(TestDetailEntity TestDetail)
        {
            ResponseModel responseModel = new ResponseModel();
            //Model.TEST_MASTER_DTL Test = new Model.TEST_MASTER_DTL();
            //bool isUpdate = true;
            //var data = (from t in this.context.TEST_MASTER_DTL
            //            where t.Test_master_dtl_id == TestDetail.TestDetailID
            //            select t).FirstOrDefault();
            //if (data == null)
            //{
            //    data = new Model.TEST_MASTER_DTL();
            //    isUpdate = false;
            //}
            //else
            //{
            //    Test = data;
            //    TestDetail.Transaction.TransactionId = data.trans_id;
            //}

            //Test.row_sta_cd = TestDetail.RowStatus.RowStatusId;
            //Test.trans_id = this.AddTransactionData(TestDetail.Transaction);
            //Test.Test_id = TestDetail.TestEntity.TestID;
            //if (Test.Test_sheet_content?.Length > 0)
            //{
            //    Test.Test_sheet_content = TestDetail.AnswerSheetContent;

            //}
            //Test.Test_sheet_name = TestDetail.AnswerSheetName;
            //Test.Test_filepath = TestDetail.FilePath;
            //Test.branch_id = TestDetail.BranchInfo.BranchID;
            //Test.remarks = TestDetail.Remarks;
            //Test.status = TestDetail.Status;
            //Test.stud_id = TestDetail.StudentInfo.StudentID;
            //Test.submit_dt = TestDetail.SubmitDate;
            //this.context.TEST_MASTER_DTL.Add(Test);
            //if (isUpdate)
            //{
            //    this.context.Entry(Test).State = System.Data.Entity.EntityState.Modified;
            //}

            return responseModel;
        }

        public async Task<List<TestPaperEntity>> GetAllTestDocLinks(long branchID,long courseid, long stdID, int batchTime)
        {
            var data = (from u in this.context.TEST_PAPER_REL
                        .Include("TEST_MASTER")
                        .Include("BRANCH_MASTER")
                        .Include("CLASS_DTL_MASTER")
                        orderby u.test_paper_id descending
                        where u.TEST_MASTER.branch_id == branchID && u.TEST_MASTER.class_dtl_id == stdID
                        && u.TEST_MASTER.course_dtl_id == courseid
                        && (batchTime == 0 || u.TEST_MASTER.batch_time_id == batchTime) && !u.doc_link.Equals(" ") && u.row_sta_cd == 1
                        select new TestPaperEntity()
                        {
                            TestID = u.test_id,
                            Remarks = u.remakrs,
                            DocLink = u.doc_link
                        }).ToList();

            return data;
        }


        public ResponseModel RemoveTestAnswerSheetdetail(long TestID, long studid)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var data = (from u in this.context.STUDENT_ANS_SHEET
                            where u.test_id == TestID && u.stud_id == studid
                            select u).ToList();

                if (data != null)
                {
                    this.context.STUDENT_ANS_SHEET.RemoveRange(data);
                    this.context.SaveChanges();
                    responseModel.Message = "TestAnswerSheetDetails Removed Successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.Message = "TestAnswerSheetDetails Not Found.";
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

        public async Task<TestEntity> GetTestDetails(long TestID, long SubjectID)
        {
            var data = (from u in this.context.TEST_MASTER
                        where u.test_id == TestID && u.subject_dtl_id == SubjectID
                        select new TestEntity()
                        {
                            TestID = u.test_id,
                            Remarks = u.remarks,
                            Marks = u.total_marks,
                            TestDate = u.test_dt,
                            TestEndTime = u.test_end_time,
                            TestName = u.test_name,
                            TestStartTime = u.test_st_time,
                            Branch = new BranchEntity()
                            {
                                BranchID = u.branch_id
                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassID = u.CLASS_DTL_MASTER.class_id,
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                                }
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.class_dtl_id.HasValue ? u.class_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseID = u.COURSE_DTL_MASTER.course_id,
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name,
                                }
                            },
                            BranchSubject = new BranchSubjectEntity()
                            {
                                Subject_dtl_id = u.subject_dtl_id.HasValue ? u.subject_dtl_id.Value : 0,
                                Subject = new SuperAdminSubjectEntity()
                                {
                                    SubjectID = u.SUBJECT_DTL_MASTER.subject_id,
                                    SubjectName = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name,
                                }
                            },
                            BatchTimeID = u.batch_time_id,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();
            if (data != null)
            {
                data.marksentered = CheckMarks(data.TestID, data.Branch.BranchID, data.BranchSubject.Subject_dtl_id, data.BatchTimeID);
            }
            return data;
        }

        public bool CheckMarks(long TestID, long BranchID, long SubjectId, int BatchId)
        {
            bool isExists =(from u in this.context.MARKS_MASTER
                            
                            where (u.test_id == TestID && u.branch_id == BranchID && u.subject_dtl_id == SubjectId && u.batch_time_id == BatchId && u.row_sta_cd == 1) select u).FirstOrDefault() != null;

            return isExists;
        }

        public async Task<List<StudentAnswerSheetEntity>> GetStudentAnsFile(long TestID)
        {

            var data = (from u in this.context.STUDENT_ANS_SHEET
                        .Include("TEST_MASTER")
                        .Include("CLASS_DTL_MASTER")
                        orderby u.ans_sheet_id descending
                        where u.test_id == TestID
                        select new StudentAnswerSheetEntity()
                        {
                            FilePath = u.ans_sheet_filepath,
                            AnswerSheetName = u.ans_sheet_name,
                        }).ToList();
            //if (data != null)
            //{
            //    data.HomeworkContentText = Convert.ToBase64String(data.HomeworkContent);
            //}
            return data;
        }

        public async Task<ResponseModel> AnsDetailUpdate(StudentAnswerSheetEntity homeworkDetail)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                Model.STUDENT_ANS_SHEET homework = new Model.STUDENT_ANS_SHEET();
                bool isUpdate = true;
                var data = (from t in this.context.STUDENT_ANS_SHEET
                            where t.test_id == homeworkDetail.TestInfo.TestID && t.stud_id == homeworkDetail.StudentInfo.StudentID
                            select t).ToList();


                foreach (var item in data)
                {

                    item.remarks = homeworkDetail.Remarks;
                    item.status = homeworkDetail.Status;
                    this.context.STUDENT_ANS_SHEET.Add(item);
                    this.context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }


                var da = this.context.SaveChanges() > 0 ? homeworkDetail.TestInfo.TestID : 0;
                if (da > 0)
                {
                    responseModel.Data = homeworkDetail;
                    responseModel.Message = "AnswerSheet Updated Successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.Message = "AnswerSheet Not Updated.";
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

    }
}
