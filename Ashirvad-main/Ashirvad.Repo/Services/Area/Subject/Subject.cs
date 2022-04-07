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
            bool isExists = this.context.SUBJECT_MASTER.Where(s => (Id == 0 || s.subject_id != Id) && s.subject.ToLower() == name.ToLower() && s.branch_id == branch && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<ResponseModel> SubjectMaintenance(SubjectEntity subjectInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            try
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
                    subjectMaster.row_sta_cd = (int)subjectInfo.RowStatus.RowStatus;
                    subjectMaster.subject_dtl_id = subjectInfo.BranchSubject.Subject_dtl_id == 0 ? (long?)null : subjectInfo.BranchSubject.Subject_dtl_id;
                    subjectMaster.trans_id = subjectInfo.Transaction.TransactionId;
                    this.context.SUBJECT_MASTER.Add(subjectMaster);
                    if (isUpdate)
                    {
                        this.context.Entry(subjectMaster).State = System.Data.Entity.EntityState.Modified;
                    }
                    //var da = this.context.SaveChanges() > 0 ? subjectMaster.subject_id : 0;
                    var da = this.context.SaveChanges() > 0 || subjectMaster.subject_id > 0;
                    if (da)
                    {
                        subjectInfo.SubjectID = subjectMaster.subject_id;
                        responseModel.Message = isUpdate == true ? "Subject Updated Successfully." : "Subject Inserted Successfully.";
                        responseModel.Status = true;
                        responseModel.Data = subjectInfo;
                    }
                    else
                    {
                        responseModel.Message = isUpdate == true ? "Subject Not Updated." : "Subject Not Inserted Successfully.";
                        responseModel.Status = false;
                    }
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "Subject Already Exist.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            return responseModel;
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
                        }).Distinct().OrderByDescending(a => a.SubjectID).ToList();

            return data;
        }

        public async Task<List<SubjectEntity>> GetAllSubjects()
        {
            var data = (from u in this.context.SUBJECT_MASTER orderby u.subject_id descending
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
            var data = (from u in this.context.SUBJECT_BRANCH_MASTER
                        orderby u.subject_id descending
                        where u.row_sta_cd == 1
                        select new SubjectEntity()
                        {
                            Subject = u.subject_name
                        }).Distinct().ToList();
            return data;
        }

        public async Task<List<SubjectEntity>> GetAllSubjectsID(string subjectName, long branchid)
        {
            var data = (from u in this.context.SUBJECT_DTL_MASTER
                        orderby u.subject_dtl_id descending
                        where u.row_sta_cd == 1 && u.SUBJECT_BRANCH_MASTER.subject_name == subjectName && (u.branch_id == branchid || branchid == 0)
                        select new SubjectEntity()
                        {
                            Subject = u.SUBJECT_BRANCH_MASTER.subject_name,
                            SubjectID = u.subject_dtl_id
                        }).ToList();

            return data;
        }

        public ResponseModel RemoveSubject(long SubjectID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var data = (from u in this.context.SUBJECT_MASTER
                            where u.subject_id == SubjectID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                    responseModel.Message = "Subject Removed Successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.Message = "Subject Not Found.";
                    responseModel.Status = false;
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            return responseModel;
            //return false;
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

        public async Task<List<SubjectEntity>> GetAllSubjectsByTestDate(string TestDate, long BranchID)
        {
            DateTime dateTime = Convert.ToDateTime(TestDate);
            var data = (from u in this.context.TEST_MASTER                     
                        orderby u.test_id descending
                        where (u.test_dt == dateTime && u.branch_id==BranchID && u.row_sta_cd == 1)
                        select new SubjectEntity()
                        {
                            testID = u.test_id,
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Subject = u.SUBJECT_DTL_MASTER.SUBJECT_BRANCH_MASTER.subject_name,
                            SubjectID = u.subject_dtl_id.HasValue? u.subject_dtl_id.Value:0,
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
