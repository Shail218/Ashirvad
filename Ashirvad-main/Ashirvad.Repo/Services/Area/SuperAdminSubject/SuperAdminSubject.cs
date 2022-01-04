﻿using Ashirvad.Common;
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

        public async Task<long> CheckSubject(string name, long Id)
        {
            long result;
            bool isExists = this.context.SUBJECT_BRANCH_MASTER.Where(s => (Id == 0 || s.subject_id != Id) && s.subject_name == name && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<long> SubjectMaintenance(SuperAdminSubjectEntity subjectEntity)
        {
            Model.SUBJECT_BRANCH_MASTER subjectMaster = new Model.SUBJECT_BRANCH_MASTER();
            if (CheckSubject(subjectEntity.SubjectName, subjectEntity.SubjectID).Result != -1)
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
                    SubjectMasterMaintenance(subjectEntity);
                    UpdateSubject(subjectEntity);
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
                        where u.subject_id == subjectID
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
            if(Isvalid)
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
                var data = (from Subject in this.context.SUBJECT_DTL_MASTER
                            select new BranchSubjectEntity
                            {
                                branch = new BranchEntity()
                                {
                                    BranchID = Subject.branch_id
                                },
                                Subject = new SuperAdminSubjectEntity()
                                {
                                    SubjectID = Subject.subject_id
                                },
                                BranchCourse = new BranchCourseEntity()
                                {
                                    course_dtl_id = Subject.course_dtl_id

                                },
                                BranchClass = new BranchClassEntity()
                                {
                                    Class_dtl_id = Subject.class_dtl_id
                                }
                            }).Distinct().ToList();

                BranchSubjectEntity branchSubject = new BranchSubjectEntity();
                branchSubject.Subject = new SuperAdminSubjectEntity()
                {
                    SubjectID = subjectentity.SubjectID,
                    SubjectName = subjectentity.SubjectName
                };
                branchSubject.UserType = subjectentity.UserType;
                branchSubject.Transaction = new TransactionEntity();
                branchSubject.Transaction = subjectentity.Transaction;
                branchSubject.isClass = false;
                branchSubject.RowStatus = new RowStatusEntity()
                {
                    RowStatus = Enums.RowStatus.Active
                };
                foreach (var item in data)
                {
                    branchSubject.branch = new BranchEntity()
                    {
                        BranchID = item.branch.BranchID,

                    };
                    branchSubject.BranchCourse = new BranchCourseEntity()
                    {
                        course_dtl_id = item.BranchCourse.course_dtl_id,

                    };
                    branchSubject.BranchClass = new BranchClassEntity()
                    {
                        Class_dtl_id = item.BranchClass.Class_dtl_id
                    };
                    branchSubject.Subject_dtl_id = 0;
                    result = _BranchSubject.SubjectMaintenance(branchSubject).Result;
                }
                if ((int)subjectentity.UserType == 5)
                {
                    SubjectEntity subject = new SubjectEntity();
                    subject.BranchInfo = new BranchEntity()
                    {
                        BranchID = subjectentity.branchEntity.BranchID,

                    };
                    subject.BranchSubject = new BranchSubjectEntity()
                    {
                        Subject_dtl_id = 0,
                    };
                    subject.Subject = subjectentity.SubjectName;
                    subject.Transaction = new TransactionEntity()
                    {
                        TransactionId = 1
                    };
                    subject.RowStatus = new RowStatusEntity()
                    {
                        RowStatus = Enums.RowStatus.Active
                    };
                    if (subjectentity.SubjectID > 0)
                    {
                        Model.SUBJECT_MASTER sub_ = new Model.SUBJECT_MASTER();
                        var std = (from sub in this.context.SUBJECT_MASTER
                                   where sub.subject == subjectentity.oldsubject
                                   && sub.row_sta_cd == 1
                                   && sub.branch_id == subjectentity.branchEntity.BranchID
                                   select new
                                   {
                                       sub_ = sub
                                   }).FirstOrDefault();

                        if (std != null)
                        {
                            subject.SubjectID = std.sub_.subject_id;
                        }
                    }

                    result = _subject.SubjectMaintenance(subject).Result;
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
    }
}
