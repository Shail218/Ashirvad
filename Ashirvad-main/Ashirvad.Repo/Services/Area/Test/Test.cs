using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Test
{
    public class Test : ModelAccess, ITestAPI
    {
        public async Task<long> TestMaintenance(TestEntity testInfo)
        {
            Model.TEST_MASTER testMaster = new Model.TEST_MASTER();
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
            testMaster.std_id = testInfo.Standard.StandardID;
            testMaster.sub_id = testInfo.Subject.SubjectID;
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

            return this.context.SaveChanges() > 0 ? testMaster.test_id : 0;
        }

        public async Task<List<TestEntity>> GetAllTestByBranch(long branchID)
        {
            var data = (from u in this.context.TEST_MASTER
                        .Include("TEST_PAPER_REL")
                        .Include("BRANCH_MASTER")
                        .Include("STD_MASTER")
                        .Include("SUBJECT_MASTER")
                        join TestPaper in this.context.TEST_PAPER_REL on u.test_id equals TestPaper.test_id
                        where u.branch_id == branchID && u.row_sta_cd==1
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
                            Standard = new StandardEntity()
                            {
                                StandardID = u.std_id,
                                Standard = u.STD_MASTER.standard
                            },
                            Subject = new SubjectEntity()
                            {
                                SubjectID = u.SUBJECT_MASTER.subject_id,
                                Subject = u.SUBJECT_MASTER.subject
                            },
                            BatchTimeID = u.batch_time_id,
                            BatchTimeText = u.batch_time_id == 1 ? "Morning" : u.batch_time_id == 2 ? "Afternoon" : "Evening",
                            TestID = u.test_id,
                            Remarks = u.remarks,
                            Marks = u.total_marks,
                            TestDate = u.test_dt,
                            TestEndTime = u.test_end_time,
                            TestName = u.test_name,
                            TestStartTime = u.test_st_time,
                            test = new TestPaperEntity()
                            {
                                DocContent = TestPaper.doc_content,
                                TestPaperID = TestPaper.test_paper_id,
                                PaperType = TestPaper.paper_type.ToString(),
                                DocLink = TestPaper.doc_link.ToString(),
                                FilePath=TestPaper.file_path
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }


        public async Task<List<TestEntity>> GetAllTestByBranchType(long branchID,long BatchType)
        {
            var data = (from u in this.context.TEST_MASTER
                        .Include("TEST_PAPER_REL")
                        .Include("BRANCH_MASTER")
                        .Include("STD_MASTER")
                        .Include("SUBJECT_MASTER")
                        join TestPaper in this.context.TEST_PAPER_REL on u.test_id equals TestPaper.test_id
                        where u.branch_id == branchID && u.batch_time_id==BatchType
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
                            Standard = new StandardEntity()
                            {
                                StandardID = u.std_id,
                                Standard = u.STD_MASTER.standard
                            },
                            Subject = new SubjectEntity()
                            {
                                SubjectID = u.SUBJECT_MASTER.subject_id,
                                Subject = u.SUBJECT_MASTER.subject
                            },
                            BatchTimeID = u.batch_time_id,
                            BatchTimeText = u.batch_time_id == 1 ? "Morning" : u.batch_time_id == 2 ? "Afternoon" : "Evening",
                            TestID = u.test_id,
                            Remarks = u.remarks,
                            Marks = u.total_marks,
                            TestDate = u.test_dt,
                            TestEndTime = u.test_end_time,
                            TestName = u.test_name,
                            TestStartTime = u.test_st_time,
                            test = new TestPaperEntity()
                            {
                                DocContent = TestPaper.doc_content,
                                TestPaperID = TestPaper.test_paper_id,
                                PaperType = TestPaper.paper_type.ToString(),
                                DocLink = TestPaper.doc_link.ToString()
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }
        public async Task<List<TestEntity>> GetAllTestByBranchAndStandard(long branchID, long stdID, int batchTime)
        {
            var data = (from u in this.context.TEST_MASTER
                        .Include("TEST_PAPER_REL")
                        .Include("BRANCH_MASTER")
                        .Include("STD_MASTER")
                        .Include("SUBJECT_MASTER")
                        where u.branch_id == branchID && u.STD_MASTER.std_id == stdID
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
                            Standard = new StandardEntity()
                            {
                                StandardID = u.std_id,
                                Standard = u.STD_MASTER.standard
                            },
                            Subject = new SubjectEntity()
                            {
                                SubjectID = u.SUBJECT_MASTER.subject_id,
                                Subject = u.SUBJECT_MASTER.subject
                            },
                            BatchTimeID = u.batch_time_id,
                            BatchTimeText = u.batch_time_id == 1 ? "Morning" : u.batch_time_id == 2 ? "Afternoon" : "Evening",
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

        public async Task<List<TestEntity>> GetAllTest(DateTime testDate, string searchParam)
        {
            DateTime fromDT = Convert.ToDateTime(testDate.ToShortTimeString() + " 00:00:00");
            DateTime toDT = Convert.ToDateTime(testDate.ToShortTimeString() + " 23:59:59");
            var data = (from u in this.context.TEST_MASTER
                        .Include("TEST_PAPER_REL")
                        .Include("BRANCH_MASTER")
                        .Include("STD_MASTER")
                        .Include("SUBJECT_MASTER")
                        where u.test_dt >= fromDT && u.test_dt <= toDT
                        && (string.IsNullOrEmpty(searchParam)
                        || u.remarks.Contains(searchParam)
                        || u.STD_MASTER.standard.Contains(searchParam)
                        || u.SUBJECT_MASTER.subject.Contains(searchParam)
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
                            Standard = new StandardEntity()
                            {
                                StandardID = u.std_id,
                                Standard = u.STD_MASTER.standard
                            },
                            Subject = new SubjectEntity()
                            {
                                SubjectID = u.SUBJECT_MASTER.subject_id,
                                Subject = u.SUBJECT_MASTER.subject
                            },
                            BatchTimeID = u.batch_time_id,
                            BatchTimeText = u.batch_time_id == 1 ? "Morning" : u.batch_time_id == 2 ? "Afternoon" : "Evening",
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
                        .Include("STD_MASTER")
                        .Include("SUBJECT_MASTER")
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
                            Standard = new StandardEntity()
                            {
                                StandardID = u.std_id,
                                Standard = u.STD_MASTER.standard
                            },
                            Subject = new SubjectEntity()
                            {
                                SubjectID = u.SUBJECT_MASTER.subject_id,
                                Subject = u.SUBJECT_MASTER.subject
                            },
                            BatchTimeID = u.batch_time_id,

                            BatchTimeText = u.batch_time_id == 1 ? "Morning" : u.batch_time_id == 2 ? "Afternoon" : "Evening",
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

        public bool RemoveTest(long testID, string lastupdatedby, bool removePaper)
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
                return true;
            }

            return false;
        }

        public async Task<long> TestPaperMaintenance(TestPaperEntity paperInfo)
        {
            Model.TEST_PAPER_REL testRel = new Model.TEST_PAPER_REL();
            bool isUpdate = true;
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
            testRel.doc_content =null;
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

            return this.context.SaveChanges() > 0 ? testRel.test_id : 0;
        }

        public async Task<List<TestPaperEntity>> GetAllTestPapaerByTest(long testID)
        {
            var data = (from u in this.context.TEST_PAPER_REL
                        .Include("TEST_MASTER")
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

        public async Task<List<TestPaperEntity>> GetAllTestPapaerByBranchStdDate(long branchID, long stdID, DateTime dt, int batchTime)
        {
            var data = (from u in this.context.TEST_PAPER_REL
                        .Include("TEST_MASTER")
                        where u.TEST_MASTER.branch_id == branchID
                        && u.TEST_MASTER.std_id == stdID
                        && u.TEST_MASTER.test_dt == dt
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
                                Subject = new SubjectEntity()
                                {
                                    Subject = u.TEST_MASTER.SUBJECT_MASTER.subject
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
                            FilePath = u.file_path,
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
                        where u.test_id == testID
                        select new TestPaperEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            DocLink = u.doc_link,
                            PaperType = u.paper_type == 1 ? "" : "",
                            PaperTypeID = u.paper_type,
                            TestID = u.test_id,
                            Remarks = u.remakrs,
                            FileName = u.file_name,
                            TestDate = u.TEST_MASTER.test_dt,
                            TestName = u.TEST_MASTER.test_name,
                            TestPaperID = u.test_paper_id,
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


        public bool RemoveTestPaper(long paperID, string lastupdatedby)
        {
            var data = (from u in this.context.TEST_PAPER_REL
                        where u.test_paper_id == paperID
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

        #region - Test Answer Sheet

        public async Task<long> AnswerSheetMaintenance(StudentAnswerSheetEntity studAnswerSheet)
        {
            Model.STUDENT_ANS_SHEET ansSheet = new Model.STUDENT_ANS_SHEET();
            bool isUpdate = true;
            var data = (from t in this.context.STUDENT_ANS_SHEET
                        where t.test_id == studAnswerSheet.TestInfo.TestID && t.stud_id==studAnswerSheet.StudentInfo.StudentID
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

            return this.context.SaveChanges() > 0 ? ansSheet.test_id : 0;
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
        
        public bool RemoveAnswerSheet(long ansID, string lastupdatedby)
        {
            var data = (from u in this.context.STUDENT_ANS_SHEET
                        where u.ans_sheet_id == ansID
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

        #endregion

        public async Task<long> TestMaintenance(TestDetailEntity TestDetail)
        {
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

            return 1;
        }

        public async Task<List<TestPaperEntity>> GetAllTestDocLinks(long branchID, long stdID, int batchTime)
        {
            var data = (from u in this.context.TEST_PAPER_REL
                        .Include("TEST_MASTER")
                        .Include("BRANCH_MASTER")
                        .Include("STD_MASTER")
                        where u.TEST_MASTER.branch_id == branchID && u.TEST_MASTER.std_id == stdID
                        && (batchTime == 0 || u.TEST_MASTER.batch_time_id == batchTime) && !u.doc_link.Equals(" ")
                        select new TestPaperEntity()
                        {
                            TestID = u.test_id,
                            Remarks = u.remakrs,
                            DocLink = u.doc_link
                        }).ToList();

            return data;
        }


        public bool RemoveTestAnswerSheetdetail(long TestID,long studid)
        {
            var data = (from u in this.context.STUDENT_ANS_SHEET
                        where u.test_id == TestID && u.stud_id== studid
                        select u).ToList();

            if (data != null)
            {
                this.context.STUDENT_ANS_SHEET.RemoveRange(data);
                this.context.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<TestEntity> GetTestDetails(long TestID, long SubjectID)
        {
            var data = (from u in this.context.TEST_MASTER                      
                        where u.test_id == TestID && u.sub_id==SubjectID
                        select new TestEntity()
                        {
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
       
    }
}
