using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.Repo.DataAcceessAPI.Area.Branch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area.Branch
{
    public class BranchSubject : ModelAccess, IBranchSubjectAPI
    {
        public async Task<long> CheckSubject(int SubjectDetailID, int SubjectID, int BranchID, int CourseDetailID, int ClassID)
        {
            long result;
            bool isExists = this.context.SUBJECT_DTL_MASTER.Where(s => (SubjectDetailID == 0 || s.subject_dtl_id != SubjectDetailID)
            && s.class_dtl_id == ClassID && s.subject_id == SubjectID && s.branch_id == BranchID && s.course_dtl_id == CourseDetailID && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<long> SubjectMaintenance(BranchSubjectEntity SubjectInfo)
        {
            Model.SUBJECT_DTL_MASTER SubjectMaster = new Model.SUBJECT_DTL_MASTER();
            if (CheckSubject((int)SubjectInfo.Subject_dtl_id, (int)SubjectInfo.Subject.SubjectID,
                (int)SubjectInfo.branch.BranchID, (int)SubjectInfo.BranchCourse.course_dtl_id, (int)SubjectInfo.BranchClass.Class_dtl_id).Result != -1)
            {
                bool isUpdate = true;
                var data = (from Subject in this.context.SUBJECT_DTL_MASTER
                            where Subject.subject_dtl_id == SubjectInfo.Subject_dtl_id
                            select new
                            {
                                SubjectMaster = Subject
                            }).FirstOrDefault();
                if (data == null)
                {
                    SubjectMaster = new Model.SUBJECT_DTL_MASTER();
                    isUpdate = false;
                }
                else
                {
                    SubjectMaster = data.SubjectMaster;
                    SubjectInfo.Transaction.TransactionId = data.SubjectMaster.trans_id;
                }

                SubjectMaster.subject_id = SubjectInfo.Subject.SubjectID;
                SubjectMaster.branch_id = SubjectInfo.branch.BranchID;
                SubjectMaster.row_sta_cd = (int)Enums.RowStatus.Active;
                SubjectMaster.is_subject = SubjectInfo.isSubject;
                SubjectMaster.course_dtl_id = SubjectInfo.BranchCourse.course_dtl_id;
                SubjectMaster.class_dtl_id = SubjectInfo.BranchClass.Class_dtl_id;
                SubjectMaster.trans_id = this.AddTransactionData(SubjectInfo.Transaction);
                this.context.SUBJECT_DTL_MASTER.Add(SubjectMaster);
                if (isUpdate)
                {
                    this.context.Entry(SubjectMaster).State = System.Data.Entity.EntityState.Modified;
                }
                var result = this.context.SaveChanges();

                if (result > 0)
                {
                    SubjectInfo.Subject_dtl_id = SubjectMaster.subject_dtl_id;
                    SubjectEntity Subjectmaster = new SubjectEntity();

                    Subjectmaster.BranchInfo = new BranchEntity()
                    {
                        BranchID = SubjectInfo.branch.BranchID,

                    };
                    SubjectInfo.isSubject = Subjectmaster.BranchInfo.BranchID == 2 ? true : SubjectInfo.isSubject;
                    Subjectmaster.BranchSubject = new BranchSubjectEntity();
                    Subjectmaster.Transaction = new TransactionEntity();
                    Subjectmaster.Transaction.TransactionId = SubjectMaster.trans_id;
                    Subjectmaster.Subject = SubjectInfo.Subject.SubjectName;
                    Subjectmaster.BranchSubject.Subject_dtl_id = SubjectInfo.Subject_dtl_id;
                    Subjectmaster.RowStatus = new RowStatusEntity()
                    {
                        RowStatus = SubjectInfo.isSubject == true ? Enums.RowStatus.Active : Enums.RowStatus.Inactive
                    };
                    if (SubjectInfo.isSubject)
                    {
                        var Result2 = SubjectMasterMaintenance(Subjectmaster);
                    }
                    
                    return result > 0 ? SubjectInfo.Subject_dtl_id : 0;
                }
                return this.context.SaveChanges() > 0 ? 1 : 0;
            }
            return -1;
        }

        public async Task<List<BranchSubjectEntity>> GetAllSubject(long BranchID)
        {
            var data = (from u in this.context.SUBJECT_DTL_MASTER
                        .Include("Subject_MASTER")
                        .Include("BRANCH_MASTER") orderby u.subject_dtl_id descending
                        where (BranchID == 0 || u.branch_id == BranchID) && u.row_sta_cd == 1 && u.is_subject == true
                        select new BranchSubjectEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Subject = new SuperAdminSubjectEntity()
                            {
                                SubjectID = u.SUBJECT_BRANCH_MASTER.subject_id,
                                SubjectName = u.SUBJECT_BRANCH_MASTER.subject_name
                            },
                            branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            isSubject = u.is_subject == true ? true : false,
                            Subject_dtl_id = u.subject_dtl_id,
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id,

                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id,

                            },
                            Class = new ClassEntity()
                            {
                                ClassID = u.CLASS_DTL_MASTER.CLASS_MASTER.class_id,
                                ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).ToList();
            if (data.Count > 0)
            {
                data[0].BranchSubjectData = (from u in this.context.SUBJECT_DTL_MASTER
                              .Include("Subject_MASTER")
                              .Include("BRANCH_MASTER")
                                             where (BranchID == 0 || u.branch_id == BranchID) && u.row_sta_cd == 1
                                             select new BranchSubjectEntity()
                                             {
                                                 branch = new BranchEntity()
                                                 {
                                                     BranchID = u.BRANCH_MASTER.branch_id,
                                                     BranchName = u.BRANCH_MASTER.branch_name
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

                                                 },
                                                 Class = new ClassEntity()
                                                 {
                                                     ClassID = u.CLASS_DTL_MASTER.CLASS_MASTER.class_id,
                                                     ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                                 },
                                             }).Distinct().ToList();
            }
            else
            {
                BranchSubjectEntity entity = new BranchSubjectEntity();
                entity.BranchSubjectData = new List<BranchSubjectEntity>();
                data.Add(entity);
            }
            return data;
        }

        public async Task<List<BranchSubjectEntity>> GetAllSubjects(DataTableAjaxPostModel model, long BranchID)
        {
            bool Isasc = model.order[0].dir == "desc" ? false : true;
            long count = (from u in this.context.SUBJECT_DTL_MASTER
                          orderby u.subject_dtl_id descending
                          where u.branch_id == BranchID && u.row_sta_cd == 1 && u.is_subject == true
                          select new BranchSubjectEntity()
                          {
                              branch = new BranchEntity()
                              {
                                  BranchID = u.BRANCH_MASTER.branch_id,
                                  BranchName = u.BRANCH_MASTER.branch_name
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
                              }
                          }).Distinct().Count();
            var data = (from u in this.context.SUBJECT_DTL_MASTER
                        .Include("Subject_MASTER")
                        .Include("BRANCH_MASTER")
                        where (BranchID == 0 || u.branch_id == BranchID) && u.row_sta_cd == 1 && u.is_subject == true
                        && (model.search.value == null
                        || model.search.value == ""
                        || u.COURSE_DTL_MASTER.COURSE_MASTER.course_name.ToLower().Contains(model.search.value)
                        || u.CLASS_DTL_MASTER.CLASS_MASTER.class_name.ToLower().Contains(model.search.value))
                        select new BranchSubjectEntity()
                        {                            
                            branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
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
                            },
                            Count = count,
                            Class = new ClassEntity()
                            {
                                ClassID = u.CLASS_DTL_MASTER.CLASS_MASTER.class_id,
                                ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                            }                            
                        })
                        .Distinct()
                        .OrderByDescending(a => a.BranchClass.Class_dtl_id)
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();
            foreach (BranchSubjectEntity branchSubjectEntity in data)
            {
                branchSubjectEntity.BranchSubjectData = (from u in this.context.SUBJECT_DTL_MASTER
                              .Include("Subject_MASTER")
                              .Include("BRANCH_MASTER")
                                             where u.class_dtl_id == branchSubjectEntity.BranchClass.Class_dtl_id && u.row_sta_cd == 1 && u.is_subject == true
                                             select new BranchSubjectEntity()
                                             {
                                                 RowStatus = new RowStatusEntity()
                                                 {
                                                     RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                                     RowStatusId = (int)u.row_sta_cd
                                                 },
                                                 branch = new BranchEntity()
                                                 {
                                                     BranchID = u.BRANCH_MASTER.branch_id,
                                                     BranchName = u.BRANCH_MASTER.branch_name
                                                 },
                                                 isSubject = u.is_subject == true ? true : false,
                                                 Subject_dtl_id = u.subject_dtl_id,
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

                                                 },
                                                 Subject = new SuperAdminSubjectEntity()
                                                 {
                                                     SubjectID = u.SUBJECT_BRANCH_MASTER.subject_id,
                                                     SubjectName = u.SUBJECT_BRANCH_MASTER.subject_name
                                                 },
                                                 Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                                             }).Distinct().ToList();
            }
            return data;
        }

        public async Task<List<BranchSubjectEntity>> GetSubjectBySubjectID(long SubjectID, long BranchID, long CourseID)
        {
            var data = (from u in this.context.SUBJECT_DTL_MASTER
                       .Include("Subject_MASTER")
                        orderby u.subject_dtl_id descending
                        where u.row_sta_cd == 1 && u.class_dtl_id == SubjectID && u.branch_id == BranchID && u.course_dtl_id == CourseID
                        select new BranchSubjectEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Subject = new SuperAdminSubjectEntity()
                            {
                                SubjectID = u.SUBJECT_BRANCH_MASTER.subject_id,
                                SubjectName = u.SUBJECT_BRANCH_MASTER.subject_name
                            },
                            Subject_dtl_id = u.subject_dtl_id,
                            isSubject = u.is_subject == true ? true : false,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).ToList();
            if (data.Count > 0)
            {
                data[0].branchSubject = (from u in this.context.SUBJECT_DTL_MASTER
                              .Include("Subject_MASTER")
                              .Include("BRANCH_MASTER")
                                         where u.row_sta_cd == 1 && u.class_dtl_id == SubjectID && u.branch_id == BranchID
                                         select new BranchSubjectEntity()
                                         {
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

                                             },
                                         }).FirstOrDefault();
            }
            return data;
        }

        public async Task<BranchSubjectEntity> GetSubjectbyID(long SubjectID)
        {
            var data = (from u in this.context.SUBJECT_DTL_MASTER
                       .Include("Subject_MASTER")
                        where u.row_sta_cd == 1 && u.branch_id == SubjectID
                        select new BranchSubjectEntity()
                        {
                            Subject_dtl_id = u.subject_dtl_id,
                            Subject = new SuperAdminSubjectEntity()
                            {
                                SubjectID = u.SUBJECT_BRANCH_MASTER.subject_id,
                                SubjectName = u.SUBJECT_BRANCH_MASTER.subject_name
                            },

                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                        }).FirstOrDefault();
            return data;
        }

        public bool RemoveSubject(long CourseID, long ClassID, long BranchID, string lastupdatedby)
        {
            var data = (from u in this.context.SUBJECT_DTL_MASTER
                        where u.branch_id == BranchID &&
                        u.course_dtl_id == CourseID &&
                        u.class_dtl_id == ClassID &&
                        u.row_sta_cd == (int)Enums.RowStatus.Active
                        select u).ToList();
            if (data != null)
            {
                foreach (var item in data)
                {
                    item.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    item.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = item.trans_id, LastUpdateBy = lastupdatedby });
                    bool Status = RemoveSubjectMaster(item.SUBJECT_BRANCH_MASTER.subject_name, BranchID, lastupdatedby);
                    this.context.SaveChanges();
                }

                return true;
            }

            return false;
        }

        public async Task<bool> CheckSubject(long SubjectID, string Subject, long BranchID)
        {

            bool isExists = this.context.SUBJECT_MASTER.Where(s => (SubjectID == 0 || s.subject_dtl_id != SubjectID)
            && s.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name == Subject && s.branch_id == BranchID && s.row_sta_cd == 1).FirstOrDefault() != null;
            return isExists;
        }

        public async Task<long> SubjectMasterMaintenance(SubjectEntity subjectInfo)
        {
            try
            {
                Model.SUBJECT_MASTER subjectMaster = new Model.SUBJECT_MASTER();
                bool IsSuccess = CheckSubject(subjectInfo.BranchSubject.Subject_dtl_id, subjectInfo.Subject, subjectInfo.BranchInfo.BranchID).Result;
                if (!IsSuccess)
                {
                    bool isUpdate = true;
                    var data = (from subject in this.context.SUBJECT_MASTER
                                where subject.subject_dtl_id == subjectInfo.BranchSubject.Subject_dtl_id
                                && subject.branch_id == subjectInfo.BranchInfo.BranchID
                                select subject).FirstOrDefault();
                    if (data == null)
                    {
                        subjectMaster = new Model.SUBJECT_MASTER();
                        isUpdate = false;
                    }
                    else
                    {
                        subjectMaster = data;
                        subjectInfo.Transaction.TransactionId = data.trans_id;
                    }

                    subjectMaster.subject = subjectInfo.Subject;
                    subjectMaster.branch_id = subjectInfo.BranchInfo.BranchID;
                    subjectMaster.row_sta_cd = (int)subjectInfo.RowStatus.RowStatus;
                    subjectMaster.trans_id = subjectInfo.Transaction.TransactionId;
                    subjectMaster.subject_dtl_id = subjectInfo.BranchSubject.Subject_dtl_id;
                    this.context.SUBJECT_MASTER.Add(subjectMaster);
                    if (isUpdate)
                    {
                        this.context.Entry(subjectMaster).State = System.Data.Entity.EntityState.Modified;
                    }
                    return this.context.SaveChanges() > 0 ? subjectMaster.subject_id : 0;
                }
            }
            catch (Exception ex)
            {

            }
            return 0;

        }

        public bool RemoveSubjectMaster(string SubjectName, long BranchID, string lastupdatedby)
        {
            var data1 = (from u in this.context.SUBJECT_DTL_MASTER
                        where u.branch_id == BranchID 
                        && u.SUBJECT_BRANCH_MASTER.subject_name == SubjectName 
                        && u.is_subject == true 
                        && u.row_sta_cd == (int)Enums.RowStatus.Active
                        select u).ToList();
            if (data1.Count == 0)
            {
                var data = (from u in this.context.SUBJECT_MASTER
                            where u.branch_id == BranchID && u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name == SubjectName && u.row_sta_cd == (int)Enums.RowStatus.Active
                            select u).ToList();
                if (data?.Count>0)
                {
                    foreach (var item in data)
                    {
                        item.row_sta_cd = (int)Enums.RowStatus.Inactive;
                        item.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = item.trans_id, LastUpdateBy = lastupdatedby });
                        this.context.SaveChanges();
                    }

                    return true;
                }
            }
            return false;
        }

        public async Task<List<BranchSubjectEntity>> GetMobileAllSubject(long BranchID)
        {
            var data = (from u in this.context.SUBJECT_DTL_MASTER
                      .Include("Subject_MASTER")
                      .Include("BRANCH_MASTER")
                        orderby u.subject_dtl_id descending
                        where (BranchID == 0 || u.branch_id == BranchID) && u.row_sta_cd == 1
                        select new BranchSubjectEntity()
                        {
                            branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
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

                            },
                            Class = new ClassEntity()
                            {
                                ClassID = u.CLASS_DTL_MASTER.CLASS_MASTER.class_id,
                                ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                            },
                        }).Distinct().ToList();
            if (data?.Count > 0)
            {
                foreach (var i in data)
                {
                    i.BranchSubjectData = (from u in this.context.SUBJECT_DTL_MASTER
                              .Include("Subject_MASTER")
                              .Include("BRANCH_MASTER")
                                           where (BranchID == 0 || u.branch_id == BranchID) && u.row_sta_cd == 1 && u.course_dtl_id == i.BranchCourse.course_dtl_id && u.class_dtl_id == i.BranchClass.Class_dtl_id
                                           select new BranchSubjectEntity()
                                           {
                                               RowStatus = new RowStatusEntity()
                                               {
                                                   RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                                   RowStatusId = (int)u.row_sta_cd
                                               },
                                               Subject = new SuperAdminSubjectEntity()
                                               {
                                                   SubjectID = u.SUBJECT_BRANCH_MASTER.subject_id,
                                                   SubjectName = u.SUBJECT_BRANCH_MASTER.subject_name
                                               },
                                               branch = new BranchEntity()
                                               {
                                                   BranchID = u.BRANCH_MASTER.branch_id,
                                                   BranchName = u.BRANCH_MASTER.branch_name
                                               },
                                               isSubject = u.is_subject == true ? true : false,
                                               Subject_dtl_id = u.subject_dtl_id,
                                               BranchCourse = new BranchCourseEntity()
                                               {
                                                   course_dtl_id = u.course_dtl_id,

                                               },
                                               BranchClass = new BranchClassEntity()
                                               {
                                                   Class_dtl_id = u.class_dtl_id,

                                               },
                                               Class = new ClassEntity()
                                               {
                                                   ClassID = u.CLASS_DTL_MASTER.CLASS_MASTER.class_id,
                                                   ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                               },
                                               Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                                           }).ToList();
                }

            }

            return data;
        }

        public async Task<List<BranchSubjectEntity>> GetMobileSubjectBySubjectID(long SubjectID, long BranchID, long CourseID)
        {
            List<BranchSubjectEntity> subjectEntities = new List<BranchSubjectEntity>();
            var data = (from u in this.context.SUBJECT_DTL_MASTER
                       .Include("Subject_MASTER")
                       .Include("BRANCH_MASTER")
                        orderby u.subject_dtl_id descending
                        where u.row_sta_cd == 1 && u.class_dtl_id == SubjectID && u.branch_id == BranchID

                        select new BranchSubjectEntity()
                        {
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

                            },
                        }).FirstOrDefault();
            if (data != null)
            {
                data.BranchSubjectData = (from u in this.context.SUBJECT_DTL_MASTER
                              .Include("Subject_MASTER")
                                          where u.row_sta_cd == 1 && u.class_dtl_id == SubjectID && u.branch_id == BranchID && u.course_dtl_id == CourseID
                                          select new BranchSubjectEntity()
                                          {
                                              RowStatus = new RowStatusEntity()
                                              {
                                                  RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                                  RowStatusId = (int)u.row_sta_cd
                                              },
                                              Subject = new SuperAdminSubjectEntity()
                                              {
                                                  SubjectID = u.SUBJECT_BRANCH_MASTER.subject_id,
                                                  SubjectName = u.SUBJECT_BRANCH_MASTER.subject_name
                                              },
                                              Subject_dtl_id = u.subject_dtl_id,
                                              isSubject = u.is_subject == true ? true : false,
                                              Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                                          }).ToList();
                subjectEntities.Add(data);
            }



            return subjectEntities;
        }

        public async Task<List<BranchSubjectEntity>> GetAllSelectedSubjects(long BranchID, long CourseID, long ClassID)
        {
            var data = (from u in this.context.SUBJECT_DTL_MASTER
                        orderby u.SUBJECT_BRANCH_MASTER.subject_name
                        where (u.class_dtl_id == ClassID || ClassID == 0)
                        && (u.course_dtl_id == CourseID || CourseID == 0)
                        && (u.branch_id == BranchID || BranchID == 0)
                        && u.row_sta_cd == 1 && u.is_subject == true
                        select new BranchSubjectEntity()
                        {
                            Subject = new SuperAdminSubjectEntity()
                            {
                                SubjectID = u.subject_id,
                                SubjectName = u.SUBJECT_BRANCH_MASTER.subject_name
                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class = new ClassEntity()
                                {
                                    ClassID = u.CLASS_DTL_MASTER.CLASS_MASTER.class_id,
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                },
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course = new CourseEntity()
                                {
                                    CourseID = u.COURSE_DTL_MASTER.COURSE_MASTER.course_id,
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                },
                            }

                        }).Distinct().ToList();

            return data;

        }
    }
}
