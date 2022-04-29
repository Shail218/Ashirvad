using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Repo.DataAcceessAPI.Area.Competition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area.Competiton
{
    public class Competition : ModelAccess, ICompetitonAPI
    {
        #region Competition Entry
        
        public async Task<long> CheckCompetitonExist(string CompetitionName, long CompetitionID)
        {
            long result;
            bool isExists = this.context.COMPETITION_MASTER.Where(s => (CompetitionID == 0 || s.competition_id != CompetitionID) && s.competition_name.ToLower() == CompetitionName.ToLower() && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }
        public async Task<ResponseModel> CompetitionMaintenance(CompetitionEntity CompetitonInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                Model.COMPETITION_MASTER competitonmaster = new Model.COMPETITION_MASTER();
                if (CheckCompetitonExist(CompetitonInfo.CompetitionName, CompetitonInfo.CompetitionID).Result != -1)
                {
                    bool isUpdate = true;
                    var data = (from t in this.context.COMPETITION_MASTER
                                where t.competition_id == CompetitonInfo.CompetitionID
                                select t).FirstOrDefault();
                    if (data == null)
                    {
                        data = new Model.COMPETITION_MASTER();
                        isUpdate = false;
                    }
                    else
                    {
                        competitonmaster = data;
                        CompetitonInfo.Transaction.TransactionId = data.trans_id;
                    }
                    competitonmaster.competition_name = CompetitonInfo.CompetitionName;
                    competitonmaster.competition_dt = CompetitonInfo.CompetitionDt;
                    competitonmaster.total_marks = CompetitonInfo.TotalMarks;
                    competitonmaster.competition_st_time = CompetitonInfo.CompetitionStartTime;
                    competitonmaster.competition_end_time = CompetitonInfo.CompetitionEndTime;
                    competitonmaster.trans_id = this.AddTransactionData(CompetitonInfo.Transaction);
                    competitonmaster.row_sta_cd = CompetitonInfo.RowStatus.RowStatusId;
                    competitonmaster.remarks = CompetitonInfo.Remarks;
                    competitonmaster.file_name = CompetitonInfo.FileName;
                    competitonmaster.file_path = CompetitonInfo.FilePath;
                    competitonmaster.doc_link = CompetitonInfo.DocLink;
                    competitonmaster.doc_type = CompetitonInfo.DocType;
                    this.context.COMPETITION_MASTER.Add(competitonmaster);
                    if (isUpdate)
                    {
                        this.context.Entry(competitonmaster).State = System.Data.Entity.EntityState.Modified;
                    }
                    var da = this.context.SaveChanges() > 0 || competitonmaster.competition_id > 0;
                    if (da)
                    {
                        CompetitonInfo.CompetitionID = competitonmaster.competition_id;
                        responseModel.Message = isUpdate == true ? "Competiton Updated." : "Competiton Inserted Successfully";
                        responseModel.Status = true;
                        responseModel.Data = CompetitonInfo;
                    }
                    else
                    {
                        responseModel.Message = isUpdate == true ? "Competiton Not Updated." : "Competiton Not Inserted Successfully";
                        responseModel.Status = false;
                    }
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "Competiton Already Exist.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            return responseModel;
        }
        public async Task<ResponseModel> DeleteCompetition(long CompetitionID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var data = (from u in this.context.COMPETITION_MASTER
                            where u.competition_id == CompetitionID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                    responseModel.Status = true;
                    responseModel.Message = "Competition Removed Successfully.";
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "Competition Not Found.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
            return responseModel;
        }
        public async Task<List<CompetitionEntity>> GetAllCompetiton()
        {
            var data = (from u in this.context.COMPETITION_MASTER
                        orderby u.competition_id descending
                        where u.row_sta_cd == 1
                        select new CompetitionEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            CompetitionName = u.competition_name,
                            CompetitionStartTime = u.competition_st_time,
                            CompetitionEndTime = u.competition_end_time,
                            CompetitionID = u.competition_id,
                            CompetitionDt = u.competition_dt,
                            FileName = u.file_name,
                            FilePath = u.file_path,
                            DocLink = u.doc_link,
                            Remarks = u.remarks,
                            TotalMarks = u.total_marks,
                            DocType = u.doc_type.HasValue ? u.doc_type.Value : false,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            return data;
        }
        public async Task<CompetitionEntity> GetCompetitionByID(long CompetitonID)
        {
            var data = (from u in this.context.COMPETITION_MASTER
                        where u.competition_id == CompetitonID
                        select new CompetitionEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            CompetitionName = u.competition_name,
                            CompetitionStartTime = u.competition_st_time,
                            CompetitionEndTime = u.competition_end_time,
                            CompetitionID = u.competition_id,
                            CompetitionDt = u.competition_dt,
                            TotalMarks = u.total_marks,
                            Remarks = u.remarks,
                            FileName = u.file_name,
                            FilePath = u.file_path,
                            DocLink = u.doc_link,
                            DocType = u.doc_type.HasValue ? u.doc_type.Value : false,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();
            return data;
        }

        public async Task<List<CompetitionEntity>> GetAllCustomCompetition(DataTableAjaxPostModel model)
        {
            var Result = new List<CompetitionEntity>();
            bool Isasc = model.order[0].dir == "desc" ? false : true;
           
            long count = (from u in this.context.COMPETITION_MASTER
                          orderby u.competition_id descending
                          where u.row_sta_cd == 1
                          select new CompetitionEntity()
                          {
                              CompetitionID = u.competition_id
                          }).Distinct().Count();
            var data = (from u in this.context.COMPETITION_MASTER
                        orderby u.competition_id descending
                        where u.row_sta_cd ==1
                      && (model.search.value == null
                        || model.search.value == ""
                        || u.competition_name.ToString().ToLower().Contains(model.search.value)
                        || u.competition_st_time.ToString().ToLower().Contains(model.search.value)
                        || u.competition_end_time.ToString().ToLower().Contains(model.search.value)
                        || u.competition_dt.ToString().ToLower().Contains(model.search.value))
                        orderby u.competition_id descending
                        select new CompetitionEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            CompetitionName = u.competition_name,
                            CompetitionStartTime = u.competition_st_time,
                            CompetitionEndTime = u.competition_end_time,
                            CompetitionID = u.competition_id,
                            CompetitionDt = u.competition_dt,
                            FileName = u.file_name,
                            FilePath = u.file_path,
                            DocLink = u.doc_link,
                            Remarks = u.remarks,
                            TotalMarks = u.total_marks,
                            DocType = u.doc_type.HasValue ? u.doc_type.Value : false,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        })
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();
            return data;
        }

        #endregion

        #region Competition Answer Sheet

        public async Task<ResponseModel> CompetitionSheetMaintenance(CompetitionAnswerSheetEntity competitionAnswerSheet)
        {
            ResponseModel responseModel = new ResponseModel();
            Model.COMPETITION_MASTER_DTL ansSheet = new Model.COMPETITION_MASTER_DTL();
            bool isUpdate = true;
            try
            {
                var data = (from t in this.context.COMPETITION_MASTER_DTL
                            where t.competition_id == competitionAnswerSheet.competitionInfo.CompetitionID && t.stud_id == competitionAnswerSheet.studentInfo.StudentID
                            select t).FirstOrDefault();
                if (data == null)
                {
                    data = new Model.COMPETITION_MASTER_DTL();
                    isUpdate = false;
                }
                else
                {
                    ansSheet = new Model.COMPETITION_MASTER_DTL();
                    isUpdate = false;

                }

                ansSheet.row_sta_cd = competitionAnswerSheet.RowStatus.RowStatusId;
                ansSheet.trans_id = this.AddTransactionData(competitionAnswerSheet.Transaction);
                ansSheet.competition_id = competitionAnswerSheet.competitionInfo.CompetitionID;
                ansSheet.competition_sheet_content = null;
                ansSheet.competition_sheet_name = competitionAnswerSheet.CompetitionSheetName;
                ansSheet.branch_id = competitionAnswerSheet.branchInfo.BranchID;
                ansSheet.remarks = competitionAnswerSheet.Remarks;
                ansSheet.status = competitionAnswerSheet.Status;
                ansSheet.stud_id = competitionAnswerSheet.studentInfo.StudentID;
                ansSheet.submit_dt = competitionAnswerSheet.SubmitDate;
                ansSheet.competition_filepath = competitionAnswerSheet.CompetitionFilepath;
                this.context.COMPETITION_MASTER_DTL.Add(ansSheet);
                if (isUpdate)
                {
                    this.context.Entry(ansSheet).State = System.Data.Entity.EntityState.Modified;
                }

                var da = this.context.SaveChanges() > 0 ? ansSheet.competition_dtl_id : 0;
                if (da > 0)
                {
                    competitionAnswerSheet.CompetitionDtlId = da;
                    responseModel.Data = competitionAnswerSheet;
                    responseModel.Message = isUpdate == true ? "AnswerSheet Updated Successfully." : "AnswerSheet Inserted Successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.Message = isUpdate == true ? "AnswerSheet Not Updated." : "AnswerSheet Not Inserted.";
                    responseModel.Status = false;
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<List<CompetitionAnswerSheetEntity>> GetAllAnswerSheetByCompetitionId(long competitionId)
        {
            var data = (from u in this.context.COMPETITION_MASTER_DTL
                        .Include("COMPETITION_MASTER")
                        .Include("STUDENT_MASTER")
                        .Include("BRANCH_MASTER")
                        where u.competition_id == competitionId
                        select new CompetitionAnswerSheetEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            CompetitionFilepath = u.competition_filepath,
                            CompetitionDtlId = u.competition_dtl_id,
                            CompetitionSheetName = u.competition_sheet_name,
                            branchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Remarks = u.remarks,
                            Status = u.status,
                            StatusText = u.status == 1 ? "Pending" : "Done",
                            studentInfo = new StudentEntity()
                            {
                                StudentID = u.STUDENT_MASTER.student_id,
                                FirstName = u.STUDENT_MASTER.first_name,
                                LastName = u.STUDENT_MASTER.last_name
                            },
                            SubmitDate = u.submit_dt,
                            competitionInfo = new CompetitionEntity()
                            {
                                CompetitionID = u.competition_id,
                                CompetitionDt = u.COMPETITION_MASTER.competition_dt,
                                CompetitionName = u.COMPETITION_MASTER.competition_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            //if (data?.Count > 0)
            //{
            //    foreach (var item in data)
            //    {
            //        int idx = data.IndexOf(item);
            //        data[idx].AnswerSheetContentText = Convert.ToBase64String(data[idx].AnswerSheetContent);
            //    }
            //}
            return data;
        }

        public async Task<List<CompetitionAnswerSheetEntity>> GetAllDistinctAnswerSheetDatabyCompetitionId(long competitionId)
        {
            var data = (from u in this.context.COMPETITION_MASTER_DTL
                        .Include("COMPETITION_MASTER")
                        .Include("STUDENT_MASTER")
                        .Include("BRANCH_MASTER")
                        where u.competition_id == competitionId
                        select new CompetitionAnswerSheetEntity()
                        {

                            CompetitionFilepath = u.competition_filepath,
                            Remarks = u.remarks,
                            Status = u.status,
                            StatusText = u.status == 1 ? "Pending" : "Done",
                            studentInfo = new StudentEntity()
                            {
                                StudentID = u.STUDENT_MASTER.student_id,
                                FirstName = u.STUDENT_MASTER.first_name,
                                LastName = u.STUDENT_MASTER.last_name,
                                Name = u.STUDENT_MASTER.first_name + " " + u.STUDENT_MASTER.last_name
                            },

                            SubmitDate = u.submit_dt,
                            competitionInfo = new CompetitionEntity()
                            {
                                CompetitionID = u.competition_id,
                                CompetitionDt = u.COMPETITION_MASTER.competition_dt,
                                CompetitionName = u.COMPETITION_MASTER.competition_name,
                            },

                        }).Distinct().ToList();
           
            return data;
        }

        public async Task<List<CompetitionAnswerSheetEntity>> GetStudentAnswerSheetbyCompetitionID(long competitionId, long studentID)
        {
            var data = (from u in this.context.COMPETITION_MASTER_DTL
                       .Include("COMPETITION_MASTER")
                       .Include("STUDENT_MASTER")
                       .Include("BRANCH_MASTER")
                        where u.competition_id == competitionId && u.stud_id == studentID
                        select new CompetitionAnswerSheetEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            CompetitionFilepath = u.competition_filepath,
                            CompetitionDtlId = u.competition_dtl_id,
                            CompetitionSheetName = u.competition_sheet_name,
                            branchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Remarks = u.remarks,
                            Status = u.status,
                            StatusText = u.status == 1 ? "Pending" : "Done",
                            studentInfo = new StudentEntity()
                            {
                                StudentID = u.STUDENT_MASTER.student_id,
                                FirstName = u.STUDENT_MASTER.first_name,
                                LastName = u.STUDENT_MASTER.last_name
                            },
                            SubmitDate = u.submit_dt,
                            competitionInfo = new CompetitionEntity()
                            {
                                CompetitionID = u.competition_id,
                                CompetitionDt = u.COMPETITION_MASTER.competition_dt,
                                CompetitionName = u.COMPETITION_MASTER.competition_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            return data;
        }

     
        #endregion
    }
}
