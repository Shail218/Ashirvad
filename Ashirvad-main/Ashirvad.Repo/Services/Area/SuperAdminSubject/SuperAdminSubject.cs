using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area;
using Ashirvad.Repo.DataAcceessAPI.Area.SuperAdminSubject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.SuperAdminSubject
{
    public class SuperAdminSubject : ModelAccess, ISuperAdminSubjectAPI
    {
        private readonly IBranchSubjectAPI _BranchSubject;

        public SuperAdminSubject(IBranchSubjectAPI branchSubject)
        {
            _BranchSubject = branchSubject;
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
                            where subject.subject_id == subjectMaster.subject_id
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
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();

            return data;
        }

        public async Task<List<SuperAdminSubjectEntity>> GetAllSubject()
        {
            var data = (from u in this.context.SUBJECT_BRANCH_MASTER
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

        public bool RemoveSubject(long subjectID, string lastupdatedby)
        {
            var data = (from u in this.context.SUBJECT_BRANCH_MASTER
                        where u.subject_id == subjectID
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
                    result = _BranchSubject.SubjectMaintenance(branchSubject).Result;
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
