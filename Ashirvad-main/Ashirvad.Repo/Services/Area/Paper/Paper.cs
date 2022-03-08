using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Paper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area.Paper
{
    public class Paper : ModelAccess, IPaperAPI
    {
        public async Task<long> PaperMaintenance(PaperEntity paperInfo)
        {
            Model.PRACTICE_PAPER paperMaster = new Model.PRACTICE_PAPER();
            Model.PRACTICE_PAPER_REL paperData = new Model.PRACTICE_PAPER_REL();
            bool isUpdate = true;
            var data = (from p in this.context.PRACTICE_PAPER.Include("PRACTICE_PAPER_REL")
                        where p.paper_id == paperInfo.PaperID
                        select new
                        {
                            paperMaster = p,
                            paperData = p.PRACTICE_PAPER_REL
                        }).FirstOrDefault();
            if (data == null)
            {
                paperMaster = new Model.PRACTICE_PAPER();
                paperData = new Model.PRACTICE_PAPER_REL();
                isUpdate = false;
            }
            else
            {
                paperMaster = data.paperMaster;
                paperData = data.paperData.FirstOrDefault();
                paperInfo.Transaction.TransactionId = data.paperMaster.trans_id;
            }

            paperMaster.row_sta_cd = paperInfo.RowStatus.RowStatusId;
            paperMaster.trans_id = this.AddTransactionData(paperInfo.Transaction);
            paperMaster.branch_id = paperInfo.Branch.BranchID;
            paperMaster.class_dtl_id = paperInfo.BranchClass.Class_dtl_id;
            paperMaster.course_dtl_id = paperInfo.BranchCourse.course_dtl_id;
            paperMaster.subject_dtl_id = paperInfo.BranchSubject.Subject_dtl_id;
            paperMaster.remarks = paperInfo.Remarks;
            paperMaster.batch_type = paperInfo.BatchTypeID;
            this.context.PRACTICE_PAPER.Add(paperMaster);
            if (isUpdate)
            {
                this.context.Entry(paperMaster).State = System.Data.Entity.EntityState.Modified;
            }
            if (!isUpdate)
            {
                paperData.paper_id = paperMaster.paper_id;
            }

            paperData.file_path = paperInfo.PaperData.FilePath;
            paperData.paper_file = paperInfo.PaperData.PaperPath;
            this.context.PRACTICE_PAPER_REL.Add(paperData);
            if (isUpdate)
            {
                this.context.Entry(paperData).State = System.Data.Entity.EntityState.Modified;
            }
            return this.context.SaveChanges() > 0 ? paperMaster.paper_id : 0;
        }

        public async Task<List<PaperEntity>> GetAllPapers(long branchID,string financialyear)
        {
            var data = (from u in this.context.PRACTICE_PAPER
                        .Include("PRACTICE_PAPER_REL")
                        .Include("BRANCH_MASTER")
                        .Include("CLASS_DTL_MASTER")
                        .Include("SUBJECT_MASTER")
                        join t in this.context.TRANSACTION_MASTER on u.trans_id equals t.trans_id
                        orderby u.paper_id descending
                        where (0 == branchID || u.branch_id == branchID && u.row_sta_cd == 1 && t.financial_year == financialyear)
                        select new PaperEntity()
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
                            BatchTypeID = u.batch_type,
                            BatchTypeText = u.batch_type == 1 ? "Morning" : u.batch_type == 2 ? "Afternoon" : "Evening",
                            PaperID = u.paper_id,
                            Remarks = u.remarks,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            PaperData = new PaperData()
                            {
                                FilePath = u.PRACTICE_PAPER_REL.Count == 0 ? " " : "https://mastermind.org.in" + u.PRACTICE_PAPER_REL.FirstOrDefault().file_path,
                                PaperID = u.PRACTICE_PAPER_REL.Count == 0 ? 0 : u.PRACTICE_PAPER_REL.FirstOrDefault().paper_id,
                                PaperPath = u.PRACTICE_PAPER_REL.Count == 0 ? " " : u.PRACTICE_PAPER_REL.FirstOrDefault().paper_file,
                                UniqueID = u.PRACTICE_PAPER_REL.Count == 0 ? 0 : u.PRACTICE_PAPER_REL.FirstOrDefault().unique_id
                            }
                        }).ToList();
            return data;
        }


        public async Task<List<SubjectEntity>> GetPracticePaperSubject(long branchID, long courseid,long stdID,int batch_time,string financialyear)
        {
            var data = (from u in this.context.PRACTICE_PAPER
                        .Include("PRACTICE_PAPER_REL")                        
                        join t in this.context.TRANSACTION_MASTER on u.trans_id equals t.trans_id
                        orderby u.paper_id descending
                        where u.branch_id == branchID
                        && u.class_dtl_id == stdID
                        && u.course_dtl_id == courseid 
                        && u.batch_type == batch_time
                        && t.financial_year == financialyear
                        select new SubjectEntity()
                        {
                            SubjectID = u.subject_dtl_id.HasValue? u.subject_dtl_id.Value:0,
                            Subject = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name
                        }).Distinct().ToList();

            return data;
        }

        public async Task<List<PaperEntity>> GetPracticePapersByStandardSubjectAndBranch(long branchID, long stdID, long subID, int batchTypeID,string financialyear)
        {
            var data = (from u in this.context.PRACTICE_PAPER
                        .Include("PRACTICE_PAPER_REL")
                        .Include("BRANCH_MASTER")
                        .Include("CLASS_DTL_MASTER")
                        .Include("SUBJECT_MASTER")
                        join t in this.context.TRANSACTION_MASTER on u.trans_id equals t.trans_id
                        orderby u.paper_id descending
                        where u.branch_id == branchID
                        && (0 == stdID || u.class_dtl_id == stdID)
                        && (0 == subID || u.subject_dtl_id == subID)
                        && (0 == batchTypeID || u.batch_type == batchTypeID)
                        && t.financial_year == financialyear
                        select new PaperEntity()
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
                            BatchTypeID = u.batch_type,
                            BatchTypeText = u.batch_type == 1 ? "Morning" : u.batch_type == 2 ? "Afternoon" : "Evening",
                            PaperID = u.paper_id,
                            Remarks = u.remarks,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            PaperData = new PaperData()
                            {
                                FilePath = u.PRACTICE_PAPER_REL.Count == 0 ? " " : "https://mastermind.org.in" + u.PRACTICE_PAPER_REL.FirstOrDefault().file_path,
                                PaperID = u.PRACTICE_PAPER_REL.Count == 0 ? 0 : u.PRACTICE_PAPER_REL.FirstOrDefault().paper_id,
                                PaperPath = u.PRACTICE_PAPER_REL.Count == 0 ? " " : u.PRACTICE_PAPER_REL.FirstOrDefault().paper_file,
                                UniqueID = u.PRACTICE_PAPER_REL.Count == 0 ? 0 : u.PRACTICE_PAPER_REL.FirstOrDefault().unique_id
                            }
                        }).ToList();

            return data;
        }

        public async Task<List<PaperEntity>> GetAllPaperWithoutContent(long branchID,string financialyear)
        {
            var data = (from u in this.context.PRACTICE_PAPER
                        .Include("PRACTICE_PAPER_REL")
                        .Include("BRANCH_MASTER")
                        .Include("CLASS_DTL_MASTER")
                        .Include("SUBJECT_MASTER")
   join t in this.context.TRANSACTION_MASTER on u.trans_id equals t.trans_id 
                        orderby u.paper_id descending
                        where (0 == branchID || u.branch_id == branchID) && u.row_sta_cd == 1 && t.financial_year == financialyear
                        select new PaperEntity()
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
                            BatchTypeID = u.batch_type,
                            BatchTypeText = u.batch_type == 1 ? "Morning" : u.batch_type == 2 ? "Afternoon" : "Evening",
                            PaperID = u.paper_id,
                            Remarks = u.remarks,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            PaperData = new PaperData()
                            {
                                FilePath = u.PRACTICE_PAPER_REL.Count == 0?" " :"https://mastermind.org.in" + u.PRACTICE_PAPER_REL.FirstOrDefault().file_path,
                                PaperID = u.PRACTICE_PAPER_REL.Count == 0 ? 0 : u.PRACTICE_PAPER_REL.FirstOrDefault().paper_id,
                                PaperPath = u.PRACTICE_PAPER_REL.Count == 0 ? " " : u.PRACTICE_PAPER_REL.FirstOrDefault().paper_file,
                                UniqueID = u.PRACTICE_PAPER_REL.Count == 0 ? 0 : u.PRACTICE_PAPER_REL.FirstOrDefault().unique_id
                            }
                        }).ToList();

            return data;
        }

        public async Task<List<PaperEntity>> GetAllCustomPaper(DataTableAjaxPostModel model, long branchID,string financialyear)
        {
            var Result = new List<PaperEntity>();
            bool Isasc = model.order[0].dir == "desc" ? false : true;
            long count = (from u in this.context.PRACTICE_PAPER
                          join t in this.context.TRANSACTION_MASTER on u.trans_id equals t.trans_id
                          where(u.row_sta_cd == 1 && u.branch_id == branchID && t.financial_year == financialyear)select u).Count();
            var data = (from u in this.context.PRACTICE_PAPER
                        .Include("PRACTICE_PAPER_REL")
                        .Include("BRANCH_MASTER")
                        .Include("CLASS_DTL_MASTER")
                        .Include("SUBJECT_MASTER")
                        join t in this.context.TRANSACTION_MASTER on u.trans_id equals t.trans_id
                        where (0 == branchID || u.branch_id == branchID) && u.row_sta_cd == 1
                         && t.financial_year == financialyear
                        && (model.search.value == null
                        || model.search.value == ""
                        || u.CLASS_DTL_MASTER.CLASS_MASTER.class_name.ToLower().Contains(model.search.value)
                        || u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name.ToLower().Contains(model.search.value))
                        orderby u.paper_id descending
                        select new PaperEntity()
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
                            BatchTypeID = u.batch_type,
                            Count = count,
                            BatchTypeText = u.batch_type == 1 ? "Morning" : u.batch_type == 2 ? "Afternoon" : "Evening",
                            PaperID = u.paper_id,
                            Remarks = u.remarks,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            PaperData = new PaperData()
                            {
                                FilePath = u.PRACTICE_PAPER_REL.Count == 0 ? " " : "https://mastermind.org.in" + u.PRACTICE_PAPER_REL.FirstOrDefault().file_path,
                                PaperID = u.PRACTICE_PAPER_REL.Count == 0 ? 0 : u.PRACTICE_PAPER_REL.FirstOrDefault().paper_id,
                                PaperPath = u.PRACTICE_PAPER_REL.Count == 0 ? " " : u.PRACTICE_PAPER_REL.FirstOrDefault().paper_file,
                                UniqueID = u.PRACTICE_PAPER_REL.Count == 0 ? 0 : u.PRACTICE_PAPER_REL.FirstOrDefault().unique_id
                            }
                        })
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();
            return data;
        }

        public async Task<PaperEntity> GetPaperByPaperID(long paperID,string financialyear)
        {
            var data = (from u in this.context.PRACTICE_PAPER
                        .Include("PRACTICE_PAPER_REL")
                        .Include("BRANCH_MASTER")
                        .Include("CLASS_DTL_MASTER")
                        .Include("SUBJECT_MASTER")
                        join t in this.context.TRANSACTION_MASTER on u.trans_id equals t.trans_id
                        where u.paper_id == paperID && t.financial_year == financialyear
                        select new PaperEntity()
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
                            BatchTypeID = u.batch_type,
                            BatchTypeText = u.batch_type == 1 ? "Morning" : u.batch_type == 2 ? "Afternoon" : "Evening",
                            PaperID = u.paper_id,
                            Remarks = u.remarks,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            PaperData = new PaperData()
                            {
                                FilePath = u.PRACTICE_PAPER_REL.Count == 0 ? " " : u.PRACTICE_PAPER_REL.FirstOrDefault().file_path,
                                PaperID = u.PRACTICE_PAPER_REL.Count == 0 ? 0 : u.PRACTICE_PAPER_REL.FirstOrDefault().paper_id,
                                PaperPath = u.PRACTICE_PAPER_REL.Count == 0 ? " " : u.PRACTICE_PAPER_REL.FirstOrDefault().paper_file,
                                UniqueID = u.PRACTICE_PAPER_REL.Count == 0 ? 0 : u.PRACTICE_PAPER_REL.FirstOrDefault().unique_id
                            }
                        }).FirstOrDefault();
            return data;
        }

        public bool RemovePaper(long paperID, string lastupdatedby)
        {
            var data = (from u in this.context.PRACTICE_PAPER
                        where u.paper_id == paperID
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
    }
}
