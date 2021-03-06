using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Homework
{
    public class HomeworkDetails : ModelAccess, IHomeworkDetailsAPI
    {

        public async Task<ResponseModel> HomeworkMaintenance(HomeworkDetailEntity homeworkDetail)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                Model.HOMEWORK_MASTER_DTL homework = new Model.HOMEWORK_MASTER_DTL();
                bool isUpdate = true;
                var data = (from t in this.context.HOMEWORK_MASTER_DTL
                            where t.homework_id == homeworkDetail.HomeworkEntity.HomeworkID && t.stud_id == homeworkDetail.StudentInfo.StudentID
                            select t).FirstOrDefault();
                if (data == null)
                {
                    data = new Model.HOMEWORK_MASTER_DTL();
                    isUpdate = false;
                }
                else
                {
                    //bool result = RemoveHomeworkdetail(homeworkDetail.HomeworkEntity.HomeworkID, homeworkDetail.StudentInfo.StudentID);
                    data = new Model.HOMEWORK_MASTER_DTL();
                    isUpdate = false;
                }

                homework.row_sta_cd = homeworkDetail.RowStatus.RowStatusId;
                homework.trans_id = this.AddTransactionData(homeworkDetail.Transaction);
                homework.homework_id = homeworkDetail.HomeworkEntity.HomeworkID;
                if (homework.homework_sheet_content?.Length > 0)
                {
                    homework.homework_sheet_content = homeworkDetail.AnswerSheetContent;

                }
                homework.homework_sheet_name = homeworkDetail.AnswerSheetName;
                homework.homework_filepath = homeworkDetail.FilePath;
                homework.branch_id = homeworkDetail.BranchInfo.BranchID;
                homework.remarks = homeworkDetail.Remarks;
                homework.status = homeworkDetail.Status;
                homework.stud_id = homeworkDetail.StudentInfo.StudentID;
                homework.submit_dt = homeworkDetail.SubmitDate;
                this.context.HOMEWORK_MASTER_DTL.Add(homework);
                if (isUpdate)
                {
                    this.context.Entry(homework).State = System.Data.Entity.EntityState.Modified;
                }

                var res = this.context.SaveChanges() > 0 ? homework.homework_master_dtl_id : 0;
                if (res > 0)
                {
                    homeworkDetail.HomeworkDetailID = homework.homework_master_dtl_id;
                    responseModel.Data = homeworkDetail;
                    responseModel.Status = true;
                    responseModel.Message = isUpdate == true ? "Homework Details Updated Successfully." : "Homework Details Inserted Successfully.";
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = isUpdate==true? "Homework Details Not Updated." : "Homework Details Not Inserted.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
            return responseModel;

        }

        public async Task<List<HomeworkDetailEntity>> GetAllHomeworkByStudent(long homeworkID)
        {
            var data = (from u in this.context.HOMEWORK_MASTER_DTL
                       .Include("HOMEWORK_MASTER")
                       .Include("STUDENT_MASTER")
                       .Include("BRANCH_MASTER")
                        orderby u.homework_master_dtl_id descending
                        where u.homework_id == homeworkID
                        select new HomeworkDetailEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            AnswerSheetContent = u.homework_sheet_content,
                            HomeworkDetailID = u.homework_master_dtl_id,
                            AnswerSheetName = u.homework_sheet_name,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Remarks = u.remarks,
                            Status = u.status,
                            StatusText = u.status == 1 ? "Pending" : "Done",
                            StudentInfo = new StudentEntity()
                            {
                                StudentID = u.STUDENT_MASTER.student_id,
                                FirstName = u.STUDENT_MASTER.first_name,
                                LastName = u.STUDENT_MASTER.last_name
                            },
                            SubmitDate = u.submit_dt,
                            HomeworkEntity = new HomeworkEntity()
                            {
                                HomeworkID = u.homework_id,
                                HomeworkDate = u.HOMEWORK_MASTER.homework_dt,
                                Remarks = u.HOMEWORK_MASTER.remarks
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            return data;
        }

        public async Task<List<HomeworkDetailEntity>> GetAllHomeworkdetailWithoutContentByStudentID(long homeworkID)
        {
            var data = (from u in this.context.HOMEWORK_MASTER_DTL
                       .Include("TEST_MASTER")
                       .Include("STUDENT_MASTER")
                       .Include("BRANCH_MASTER")
                        orderby u.homework_master_dtl_id descending
                        where u.homework_id == homeworkID
                        select new HomeworkDetailEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            HomeworkDetailID = u.homework_master_dtl_id,
                            AnswerSheetName = u.homework_sheet_name,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Remarks = u.remarks,
                            Status = u.status,
                            StatusText = u.status == 1 ? "Pending" : "Done",
                            StudentInfo = new StudentEntity()
                            {
                                StudentID = u.STUDENT_MASTER.student_id,
                                FirstName = u.STUDENT_MASTER.first_name,
                                LastName = u.STUDENT_MASTER.last_name
                            },
                            SubmitDate = u.submit_dt,
                            HomeworkEntity = new HomeworkEntity()
                            {
                                HomeworkID = u.homework_id,
                                HomeworkDate = u.HOMEWORK_MASTER.homework_dt,
                                Remarks = u.HOMEWORK_MASTER.remarks
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            return data;
        }

        public async Task<List<HomeworkDetailEntity>> GetAllAnsSheetByStudentID(long homeworkID, long studentID)
        {
            var data = (from u in this.context.HOMEWORK_MASTER_DTL
                        .Include("TEST_MASTER")
                        .Include("STUDENT_MASTER")
                        .Include("BRANCH_MASTER")
                        orderby u.homework_master_dtl_id descending
                        where u.homework_id == homeworkID && u.stud_id == studentID
                        select new HomeworkDetailEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            AnswerSheetContent = u.homework_sheet_content,
                            HomeworkDetailID = u.homework_master_dtl_id,
                            AnswerSheetName = u.homework_sheet_name,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Remarks = u.remarks,
                            Status = u.status,
                            StatusText = u.status == 1 ? "Pending" : "Done",
                            StudentInfo = new StudentEntity()
                            {
                                StudentID = u.STUDENT_MASTER.student_id,
                                FirstName = u.STUDENT_MASTER.first_name,
                                LastName = u.STUDENT_MASTER.last_name
                            },
                            SubmitDate = u.submit_dt,
                            HomeworkEntity = new HomeworkEntity()
                            {
                                HomeworkID = u.homework_id,
                                HomeworkDate = u.HOMEWORK_MASTER.homework_dt,
                                Remarks = u.HOMEWORK_MASTER.remarks
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    int idx = data.IndexOf(item);
                    data[idx].AnswerSheetContentText = Convert.ToBase64String(data[idx].AnswerSheetContent);
                }
            }
            return data;
        }

        public async Task<HomeworkDetailEntity> GetHomeworkByHomeworkID(long homeworkdetailID)
        {
            var data = (from u in this.context.HOMEWORK_MASTER_DTL
                        .Include("TEST_MASTER")
                        .Include("STUDENT_MASTER")
                        .Include("BRANCH_MASTER")
                        where u.homework_master_dtl_id == homeworkdetailID
                        select new HomeworkDetailEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            AnswerSheetContent = u.homework_sheet_content,
                            HomeworkDetailID = u.homework_master_dtl_id,
                            AnswerSheetName = u.homework_sheet_name,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Remarks = u.remarks,
                            Status = u.status,
                            StatusText = u.status == 1 ? "Pending" : "Done",
                            StudentInfo = new StudentEntity()
                            {
                                StudentID = u.STUDENT_MASTER.student_id,
                                FirstName = u.STUDENT_MASTER.first_name,
                                LastName = u.STUDENT_MASTER.last_name
                            },
                            SubmitDate = u.submit_dt,
                            HomeworkEntity = new HomeworkEntity()
                            {
                                HomeworkID = u.homework_id,
                                HomeworkDate = u.HOMEWORK_MASTER.homework_dt,
                                Remarks = u.HOMEWORK_MASTER.remarks
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();

            return data;
        }

        public ResponseModel RemoveHomework(long homeworkdetailID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var data = (from u in this.context.HOMEWORK_MASTER_DTL
                            where u.homework_master_dtl_id == homeworkdetailID
                            select u).FirstOrDefault();

                if (data != null)
                {
                    data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                    responseModel.Status = true;
                    responseModel.Message = "Homework Removed Successfully.";
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "Homework Not Found.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
            return responseModel;
        }

        public ResponseModel RemoveHomeworkdetail(long homeworkdetailID, long UserID)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var data = (from u in this.context.HOMEWORK_MASTER_DTL
                            where u.homework_id == homeworkdetailID && u.stud_id == UserID
                            select u).ToList();

                if (data != null)
                {
                    this.context.HOMEWORK_MASTER_DTL.RemoveRange(data);
                    this.context.SaveChanges();
                    responseModel.Status = true;
                    responseModel.Message = "Homework Details Removed Successfully.";
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "Homework Details Not Found.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
            return responseModel;



        }

        public async Task<ResponseModel> HomeworkDetailUpdate(HomeworkDetailEntity homeworkDetail)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                Model.HOMEWORK_MASTER_DTL homework = new Model.HOMEWORK_MASTER_DTL();
                bool isUpdate = true;
                var data = (from t in this.context.HOMEWORK_MASTER_DTL
                            where t.homework_id == homeworkDetail.HomeworkEntity.HomeworkID && t.stud_id == homeworkDetail.HomeworkEntity.StudentInfo.StudentID
                            select t).ToList();


                foreach (var item in data)
                {

                    item.remarks = homeworkDetail.Remarks;
                    item.status = homeworkDetail.Status;
                    this.context.HOMEWORK_MASTER_DTL.Add(item);
                    this.context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }


                var res = this.context.SaveChanges() > 0 ? homeworkDetail.HomeworkEntity.HomeworkID : 0;
                if (res > 0)
                {
                    homeworkDetail.HomeworkDetailID = homework.homework_master_dtl_id;
                    responseModel.Data = homeworkDetail;

                    responseModel.Status = true;
                    responseModel.Message = "Homework Details Remarks Updated Successfully.";
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "Homework Details Remarks Not Updated.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
            return responseModel;

        }

        public async Task<ResponseModel> HomeworkDetailFileUpdate(HomeworkDetailEntity homeworkDetail)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                Model.HOMEWORK_MASTER_DTL homework = new Model.HOMEWORK_MASTER_DTL();
                bool isUpdate = true;
                var data = (from t in this.context.HOMEWORK_MASTER_DTL
                            where t.homework_id == homeworkDetail.HomeworkEntity.HomeworkID && t.stud_id == homeworkDetail.StudentInfo.StudentID
                            select t).ToList();


                foreach (var item in data)
                {
                    item.trans_id = this.AddTransactionData(homeworkDetail.Transaction);
                    item.student_filepath = homeworkDetail.StudentFilePath;
                    this.context.HOMEWORK_MASTER_DTL.Add(item);
                    this.context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }
                var res = this.context.SaveChanges() > 0 ? homework.homework_master_dtl_id : 0;
                if (res > 0)
                {
                    homeworkDetail.HomeworkDetailID = homework.homework_master_dtl_id;
                    responseModel.Data = homeworkDetail;
                    responseModel.Status = true;
                    responseModel.Message = "Homework Details File Updated Successfully.";
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "Homework Details File Not Updated.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
            return responseModel;

        }

    }
}
