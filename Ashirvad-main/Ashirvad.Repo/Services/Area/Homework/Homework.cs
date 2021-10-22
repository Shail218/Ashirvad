using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Homework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Homework
{
    public class Homework : ModelAccess, IHomeworkAPI
    {
        public async Task<long> HomeworkMaintenance(HomeworkEntity homeworkInfo)
        {
            Model.HOMEWORK_MASTER homework = new Model.HOMEWORK_MASTER();
            bool isUpdate = true;
            var data = (from t in this.context.HOMEWORK_MASTER
                        where t.homework_id == homeworkInfo.HomeworkID
                        select t).FirstOrDefault();
            if (data == null)
            {
                data = new Model.HOMEWORK_MASTER();
                isUpdate = false;
            }
            else
            {
                homework = data;
                homeworkInfo.Transaction.TransactionId = data.trans_id;
            }

            homework.row_sta_cd = homeworkInfo.RowStatus.RowStatusId;
            homework.trans_id = this.AddTransactionData(homeworkInfo.Transaction);
            homework.branch_id = homeworkInfo.BranchInfo.BranchID;
            homework.batch_time_id = homeworkInfo.BatchTimeID;
            if (homeworkInfo.HomeworkContent?.Length > 0)
            {
                homework.homework_content = homeworkInfo.HomeworkContent;
                homework.homework_file = homeworkInfo.HomeworkContentFileName;
            }

            homework.remarks = homeworkInfo.Remarks;
            homework.homework_dt = homeworkInfo.HomeworkDate;
            homework.sub_id = homeworkInfo.SubjectInfo.SubjectID;
            homework.std_id = homeworkInfo.StandardInfo.StandardID;
            this.context.HOMEWORK_MASTER.Add(homework);
            if (isUpdate)
            {
                this.context.Entry(homework).State = System.Data.Entity.EntityState.Modified;
            }

            return this.context.SaveChanges() > 0 ? homework.homework_id : 0;
        }

        public async Task<List<HomeworkEntity>> GetAllHomeworkByBranch(long branchID, long stdID, int batchTime)
        {
            var data = (from u in this.context.HOMEWORK_MASTER
                        .Include("BRANCH_MASTER")
                        .Include("STD_MASTER")
                        .Include("SUBJECT_MASTER")
                        where u.branch_id == branchID
                        && (0 == stdID || u.std_id == stdID)
                        && (0 == batchTime || u.batch_time_id == batchTime)
                        select new HomeworkEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            HomeworkContent = u.homework_content,
                            HomeworkID = u.homework_id,
                            HomeworkContentFileName = u.homework_file,
                            HomeworkDate = u.homework_dt,
                            Remarks = u.remarks,
                            StandardInfo = new StandardEntity()
                            {
                                Standard = u.STD_MASTER.standard,
                                StandardID = u.STD_MASTER.std_id
                            },
                            SubjectInfo = new SubjectEntity()
                            {
                                Subject = u.SUBJECT_MASTER.subject,
                                SubjectID = u.SUBJECT_MASTER.subject_id
                            },
                            BatchTimeID = u.batch_time_id,
                            BatchTimeText = u.batch_time_id == 1 ? "Morning" : u.batch_time_id == 2 ? "Afternoon" : "Evening",
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    int idx = data.IndexOf(item);
                    data[idx].HomeworkContentText = Convert.ToBase64String(data[idx].HomeworkContent);
                }
            }
            return data;
        }

        public async Task<List<HomeworkEntity>> GetAllHomeworks(DateTime hwDate, string searchParam)
        {
            DateTime fromDT = Convert.ToDateTime(hwDate.ToShortTimeString() + " 00:00:00");
            DateTime toDT = Convert.ToDateTime(hwDate.ToShortTimeString() + " 23:59:59");
            var data = (from u in this.context.HOMEWORK_MASTER
                        .Include("BRANCH_MASTER")
                        .Include("STD_MASTER")
                        .Include("SUBJECT_MASTER")
                        where u.homework_dt >= fromDT && u.homework_dt <= toDT
                        && (string.IsNullOrEmpty(searchParam)
                        || u.remarks.Contains(searchParam)
                        || u.STD_MASTER.standard.Contains(searchParam)
                        || u.SUBJECT_MASTER.subject.Contains(searchParam)
                        || (u.batch_time_id == 1 ? "Morning" : u.batch_time_id == 2 ? "Afternoon" : "Evening").Contains(searchParam))
                        select new HomeworkEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            HomeworkContent = u.homework_content,
                            HomeworkID = u.homework_id,
                            HomeworkContentFileName = u.homework_file,
                            HomeworkDate = u.homework_dt,
                            Remarks = u.remarks,
                            StandardInfo = new StandardEntity()
                            {
                                Standard = u.STD_MASTER.standard,
                                StandardID = u.STD_MASTER.std_id
                            },
                            SubjectInfo = new SubjectEntity()
                            {
                                Subject = u.SUBJECT_MASTER.subject,
                                SubjectID = u.SUBJECT_MASTER.subject_id
                            },
                            BatchTimeID = u.batch_time_id,
                            BatchTimeText = u.batch_time_id == 1 ? "Morning" : u.batch_time_id == 2 ? "Afternoon" : "Evening",
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    int idx = data.IndexOf(item);
                    data[idx].HomeworkContentText = Convert.ToBase64String(data[idx].HomeworkContent);
                }
            }
            return data;
        }

        public async Task<List<HomeworkEntity>> GetAllHomeworkWithoutContentByBranch(long branchID, long stdID)
        {
            var data = (from u in this.context.HOMEWORK_MASTER
                        .Include("BRANCH_MASTER")
                        .Include("STD_MASTER")
                        .Include("SUBJECT_MASTER")
                        where u.branch_id == branchID
                        && (0 == stdID || u.std_id == stdID) && u.row_sta_cd == 1
                        select new HomeworkEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            HomeworkID = u.homework_id,
                            HomeworkContentFileName = u.homework_file,
                            HomeworkDate = u.homework_dt,
                            Remarks = u.remarks,
                            StandardInfo = new StandardEntity()
                            {
                                Standard = u.STD_MASTER.standard,
                                StandardID = u.STD_MASTER.std_id
                            },
                            SubjectInfo = new SubjectEntity()
                            {
                                Subject = u.SUBJECT_MASTER.subject,
                                SubjectID = u.SUBJECT_MASTER.subject_id
                            },
                            BatchTimeID = u.batch_time_id,
                            BatchTimeText = u.batch_time_id == 1 ? "Morning" : u.batch_time_id == 2 ? "Afternoon" : "Evening",
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public async Task<HomeworkEntity> GetHomeworkByHomeworkID(long homeworkID)
        {
            var data = (from u in this.context.HOMEWORK_MASTER
                        .Include("BRANCH_MASTER")
                        .Include("STD_MASTER")
                        .Include("SUBJECT_MASTER")
                        where u.homework_id == homeworkID
                        select new HomeworkEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            HomeworkContent = u.homework_content,
                            HomeworkID = u.homework_id,
                            HomeworkContentFileName = u.homework_file,
                            HomeworkDate = u.homework_dt,
                            Remarks = u.remarks,
                            StandardInfo = new StandardEntity()
                            {
                                Standard = u.STD_MASTER.standard,
                                StandardID = u.STD_MASTER.std_id
                            },
                            SubjectInfo = new SubjectEntity()
                            {
                                Subject = u.SUBJECT_MASTER.subject,
                                SubjectID = u.SUBJECT_MASTER.subject_id
                            },
                            BatchTimeID = u.batch_time_id,
                            BatchTimeText = u.batch_time_id == 1 ? "Morning" : u.batch_time_id == 2 ? "Afternoon" : "Evening",
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();
            if (data != null)
            {
                data.HomeworkContentText = Convert.ToBase64String(data.HomeworkContent);
            }
            return data;
        }



        public bool RemoveHomework(long homeworkID, string lastupdatedby)
        {
            var data = (from u in this.context.HOMEWORK_MASTER
                        where u.homework_id == homeworkID
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
