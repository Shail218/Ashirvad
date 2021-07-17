using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Subject
{
    public class Subject : ModelAccess, ISubjectAPI
    {
        public async Task<long> SubjectMaintenance(SubjectEntity subjectInfo)
        {
            Model.SUBJECT_MASTER subjectMaster = new Model.SUBJECT_MASTER();

            bool isUpdate = true;
            var data = (from subject in this.context.SUBJECT_MASTER
                        where subject.subject_id == subjectInfo.SubjectID
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
            subjectMaster.row_sta_cd = subjectInfo.RowStatus.RowStatusId;
            subjectMaster.trans_id = this.AddTransactionData(subjectInfo.Transaction);
            this.context.SUBJECT_MASTER.Add(subjectMaster);
            if (isUpdate)
            {
                this.context.Entry(subjectMaster).State = System.Data.Entity.EntityState.Modified;
            }
            return this.context.SaveChanges() > 0 ? subjectMaster.subject_id : 0;
        }

        public async Task<List<SubjectEntity>> GetAllSubjects(long branchID)
        {
            var data = (from u in this.context.SUBJECT_MASTER
                        where branchID == 0 || u.branch_id == branchID
                        select new SubjectEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Subject = u.subject,
                            SubjectID = u.subject_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public async Task<List<SubjectEntity>> GetAllSubjects()
        {
            var data = (from u in this.context.SUBJECT_MASTER
                        select new SubjectEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Subject = u.subject,
                            SubjectID = u.subject_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public bool RemoveSubject(long SubjectID, string lastupdatedby)
        {
            var data = (from u in this.context.SUBJECT_MASTER
                        where u.subject_id == SubjectID
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

        public async Task<SubjectEntity> GetSubjectByID(long subjectID)
        {
            var data = (from u in this.context.SUBJECT_MASTER
                        where u.subject_id == subjectID
                        select new SubjectEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Subject = u.subject,
                            SubjectID = u.subject_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();

            return data;
        }
    }
}
