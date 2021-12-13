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

        public async Task<long> CheckHomework(HomeworkEntity homeworkInfo)
        {
            long result;
            bool isExists = this.context.HOMEWORK_MASTER.Where(s => (homeworkInfo.HomeworkID == 0 || s.homework_id != homeworkInfo.HomeworkID) && 
            s.homework_dt == homeworkInfo.HomeworkDate && s.branch_id == homeworkInfo.BranchInfo.BranchID && s.std_id == homeworkInfo.StandardInfo.StandardID 
            && s.sub_id == homeworkInfo.SubjectInfo.SubjectID && s.batch_time_id== homeworkInfo.BatchTimeID && s.row_sta_cd==1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }
        public async Task<long> HomeworkMaintenance(HomeworkEntity homeworkInfo)
        {
            Model.HOMEWORK_MASTER homework = new Model.HOMEWORK_MASTER();
            if (CheckHomework(homeworkInfo).Result != -1)
            {
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
                homework.homework_file = homeworkInfo.HomeworkContentFileName;
                homework.file_path = homeworkInfo.FilePath;
                homework.remarks = homeworkInfo.Remarks;
                homework.homework_dt = homeworkInfo.HomeworkDate;
                homework.sub_id = homeworkInfo.SubjectInfo.SubjectID;
                homework.std_id = homeworkInfo.StandardInfo.StandardID;
                this.context.HOMEWORK_MASTER.Add(homework);
                if (isUpdate)
                {
                    this.context.Entry(homework).State = System.Data.Entity.EntityState.Modified;
                }
            }
            else
            {
                return -1;
            }
            return this.context.SaveChanges() > 0 ? homework.homework_id : 0;
        }

        public async Task<List<HomeworkEntity>> GetAllHomeworkByBranchStudent(long branchID, long stdID, int batchTime, long studentId)
        {
            var data = (from u in this.context.HOMEWORK_MASTER
                        .Include("BRANCH_MASTER")
                        .Include("STD_MASTER")
                        .Include("SUBJECT_MASTER")
                            //join hd in this.context.HOMEWORK_MASTER_DTL on u.homework_id equals hd.homework_id
                        where u.branch_id == branchID
                        && (u.std_id == stdID)
                        && (u.batch_time_id == batchTime) && u.row_sta_cd == 1 /*&& hd.stud_id == studentId*/
                        select new HomeworkEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            FilePath = "http://highpack-001-site12.dtempurl.com" + u.file_path,
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
                            //homeworkDetailEntity = new HomeworkDetailEntity()
                            //{
                            //    StatusText = hd.status== (int)Enums.HomeWorkStatus.Done? "Done": "Pending",
                            //    Remarks = hd.remarks,
                            //    Status=hd.status
                            //},

                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).Distinct().ToList();
            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    var a = (from z in this.context.HOMEWORK_MASTER_DTL where z.homework_id == item.HomeworkID && z.stud_id == studentId select z).FirstOrDefault();
                    if (a != null)
                    {
                        item.homeworkDetailEntity = new HomeworkDetailEntity()
                        {
                            StatusText = a.status == (int)Enums.HomeWorkStatus.Done ? "Done" : "Pending",
                            Remarks = a.remarks,
                            Status = a.status
                        };
                    }
                    else
                    {
                        item.homeworkDetailEntity = new HomeworkDetailEntity()
                        {
                            StatusText = "Pending",
                            Remarks = "",
                            Status = 2
                        };
                    }
                }
            }

            return data;
        }


        public async Task<List<HomeworkEntity>> GetAllHomeworkByBranch(long branchID, long stdID, int batchTime)
        {
            var data = (from u in this.context.HOMEWORK_MASTER
                        .Include("BRANCH_MASTER")
                        .Include("STD_MASTER")
                        .Include("SUBJECT_MASTER")
                        join hd in this.context.HOMEWORK_MASTER_DTL on u.homework_id equals hd.homework_id
                        where u.branch_id == branchID
                        && (u.std_id == stdID)
                        && ( u.batch_time_id == batchTime) && u.row_sta_cd == 1
                        select new HomeworkEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            FilePath = u.file_path,
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
                            homeworkDetailEntity = new HomeworkDetailEntity()
                            {
                                StatusText = hd.status== (int)Enums.HomeWorkStatus.Done? "Done": "Pending",
                                Remarks = hd.remarks,
                                Status=hd.status
                            },
                            
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            
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
                            FilePath = u.file_path,
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
                            FilePath = "http://highpack-001-site12.dtempurl.com" + u.file_path,
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
                            FilePath = u.file_path,
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
            //if (data != null)
            //{
            //    data.HomeworkContentText = Convert.ToBase64String(data.HomeworkContent);
            //}
            return data;
        }

        public async Task<List<HomeworkEntity>> GetStudentHomeworkChecking(long homeworkID)
        {

            var data = (from u in this.context.HOMEWORK_MASTER_DTL
                        .Include("HOMEWORK_MASTER")
                        .Include("STD_MASTER")
                        .Include("STUDENT_MASTER")

                        where u.homework_id == homeworkID
                        select new HomeworkEntity()
                        {
                            
                            HomeworkID = u.homework_id,                            
                            HomeworkDate = u.submit_dt,
                            Status = u.status,
                            Remarks = u.remarks,
                            StudentFilePath = u.student_filepath,
                            StandardInfo = new StandardEntity()
                            {
                                Standard = u.HOMEWORK_MASTER.STD_MASTER.standard,
                                StandardID = u.HOMEWORK_MASTER.std_id
                            },
                            SubjectInfo = new SubjectEntity()
                            {
                                Subject = u.STUDENT_MASTER.first_name,
                                SubjectID = u.HOMEWORK_MASTER.sub_id
                            },
                            StudentInfo = new StudentEntity()
                            {
                                StudentID=u.stud_id,
                                Name=u.STUDENT_MASTER.first_name+" "+ u.STUDENT_MASTER.last_name
                            },
                            BatchTimeText = u.HOMEWORK_MASTER.batch_time_id == 1 ? "Morning" : u.HOMEWORK_MASTER.batch_time_id == 2 ? "Afternoon" : "Evening",
                           
                        }).Distinct().ToList();
            //if (data != null)
            //{
            //    data.HomeworkContentText = Convert.ToBase64String(data.HomeworkContent);
            //}
            return data;
        }
        public async Task<List<HomeworkEntity>> GetStudentHomeworkFile(long homeworkID)
        {

            var data = (from u in this.context.HOMEWORK_MASTER_DTL
                        .Include("HOMEWORK_MASTER")
                        .Include("STD_MASTER")
                        where u.homework_id == homeworkID
                        select new HomeworkEntity()
                        {
                            FilePath=u.homework_filepath,
                            HomeworkContentFileName=u.homework_sheet_name,
                           

                        }).ToList();
            //if (data != null)
            //{
            //    data.HomeworkContentText = Convert.ToBase64String(data.HomeworkContent);
            //}
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
