using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

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

        public async Task<long> MarksMaintenance(MarksEntity MarksInfo)
        {
            Model.MARKS_MASTER MarksMaster = new Model.MARKS_MASTER();
            bool isUpdate = true;
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
            MarksMaster.subject_id = MarksInfo.SubjectInfo.SubjectID;
            MarksMaster.batch_time_id = (int)MarksInfo.BatchType;
            MarksMaster.trans_id = this.AddTransactionData(MarksInfo.Transaction);
            this.context.MARKS_MASTER.Add(MarksMaster);
            if (isUpdate)
            {
                this.context.Entry(MarksMaster).State = System.Data.Entity.EntityState.Modified;
            }
            return this.context.SaveChanges() > 0 ? MarksMaster.marks_id : 0;
        }

        public async Task<List<MarksEntity>> GetAllMarks()
        {
            var data = (from u in this.context.MARKS_MASTER
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
                        where u.branch_id == Branch && (u.marks_id == MarksID || MarksID == 0) && (u.TEST_MASTER.std_id == Std || Std == 0) && (u.TEST_MASTER.batch_time_id == Batch || Batch == 0) && u.row_sta_cd == 1
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
                            SubjectInfo = new SubjectEntity()
                            {
                                SubjectID = u.SUBJECT_MASTER.subject_id,
                                Subject = u.SUBJECT_MASTER.subject
                            }
                        }).ToList();

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

        public bool RemoveMarks(long MarksID, string lastupdatedby)
        {
            var data = (from u in this.context.MARKS_MASTER
                        where u.marks_id == MarksID
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
                        where (u.branch_id == BranchID || BranchID == 0) && u.student_id == StudentID && u.row_sta_cd == 1
                        select new MarksEntity()
                        {
                            MarksID = u.marks_id,
                            MarksDate = u.marks_dt,                            
                            AchieveMarks = u.achive_marks,
                            MarksFilepath = "http://highpack-001-site12.dtempurl.com" + u.file_path,
                            MarksContentFileName = u.file_name,
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id },
                            testEntityInfo = new TestEntity()
                            {
                                TestID = u.test_id,
                                TestDate = u.TEST_MASTER.test_dt,
                                Marks = u.TEST_MASTER.total_marks
                            },
                            SubjectInfo = new SubjectEntity()
                            {
                                Subject = u.SUBJECT_MASTER.subject
                            }
                        }).ToList();

            return data;
        }

    }
}
