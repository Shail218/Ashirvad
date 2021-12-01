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
        public async Task<long> CheckSubject(string name, long branch, long Id)
        {
            long result;
            bool isExists = this.context.SUBJECT_MASTER.Where(s => (Id == 0 || s.subject_id != Id) && s.subject == name && s.branch_id == branch && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<long> SubjectMaintenance(SubjectEntity subjectInfo)
        {
            Model.SUBJECT_MASTER subjectMaster = new Model.SUBJECT_MASTER();
            if (CheckSubject(subjectInfo.Subject, subjectInfo.BranchInfo.BranchID, subjectInfo.SubjectID).Result != -1)
            {
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
            else
            {
                return -1;
            }

        }

        public async Task<List<SubjectEntity>> GetAllSubjects(long branchID)
        {
            var data = (from u in this.context.SUBJECT_MASTER
                        .Include("SUBJECT_DTL_MASTER")
                        where (branchID == 0 || u.branch_id == branchID) && u.row_sta_cd == 1
                        select new SubjectEntity()
                        {
                            Subject = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name,
                            SubjectID = u.subject_id,
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).Distinct().ToList();

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

        public async Task<List<SubjectEntity>> GetAllSubjectsName(long branchid)
        {
            var data = (from u in this.context.SUBJECT_MASTER
                        .Include("SUBJECT_DTL_MASTER")
                        where u.row_sta_cd == 1 && (u.branch_id == branchid || branchid == 0)
                        select new SubjectEntity()
                        {
                            Subject = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name
                        }).Distinct().ToList();
            return data;
        }

        public async Task<List<SubjectEntity>> GetAllSubjectsID(string subjectName, long branchid)
        {
            var data = (from u in this.context.SUBJECT_MASTER
                        .Include("SUBJECT_DTL_MASTER")
                        where u.row_sta_cd == 1 && u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name == subjectName && (u.branch_id == branchid || branchid == 0)
                        select new SubjectEntity()
                        {
                            Subject = u.subject,
                            SubjectID = u.subject_id
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

        public async Task<List<SubjectEntity>> GetAllSubjectsByTestDate(string TestDate)
        {
            DateTime dateTime = Convert.ToDateTime(TestDate);
            var data = (from u in this.context.TEST_MASTER
                        join sm in this.context.SUBJECT_MASTER on u.sub_id equals sm.subject_id
                        where (u.test_dt == dateTime && u.row_sta_cd == 1)
                        select new SubjectEntity()
                        {
                            testID = u.test_id,
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Subject = u.SUBJECT_MASTER.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name,
                            SubjectID = sm.subject_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }
    }
}
