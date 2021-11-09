using Ashirvad.Common;
using Ashirvad.Data;
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
                return this.context.SaveChanges() > 0 ? subjectMaster.subject_id : 0;
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
    }
}
