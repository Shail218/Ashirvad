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
                            FilePath = "https://mastermind.org.in" + u.file_path,
                            DocLink = u.doc_link,
                            Remarks = u.remarks,
                            TotalMarks = u.total_marks,
                            DocType = u.doc_type.HasValue ? u.doc_type.Value : false,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            return data;
        }
        public async Task<List<CompetitionEntity>> GetAllCompetitonData()
        {
            var data = (from u in this.context.COMPETITION_MASTER
                        orderby u.competition_id
                        select new CompetitionEntity()
                        {
                            CompetitionName = u.competition_name,
                            CompetitionID = u.competition_id

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
                            FilePath = "https://mastermind.org.in" + u.file_path,
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
                        where u.row_sta_cd == 1
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
                            FilePath = "https://mastermind.org.in" + u.file_path,
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
                            CompetitionFilepath = "https://mastermind.org.in" + u.competition_filepath,
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
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
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
                                LastName = u.STUDENT_MASTER.last_name,
                                BranchClass = new BranchClassEntity()
                                {
                                    BranchCourse = new BranchCourseEntity()
                                    {
                                        course = new CourseEntity()
                                        {
                                            CourseName = u.STUDENT_MASTER.CLASS_DTL_MASTER.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                        }
                                    },
                                    Class = new ClassEntity()
                                    {
                                        ClassName = u.STUDENT_MASTER.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                    }
                                }
                            },
                            SubmitDate = u.submit_dt,
                            competitionInfo = new CompetitionEntity()
                            {
                                CompetitionID = u.competition_id,
                                CompetitionDt = u.COMPETITION_MASTER.competition_dt,
                                CompetitionName = u.COMPETITION_MASTER.competition_name
                            }

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
                            CompetitionFilepath = "https://mastermind.org.in" + u.competition_filepath,
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

        public ResponseModel RemoveCompetitionAnswerSheetdetail(long competitionId, long studid)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var data = (from u in this.context.COMPETITION_MASTER_DTL
                            where u.competition_id == competitionId && u.stud_id == studid
                            select u).ToList();

                if (data != null)
                {
                    this.context.COMPETITION_MASTER_DTL.RemoveRange(data);
                    this.context.SaveChanges();
                    responseModel.Message = "Competition AnswerSheetDetails Removed Successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.Message = "Competition AnswerSheetDetails Not Found.";
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

        public ResponseModel UpdateCompetitionAnswerSheetRemarks(long competitionId, long studid, string remarks)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var data = (from u in this.context.COMPETITION_MASTER_DTL
                            where u.competition_id == competitionId && u.stud_id == studid && u.row_sta_cd == 1
                            select u).FirstOrDefault();
                if (data != null)
                {
                    data.remarks = remarks;
                    this.context.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    var da = this.context.SaveChanges();
                    if (da > 0)
                    {
                        responseModel.Message = "Remarks Updated Successfully.";
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = "Remarks Not Updated.";
                        responseModel.Status = false;
                    }
                }

            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            return responseModel;
        }

        #endregion

        #region Competition Rank Entry

        public async Task<CommonResponseModel<List<CompetitionAnswerSheetEntity>>> GetStudentListforCompetitionRankEntry(long competitionId)
        {
            CommonResponseModel<List<CompetitionAnswerSheetEntity>> responseModel = new CommonResponseModel<List<CompetitionAnswerSheetEntity>>();
            try
            {
                if (!CheckCompeitionRankEntry(competitionId))
                {
                    responseModel.Data = (from u in this.context.STUDENT_MASTER
                                .Include("BRANCH_MASTER")
                                          where u.row_sta_cd == 1
                                          orderby u.BRANCH_MASTER.branch_name ascending
                                          select new CompetitionAnswerSheetEntity()
                                          {
                                              RowStatus = new RowStatusEntity()
                                              {
                                                  RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                                  RowStatusId = (int)u.row_sta_cd
                                              },
                                              branchInfo = new BranchEntity()
                                              {
                                                  BranchID = u.BRANCH_MASTER.branch_id,
                                                  BranchName = u.BRANCH_MASTER.branch_name
                                              },
                                              studentInfo = new StudentEntity()
                                              {
                                                  StudentID = u.student_id,
                                                  FirstName = u.first_name,
                                                  LastName = u.last_name,
                                                  BranchClass = new BranchClassEntity()
                                                  {
                                                      BranchCourse = new BranchCourseEntity()
                                                      {
                                                          course = new CourseEntity()
                                                          {
                                                              CourseID = u.CLASS_DTL_MASTER.COURSE_DTL_MASTER.COURSE_MASTER.course_id,
                                                              CourseName = u.CLASS_DTL_MASTER.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                                          }
                                                      },
                                                      Class = new ClassEntity()
                                                      {
                                                          ClassID = u.CLASS_DTL_MASTER.CLASS_MASTER.class_id,
                                                          ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                                      }
                                                  }
                                              }
                                          }).Distinct().ToList();
                    if (responseModel.Data?.Count > 0)
                    {
                        var compe = (from com in this.context.COMPETITION_MASTER
                                     where com.competition_id == competitionId
                                     select new CompetitionEntity()
                                     {
                                         CompetitionID = com.competition_id,
                                         CompetitionName = com.competition_name
                                     }).FirstOrDefault();
                        foreach (var item in responseModel.Data)
                        {
                            item.competitionInfo = compe;
                        }
                        responseModel.Message = "Success";
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = "No Student Found.";
                        responseModel.Status = false;
                    }
                }
                else
                {
                    responseModel.Message = "Competition Rank Entry Already Done.";
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
        public bool CheckCompeitionRankEntry(long CompetitionId)
        {
            bool isExists = this.context.COMPETITION_RANK_MASTER.Where(s => s.competition_id == CompetitionId && s.row_sta_cd == 1).FirstOrDefault() != null;
            return isExists;
        }
        public async Task<ResponseModel> CompetitionRankMaintenance(CompetitionRankEntity rankEntity)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                bool isUpdate = true;
                Model.COMPETITION_RANK_MASTER compRankEntity = new Model.COMPETITION_RANK_MASTER();
                var data = (from cl in this.context.COMPETITION_RANK_MASTER
                            where cl.competition_rank_id == rankEntity.CompetitionRankId
                            select cl).FirstOrDefault();
                if (data == null)
                {
                    compRankEntity = new Model.COMPETITION_RANK_MASTER();
                    isUpdate = false;
                }
                else
                {
                    compRankEntity = data;
                    rankEntity.Transaction.TransactionId = compRankEntity.trans_id;
                }
                compRankEntity.student_id = rankEntity.studentInfo.StudentID;
                compRankEntity.branch_id = rankEntity.branchInfo.BranchID;
                compRankEntity.competition_rank = rankEntity.competitionRank;
                compRankEntity.rank_dt = rankEntity.RankDate;
                compRankEntity.row_sta_cd = rankEntity.RowStatus.RowStatusId;
                compRankEntity.competition_id = rankEntity.competitionInfo.CompetitionID;
                compRankEntity.trans_id = this.AddTransactionData(rankEntity.Transaction);
                this.context.COMPETITION_RANK_MASTER.Add(compRankEntity);
                if (isUpdate)
                {
                    this.context.Entry(compRankEntity).State = System.Data.Entity.EntityState.Modified;
                }
                var Result = this.context.SaveChanges();
                if (Result > 0)
                {
                    responseModel.Status = true;
                    responseModel.Message = isUpdate == true ? "Rank Updated Successfully." : "Rank Inserted Successfully.";
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = isUpdate == true ? "Rank Not Updated." : "Rank Not Inserted.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                if (ex.InnerException != null)
                {
                    responseModel.Message = ex.InnerException.Message.ToString();
                }
                responseModel.Status = false;
            }

            return responseModel;
        }
        public async Task<ResponseModel> UpdateRankDetail(long CompetitionId, long CompetitionRankId, string Remarks)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var data = (from cl in this.context.COMPETITION_RANK_MASTER
                            where cl.competition_rank_id == CompetitionRankId && cl.competition_id == CompetitionId && cl.row_sta_cd == 1
                            select cl).FirstOrDefault();
                if (data != null)
                {
                    data.competition_rank = Remarks;
                    this.context.COMPETITION_RANK_MASTER.Add(data);
                    this.context.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    this.context.SaveChanges();
                    model.Status = true;
                    model.Message = "Rank Updated Successfully.";
                }
                else
                {
                    model.Status = false;
                    model.Message = "Data Not Found.";
                }
            }
            catch (Exception ex)
            {
                model.Message = ex.Message.ToString();
                model.Status = false;
            }
            return model;
        }
        public async Task<CommonResponseModel<List<CompetitionRankEntity>>> GetCompetitionRankListbyCompetitionId(long CompetitionId)
        {
            CommonResponseModel<List<CompetitionRankEntity>> responseModel = new CommonResponseModel<List<CompetitionRankEntity>>();
            try
            {
                responseModel.Data = (from u in this.context.COMPETITION_RANK_MASTER
                               .Include("BRANCH_MASTER")
                                      orderby u.BRANCH_MASTER.branch_name ascending
                                      select new CompetitionRankEntity()
                                      {
                                          RowStatus = new RowStatusEntity()
                                          {
                                              RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                              RowStatusId = (int)u.row_sta_cd
                                          },
                                          branchInfo = new BranchEntity()
                                          {
                                              BranchID = u.BRANCH_MASTER.branch_id,
                                              BranchName = u.BRANCH_MASTER.branch_name
                                          },
                                          studentInfo = new StudentEntity()
                                          {
                                              StudentID = u.student_id,
                                              FirstName = u.STUDENT_MASTER.first_name,
                                              LastName = u.STUDENT_MASTER.last_name,
                                              BranchClass = new BranchClassEntity()
                                              {
                                                  BranchCourse = new BranchCourseEntity()
                                                  {
                                                      course = new CourseEntity()
                                                      {
                                                          CourseID = u.STUDENT_MASTER.COURSE_DTL_MASTER.COURSE_MASTER.course_id,
                                                          CourseName = u.STUDENT_MASTER.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                                      }
                                                  },
                                                  Class = new ClassEntity()
                                                  {
                                                      ClassID = u.STUDENT_MASTER.CLASS_DTL_MASTER.CLASS_MASTER.class_id,
                                                      ClassName = u.STUDENT_MASTER.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                                  }
                                              }
                                          },
                                          competitionInfo = new CompetitionEntity()
                                          {
                                              CompetitionID = u.competition_id,
                                              CompetitionName = u.COMPETITION_MASTER.competition_name
                                          },
                                          CompetitionRankId = u.competition_rank_id,
                                          competitionRank = u.competition_rank
                                      }).ToList();
                if (responseModel.Data?.Count > 0)
                {
                    responseModel.Status = true;
                    responseModel.Message = "Success";
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "No Data Found.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            return responseModel;
        }
        public async Task<CompetitionRankEntity> GetStudentRank(long CompetitionId,long StudentID)
        {
            var data = (from u in this.context.COMPETITION_RANK_MASTER
                               .Include("BRANCH_MASTER")
                        where u.row_sta_cd == 1 && u.competition_id == CompetitionId
                        && u.student_id == StudentID
                        orderby u.BRANCH_MASTER.branch_name ascending
                        select new CompetitionRankEntity()
                        {
                            studentInfo = new StudentEntity()
                            {
                                StudentID = u.student_id,
                                FirstName = u.STUDENT_MASTER.first_name,
                                LastName = u.STUDENT_MASTER.last_name,
                                BranchClass = new BranchClassEntity()
                                {
                                    BranchCourse = new BranchCourseEntity()
                                    {
                                        course = new CourseEntity()
                                        {
                                            CourseID = u.STUDENT_MASTER.COURSE_DTL_MASTER.COURSE_MASTER.course_id,
                                            CourseName = u.STUDENT_MASTER.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                        }
                                    },
                                    Class = new ClassEntity()
                                    {
                                        ClassID = u.STUDENT_MASTER.CLASS_DTL_MASTER.CLASS_MASTER.class_id,
                                        ClassName = u.STUDENT_MASTER.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                    }
                                }
                            },
                            competitionInfo = new CompetitionEntity()
                            {
                                CompetitionID = u.competition_id,
                                CompetitionName = u.COMPETITION_MASTER.competition_name
                            },
                            CompetitionRankId = u.competition_rank_id,
                            competitionRank = u.competition_rank
                        }).FirstOrDefault();
            return data;
        }
        public async Task<CommonResponseModel<List<CompetitionRankEntity>>> GetCompetitionRankDistinctList()
        {
            CommonResponseModel<List<CompetitionRankEntity>> responseModel = new CommonResponseModel<List<CompetitionRankEntity>>();
            try
            {
                responseModel.Data = (from u in this.context.COMPETITION_RANK_MASTER
                               .Include("BRANCH_MASTER")
                                      orderby u.BRANCH_MASTER.branch_name ascending
                                      select new CompetitionRankEntity()
                                      {
                                          RowStatus = new RowStatusEntity()
                                          {
                                              RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                              RowStatusId = (int)u.row_sta_cd
                                          },
                                          competitionInfo = new CompetitionEntity()
                                          {
                                              CompetitionID = u.competition_id,
                                              CompetitionName = u.COMPETITION_MASTER.competition_name,
                                              CompetitionDt = u.COMPETITION_MASTER.competition_dt
                                          }
                                      }).Distinct().ToList();
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<List<CompetitionRankEntity>> GetCompetitionRankListByCompetitionIdandBranchID(long competitionId, long branchId)
        {
            List<CompetitionRankEntity> responseModel = new List<CompetitionRankEntity>();
            try
            {
                responseModel = (from u in this.context.COMPETITION_RANK_MASTER
                               .Include("BRANCH_MASTER")
                               where u.branch_id == branchId && u.competition_id==competitionId && u.row_sta_cd ==1
                                      select new CompetitionRankEntity()
                                      {
                                          branchInfo = new BranchEntity()
                                          {
                                              BranchID = u.BRANCH_MASTER.branch_id,
                                              BranchName = u.BRANCH_MASTER.branch_name
                                          },
                                          studentInfo = new StudentEntity()
                                          {
                                              StudentID = u.student_id,
                                              Name = u.STUDENT_MASTER.first_name + " "+u.STUDENT_MASTER.last_name,
                                              BranchClass = new BranchClassEntity()
                                              {
                                                  BranchCourse = new BranchCourseEntity()
                                                  {
                                                      course = new CourseEntity()
                                                      {
                                                          CourseID = u.STUDENT_MASTER.COURSE_DTL_MASTER.COURSE_MASTER.course_id,
                                                          CourseName = u.STUDENT_MASTER.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                                      }
                                                  },
                                                  Class = new ClassEntity()
                                                  {
                                                      ClassID = u.STUDENT_MASTER.CLASS_DTL_MASTER.CLASS_MASTER.class_id,
                                                      ClassName = u.STUDENT_MASTER.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                                  }
                                              }
                                          },
                                          CompetitionRankId = u.competition_rank_id,
                                          competitionRank = u.competition_rank
                                      }).ToList();
                
            }
            catch (Exception ex)
            {
            }
            return responseModel;
        }
        #endregion

        #region Competition Winner Entry

        public async Task<ResponseModel> CompetitionWinnerMaintenance(CompetitionWinnerEntity winnerEntity)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                bool isUpdate = true;
                Model.COMPETITION_WINNER_MASTER compwinnerEntity = new Model.COMPETITION_WINNER_MASTER();
                if(CheckCompetitionWinnerEntry(winnerEntity.competitionInfo.CompetitionID,winnerEntity.competitionRankInfo.CompetitionRankId,
                    winnerEntity.competition_winner_id).Result != -1)
                {
                    var data = (from cl in this.context.COMPETITION_WINNER_MASTER
                                where cl.competition_winner_id == winnerEntity.competition_winner_id
                                select cl).FirstOrDefault();
                    if (data == null)
                    {
                        compwinnerEntity = new Model.COMPETITION_WINNER_MASTER();
                        isUpdate = false;
                    }
                    else
                    {
                        compwinnerEntity = data;
                        winnerEntity.Transaction.TransactionId = compwinnerEntity.trans_id;
                    }
                    compwinnerEntity.competition_rank_id = winnerEntity.competitionRankInfo.CompetitionRankId;
                    compwinnerEntity.prize_name = winnerEntity.prizeName;
                    compwinnerEntity.row_sta_id = winnerEntity.RowStatus.RowStatusId;
                    compwinnerEntity.competition_id = winnerEntity.competitionInfo.CompetitionID;
                    compwinnerEntity.trans_id = this.AddTransactionData(winnerEntity.Transaction);
                    this.context.COMPETITION_WINNER_MASTER.Add(compwinnerEntity);
                    if (isUpdate)
                    {
                        this.context.Entry(compwinnerEntity).State = System.Data.Entity.EntityState.Modified;
                    }
                    var Result = this.context.SaveChanges();
                    if (Result > 0)
                    {
                        model.Status = true;
                        model.Message = isUpdate == true ? "Competition Winner Updated Successfully." : "Competition Winner Inserted Successfully.";
                    }
                    else
                    {
                        model.Status = false;
                        model.Message = isUpdate == true ? "Competition Winner Not Updated." : "Competition Winner Not Inserted.";
                    }
                }
                else
                {
                    model.Status = false;
                    model.Message = "Competiton Winner Already Exist.";
                }
            }
            catch (Exception ex)
            {
                model.Status = false;
                model.Message = ex.Message;
                if (ex.InnerException != null)
                {
                    model.Message = ex.InnerException.Message;
                }
            }
            return model;
        }
        public async Task<long> CheckCompetitionWinnerEntry(long CompetitionId, long CompetitionRankId,long competetionwinnerID)
        {
            long result;
            bool isExists = this.context.COMPETITION_WINNER_MASTER.Where(s => s.competition_id == CompetitionId && s.competition_rank_id == CompetitionRankId && s.row_sta_id == 1 &&
            (competetionwinnerID == 0 || s.competition_winner_id == competetionwinnerID)).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;      
        }
        public async Task<CommonResponseModel<List<CompetitionWinnerEntity>>> GetCompetitionWinnerListbyCompetitionId()
        {
            CommonResponseModel<List<CompetitionWinnerEntity>> responseModel = new CommonResponseModel<List<CompetitionWinnerEntity>>();
            try
            {
                responseModel.Data = (from u in this.context.COMPETITION_WINNER_MASTER
                               .Include("BRANCH_MASTER") where u.row_sta_id == 1
                                      orderby u.competition_id ascending
                                      select new CompetitionWinnerEntity()
                                      {
                                          prizeName = u.prize_name,
                                          competition_winner_id = u.competition_winner_id,
                                          RowStatus = new RowStatusEntity()
                                          {
                                              RowStatus = u.row_sta_id == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                              RowStatusId = (int)u.row_sta_id
                                          },
                                          competitionRankInfo = new CompetitionRankEntity()
                                          {
                                              branchInfo = new BranchEntity()
                                              {
                                                  BranchID = u.COMPETITION_RANK_MASTER.BRANCH_MASTER.branch_id,
                                                  BranchName = u.COMPETITION_RANK_MASTER.BRANCH_MASTER.branch_name
                                              },
                                              studentInfo = new StudentEntity()
                                              {
                                                  StudentID = u.COMPETITION_RANK_MASTER.student_id,
                                                  FirstName = u.COMPETITION_RANK_MASTER.STUDENT_MASTER.first_name,
                                                  LastName = u.COMPETITION_RANK_MASTER.STUDENT_MASTER.last_name,
                                                  BranchClass = new BranchClassEntity()
                                                  {
                                                      BranchCourse = new BranchCourseEntity()
                                                      {
                                                          course = new CourseEntity()
                                                          {
                                                              CourseID = u.COMPETITION_RANK_MASTER.STUDENT_MASTER.COURSE_DTL_MASTER.COURSE_MASTER.course_id,
                                                              CourseName = u.COMPETITION_RANK_MASTER.STUDENT_MASTER.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                                          }
                                                      },
                                                      Class = new ClassEntity()
                                                      {
                                                          ClassID = u.COMPETITION_RANK_MASTER.STUDENT_MASTER.CLASS_DTL_MASTER.CLASS_MASTER.class_id,
                                                          ClassName = u.COMPETITION_RANK_MASTER.STUDENT_MASTER.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                                      }
                                                  }
                                              },
                                              CompetitionRankId = u.competition_rank_id,
                                              competitionRank = u.COMPETITION_RANK_MASTER.competition_rank
                                          },
                                          competitionInfo = new CompetitionEntity()
                                          {
                                              CompetitionID = u.competition_id,
                                              CompetitionName = u.COMPETITION_MASTER.competition_name
                                          }
                                      }).ToList();
                if (responseModel.Data?.Count > 0)
                {
                    responseModel.Status = true;
                    responseModel.Message = "Success";
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "No Data Found.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            return responseModel;
        }
        public async Task<CommonResponseModel<CompetitionWinnerEntity>> GetCompetitionWinnerDetailbyId(long competitionWinnerId)
        {
            CommonResponseModel<CompetitionWinnerEntity> responseModel = new CommonResponseModel<CompetitionWinnerEntity>();
            try
            {
                responseModel.Data = (from u in this.context.COMPETITION_WINNER_MASTER
                               .Include("BRANCH_MASTER") 
                               where u.competition_winner_id== competitionWinnerId && u.row_sta_id ==(int)Enums.RowStatus.Active
                                      select new CompetitionWinnerEntity()
                                      {
                                          prizeName = u.prize_name,
                                          competition_winner_id = u.competition_winner_id,
                                          RowStatus = new RowStatusEntity()
                                          {
                                              RowStatus = u.row_sta_id == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                              RowStatusId = (int)u.row_sta_id
                                          },
                                          competitionRankInfo = new CompetitionRankEntity()
                                          {
                                              branchInfo = new BranchEntity()
                                              {
                                                  BranchID = u.COMPETITION_RANK_MASTER.BRANCH_MASTER.branch_id,
                                                  BranchName = u.COMPETITION_RANK_MASTER.BRANCH_MASTER.branch_name
                                              },
                                              studentInfo = new StudentEntity()
                                              {
                                                  StudentID = u.COMPETITION_RANK_MASTER.student_id,
                                                  FirstName = u.COMPETITION_RANK_MASTER.STUDENT_MASTER.first_name,
                                                  LastName = u.COMPETITION_RANK_MASTER.STUDENT_MASTER.last_name,
                                                  BranchClass = new BranchClassEntity()
                                                  {
                                                      BranchCourse = new BranchCourseEntity()
                                                      {
                                                          course = new CourseEntity()
                                                          {
                                                              CourseID = u.COMPETITION_RANK_MASTER.STUDENT_MASTER.COURSE_DTL_MASTER.COURSE_MASTER.course_id,
                                                              CourseName = u.COMPETITION_RANK_MASTER.STUDENT_MASTER.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                                          }
                                                      },
                                                      Class = new ClassEntity()
                                                      {
                                                          ClassID = u.COMPETITION_RANK_MASTER.STUDENT_MASTER.CLASS_DTL_MASTER.CLASS_MASTER.class_id,
                                                          ClassName = u.COMPETITION_RANK_MASTER.STUDENT_MASTER.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                                      }
                                                  }
                                              },
                                              CompetitionRankId = u.competition_rank_id,
                                              competitionRank = u.COMPETITION_RANK_MASTER.competition_rank
                                          },
                                          competitionInfo = new CompetitionEntity()
                                          {
                                              CompetitionID = u.competition_id,
                                              CompetitionName = u.COMPETITION_MASTER.competition_name
                                          }
                                      }).FirstOrDefault();
                if (responseModel.Data!=null)
                {
                    responseModel.Status = true;
                    responseModel.Message = "Success";
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "No Data Found.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
            return responseModel;
        }
        public async Task<ResponseModel> DeleteCompetitionWinner(long competitionWinnerId, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var data = (from u in this.context.COMPETITION_WINNER_MASTER
                            where u.competition_winner_id == competitionWinnerId
                            select u).FirstOrDefault();
                if (data != null)
                {
                    data.row_sta_id = (int)Enums.RowStatus.Inactive;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                    responseModel.Status = true;
                    responseModel.Message = "Competition Winner Removed Successfully.";
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "Competition Winner Not Found.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
            return responseModel;
        }
        #endregion
    }
}
