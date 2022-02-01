using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.Repo.DataAcceessAPI.Area.Subject;
using Ashirvad.Repo.DataAcceessAPI.Area.SuperAdminSubject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area.SuperAdminSubject
{
    public class SuperAdminSubject : ModelAccess, ISuperAdminSubjectAPI
    {
        private readonly IBranchSubjectAPI _BranchSubject;
        private readonly ISubjectAPI _subject;

        public SuperAdminSubject(IBranchSubjectAPI branchSubject, ISubjectAPI subject)
        {
            _BranchSubject = branchSubject;
            _subject = subject;
        }

        public async Task<long> CheckSubject(string name,long courseid,long classid,long Id)
        {
            long result;
            bool isExists = this.context.SUBJECT_BRANCH_MASTER.Where(s => (Id == 0 || s.subject_id != Id) && s.course_id == courseid && s.class_id == classid && s.subject_name == name && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<long> SubjectMaintenance(SuperAdminSubjectEntity subjectEntity)
        {
            Model.SUBJECT_BRANCH_MASTER subjectMaster = new Model.SUBJECT_BRANCH_MASTER();
            if (CheckSubject(subjectEntity.SubjectName, subjectEntity.courseEntity.CourseID,subjectEntity.classEntity.ClassID ,subjectEntity.SubjectID).Result != -1)
            {
                bool isUpdate = true;
                var data = (from subject in this.context.SUBJECT_BRANCH_MASTER
                            where subject.subject_id == subjectEntity.SubjectID
                            select new
                            {
                                subjectMaster = subject
                            }).FirstOrDefault();
                if (data == null)
                {
                    subjectMaster = new Model.SUBJECT_BRANCH_MASTER();
                    isUpdate = false;
                }
                else
                {
                    subjectMaster = data.subjectMaster;
                    subjectEntity.Transaction.TransactionId = data.subjectMaster.trans_id;
                }
                subjectMaster.subject_name = subjectEntity.SubjectName;
                subjectMaster.row_sta_cd = subjectEntity.RowStatus.RowStatusId;
                subjectMaster.trans_id = this.AddTransactionData(subjectEntity.Transaction);
                subjectMaster.course_id = subjectEntity.courseEntity.CourseID;
                subjectMaster.class_id = subjectEntity.classEntity.ClassID;
                this.context.SUBJECT_BRANCH_MASTER.Add(subjectMaster);
                if (isUpdate)
                {
                    this.context.Entry(subjectMaster).State = System.Data.Entity.EntityState.Modified;
                }
                var Result = this.context.SaveChanges();
                if (Result > 0)
                {
                    subjectEntity.SubjectID = subjectMaster.subject_id;
                    subjectEntity.Transaction.TransactionId = subjectEntity.Transaction.TransactionId;
                    if((int)subjectEntity.UserType == 5)
                    {
                        SuperAdminSubjectMasterMaintenance(subjectEntity);
                    }
                    else
                    {
                        SubjectMasterMaintenance(subjectEntity);
                    }                   
                    //UpdateSubject(subjectEntity);
                }
                return this.context.SaveChanges() > 0 ? subjectEntity.SubjectID : 0;
            }
            else
            {
                return -1;
            }
        }

        public async Task<SuperAdminSubjectEntity> GetSubjectBySubjectID(long subjectID)
        {
            var data = (from u in this.context.SUBJECT_BRANCH_MASTER
                        .Include("COURSE_MASTER")
                        .Include("CLASS_MASTER")
                        where u.subject_id == subjectID
                        select new SuperAdminSubjectEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            courseEntity = new CourseEntity()
                            {
                                CourseID = u.COURSE_MASTER.course_id,
                                CourseName = u.COURSE_MASTER.course_name
                            },
                            classEntity = new ClassEntity()
                            {
                                ClassID = u.CLASS_MASTER.class_id,
                                ClassName = u.CLASS_MASTER.class_name
                            },
                            SubjectID = u.subject_id,
                            SubjectName = u.subject_name,
                            oldsubject = u.subject_name,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();

            return data;
        }

        public async Task<List<SuperAdminSubjectEntity>> GetAllSubject()
        {
            var data = (from u in this.context.SUBJECT_BRANCH_MASTER
                        orderby u.subject_id descending
                        where u.row_sta_cd == 1
                        select new SuperAdminSubjectEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            SubjectID = u.subject_id,
                            SubjectName = u.subject_name,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },

                        }).ToList();

            return data;
        }

        public async Task<List<SuperAdminSubjectEntity>> GetAllSubjectByCourseClass(long courseid,long classid)
        {
            long CourseID = 0, ClassID = 0;
            var coursedata = this.context.COURSE_DTL_MASTER.Where(s => s.course_dtl_id == courseid && s.row_sta_cd == 1).FirstOrDefault();
            CourseID = coursedata == null ? CourseID : coursedata.course_id;
            var classdata = this.context.CLASS_DTL_MASTER.Where(s => s.class_dtl_id == classid && s.row_sta_cd == 1).FirstOrDefault();
            ClassID = classdata == null ? ClassID : classdata.class_id;
            var data = (from u in this.context.SUBJECT_BRANCH_MASTER
                        orderby u.subject_id descending
                        where u.row_sta_cd == 1 && u.course_id == CourseID && u.class_id == ClassID
                        select new SuperAdminSubjectEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            SubjectID = u.subject_id,
                            SubjectName = u.subject_name,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },

                        }).OrderByDescending(a => a.SubjectID).ToList();

            return data;
        }

        public bool CheckHistory(long subjectID)
        {
            bool Issuccess = true;
            Issuccess = this.context.SUBJECT_DTL_MASTER.Where(s => s.subject_id == subjectID && s.is_subject == true && s.row_sta_cd == 1).FirstOrDefault() != null;
            if (Issuccess)
            {
                return false;
            }
            return true;
        }

        public bool RemoveSubject(long subjectID, string lastupdatedby)
        {
            bool Isvalid = CheckHistory(subjectID);
            if (Isvalid)
            {
                var data = (from u in this.context.SUBJECT_BRANCH_MASTER
                            where u.subject_id == subjectID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                }
                var data1 = (from u in this.context.SUBJECT_DTL_MASTER
                             where u.subject_id == subjectID && u.is_subject == false && u.row_sta_cd == 1
                             select u).FirstOrDefault();
                if (data1 != null)
                {
                    data1.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data1.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data1.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public async Task<long> SubjectMasterMaintenance(SuperAdminSubjectEntity subjectentity)
        {
            try
            {
                long result = 0;

                var List = GetSubjectdetails(subjectentity.courseEntity.CourseID, subjectentity.classEntity.ClassID).Result;

                BranchSubjectEntity branchSubject = new BranchSubjectEntity();
                branchSubject.Subject = new SuperAdminSubjectEntity()
                {
                    SubjectID = subjectentity.SubjectID,
                    SubjectName = subjectentity.SubjectName
                };
                branchSubject.UserType = subjectentity.UserType;
                branchSubject.Transaction = new TransactionEntity();
                branchSubject.Transaction = subjectentity.Transaction;
                branchSubject.isSubject = false;
                branchSubject.RowStatus = new RowStatusEntity()
                {
                    RowStatus = Enums.RowStatus.Active
                };
                foreach (var item in List)
                {
                    branchSubject.branch = item.branch;
                    branchSubject.BranchCourse = new BranchCourseEntity()
                    {
                        course_dtl_id = item.BranchCourse.course_dtl_id,

                    };
                    branchSubject.BranchClass = new BranchClassEntity()
                    {
                        Class_dtl_id = item.BranchClass.Class_dtl_id
                    };
                    
                    result = _BranchSubject.SubjectMaintenance(branchSubject).Result;
                }
                //if ((int)subjectentity.UserType == 5)
                //{
                //    SubjectEntity subject = new SubjectEntity();
                //    subject.BranchInfo = new BranchEntity()
                //    {
                //        BranchID = subjectentity.branchEntity.BranchID,

                //    };
                //    subject.BranchSubject = new BranchSubjectEntity()
                //    {
                //        Subject_dtl_id = 0,
                //    };
                //    subject.Subject = subjectentity.SubjectName;
                //    subject.Transaction = new TransactionEntity()
                //    {
                //        TransactionId = 1
                //    };
                //    subject.RowStatus = new RowStatusEntity()
                //    {
                //        RowStatus = Enums.RowStatus.Active
                //    };
                //    if (subjectentity.SubjectID > 0)
                //    {
                //        Model.SUBJECT_MASTER sub_ = new Model.SUBJECT_MASTER();
                //        var std = (from sub in this.context.SUBJECT_MASTER
                //                   where sub.subject == subjectentity.oldsubject
                //                   && sub.row_sta_cd == 1
                //                   && sub.branch_id == subjectentity.branchEntity.BranchID
                //                   select new
                //                   {
                //                       sub_ = sub
                //                   }).FirstOrDefault();

                //        if (std != null)
                //        {
                //            subject.SubjectID = std.sub_.subject_id;
                //        }
                //    }

                //    result = _subject.SubjectMaintenance(subject).Result;
                //}
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<long> SuperAdminSubjectMasterMaintenance(SuperAdminSubjectEntity subjectentity)
        {
            try
            {
                long result = 0;

                var List = (from u in context.CLASS_DTL_MASTER
                            where u.branch_id == subjectentity.branchEntity.BranchID 
                            && u.row_sta_cd == 1
                            && u.class_id == subjectentity.classEntity.ClassID
                            && u.COURSE_DTL_MASTER.course_id == subjectentity.courseEntity.CourseID
                            select new BranchSubjectEntity
                            {
                                BranchCourse = new BranchCourseEntity()
                                {
                                    course_dtl_id = u.COURSE_DTL_MASTER.course_dtl_id
                                },
                                BranchClass = new BranchClassEntity()
                                {
                                    Class_dtl_id = u.class_dtl_id
                                },
                                branch = new BranchEntity()
                                {
                                    BranchID = u.branch_id
                                }
                            }).ToList();

                BranchSubjectEntity branchSubject = new BranchSubjectEntity();
                branchSubject.Subject = new SuperAdminSubjectEntity()
                {
                    SubjectID = subjectentity.SubjectID,
                    SubjectName = subjectentity.SubjectName
                };
                branchSubject.UserType = subjectentity.UserType;
                branchSubject.Transaction = new TransactionEntity();
                branchSubject.Transaction = subjectentity.Transaction;
                branchSubject.isSubject = true;
                branchSubject.RowStatus = new RowStatusEntity()
                {
                    RowStatus = Enums.RowStatus.Active
                };
                foreach (var item in List)
                {
                    branchSubject.branch = item.branch;
                    branchSubject.BranchCourse = new BranchCourseEntity()
                    {
                        course_dtl_id = item.BranchCourse.course_dtl_id,

                    };
                    branchSubject.BranchClass = new BranchClassEntity()
                    {
                        Class_dtl_id = item.BranchClass.Class_dtl_id
                    };

                    result = _BranchSubject.SubjectMaintenance(branchSubject).Result;
                }               
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<List<SuperAdminSubjectEntity>> GetAllCustomSubject(DataTableAjaxPostModel model)
        {
            var Count = this.context.SUBJECT_BRANCH_MASTER.Where(a => a.row_sta_cd == 1).Count();
            var data = (from u in this.context.SUBJECT_BRANCH_MASTER
                        .Include("COURSE_MASTER")
                        .Include("CLASS_MASTER")
                        orderby u.subject_id descending
                        where u.row_sta_cd == 1
                        select new SuperAdminSubjectEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            courseEntity = new CourseEntity()
                            {
                                CourseName = u.COURSE_MASTER.course_name
                            },
                            classEntity = new ClassEntity()
                            {
                                ClassName = u.CLASS_MASTER.class_name
                            },
                            SubjectID = u.subject_id,
                            courseid = u.course_id,
                            classid = u.class_id,
                            SubjectName = u.subject_name,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            Count = Count
                        })
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();

            return data;
        }


        public async Task<long> UpdateSubject(SuperAdminSubjectEntity subjectEntity)
        {
            try
            {
                long result = 0;
                var data = (from std in this.context.SUBJECT_MASTER
                            where std.SUBJECT_DTL_MASTER.subject_id == subjectEntity.SubjectID
                            && std.row_sta_cd == 1
                            select new SubjectEntity
                            {
                                SubjectID = std.subject_id
                            }).Distinct().ToList();
                if (data?.Count > 0)
                {
                    foreach (var item in data)
                    {
                        Model.SUBJECT_MASTER _MASTER = new Model.SUBJECT_MASTER();

                        var master = (from cl in this.context.SUBJECT_MASTER
                                      where cl.subject_id == item.SubjectID
                                      select new
                                      {
                                          _MASTER = cl
                                      }).FirstOrDefault();
                        if (master != null)
                        {
                            _MASTER = master._MASTER;
                            _MASTER.subject = subjectEntity.SubjectName;
                            this.context.SUBJECT_MASTER.Add(_MASTER);
                            this.context.Entry(_MASTER).State = System.Data.Entity.EntityState.Modified;
                            var Result = this.context.SaveChanges();

                        }


                    }
                }


                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }



        public async Task<List<BranchSubjectEntity>> GetAllSubjectByCourseClassddl(long courseid, long classid, bool Isupdate = false)
        {
            var data = (from u in this.context.SUBJECT_DTL_MASTER
                        orderby u.subject_dtl_id descending
                        where u.row_sta_cd == 1
                        && u.class_dtl_id == classid
                        && u.course_dtl_id == courseid
                        
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
                        }).OrderByDescending(a => a.Subject.SubjectID).ToList();

            if (data?.Count == 0)
            {
                long CourseID = 0, ClassID = 0;
                var coursedata = this.context.COURSE_DTL_MASTER.Where(s => s.course_dtl_id == courseid && s.row_sta_cd == 1).FirstOrDefault();
                CourseID = coursedata == null ? CourseID : coursedata.course_id;
                var classdata = this.context.CLASS_DTL_MASTER.Where(s => s.class_dtl_id == classid && s.row_sta_cd == 1).FirstOrDefault();
                ClassID = classdata == null ? ClassID : classdata.class_id;
                var data1 = (from u in this.context.SUBJECT_BRANCH_MASTER
                            orderby u.subject_id descending
                            where u.row_sta_cd == 1 && u.course_id == CourseID && u.class_id == ClassID
                            select new BranchSubjectEntity()
                            {
                                RowStatus = new RowStatusEntity()
                                {
                                    RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                    RowStatusId = (int)u.row_sta_cd,
                                    RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                                },
                                Subject = new SuperAdminSubjectEntity()
                                {
                                    SubjectID = u.subject_id,
                                    SubjectName = u.subject_name
                                },
                               
                                Transaction = new TransactionEntity() { TransactionId = u.trans_id },

                            }).OrderByDescending(a => a.Subject.SubjectID).ToList();
                return data1;
            }

            return data;
        }


        public async Task<List<BranchSubjectEntity>> GetSubjectdetails(long CourseID, long ClassID)
        {
            var data = (from u in this.context.SUBJECT_DTL_MASTER
                        where u.row_sta_cd == 1 
                        && u.CLASS_DTL_MASTER.class_id == ClassID
                        && u.CLASS_DTL_MASTER.COURSE_DTL_MASTER.course_id == CourseID
                        select new BranchSubjectEntity()
                        {

                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id
                            },
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id
                            },
                            branch = new BranchEntity()
                            {
                                BranchID = u.branch_id
                            }

                        }).Distinct().ToList();

            return data;
        }
    }
}
