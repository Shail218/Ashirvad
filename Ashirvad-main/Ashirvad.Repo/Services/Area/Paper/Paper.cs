using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Paper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Paper
{
    public class Paper : ModelAccess, IPaperAPI
    {
        public async Task<long> PaperMaintenance(PaperEntity paperInfo)
        {
            Model.PRACTICE_PAPER paperMaster = new Model.PRACTICE_PAPER();
            Model.PRACTICE_PAPER_REL paperData = new Model.PRACTICE_PAPER_REL();
            bool isUpdate = true;
            var data = (from p in this.context.PRACTICE_PAPER.Include("PRACTICE_PAPER_REL")
                        where p.paper_id == paperInfo.PaperID
                        select new
                        {
                            paperMaster = p,
                            paperData = p.PRACTICE_PAPER_REL
                        }).FirstOrDefault();
            if (data == null)
            {
                paperMaster = new Model.PRACTICE_PAPER();
                paperData = new Model.PRACTICE_PAPER_REL();
                isUpdate = false;
            }
            else
            {
                paperMaster = data.paperMaster;
                paperData = data.paperData.FirstOrDefault();
                paperInfo.Transaction.TransactionId = data.paperMaster.trans_id;
            }

            paperMaster.row_sta_cd = paperInfo.RowStatus.RowStatusId;
            paperMaster.trans_id = this.AddTransactionData(paperInfo.Transaction);
            paperMaster.branch_id = paperInfo.Branch.BranchID;
            paperMaster.std_id = paperInfo.Standard.StandardID;
            paperMaster.sub_id = paperInfo.Subject.SubjectID;
            paperMaster.remarks = paperInfo.Remarks;
            paperMaster.batch_type = paperInfo.BatchTypeID;
            this.context.PRACTICE_PAPER.Add(paperMaster);
            if (isUpdate)
            {
                this.context.Entry(paperMaster).State = System.Data.Entity.EntityState.Modified;
            }
            if (!isUpdate)
            {
                paperData.paper_id = paperMaster.paper_id;
            }

            paperData.file_path = paperInfo.PaperData.FilePath;
            paperData.paper_file = paperInfo.PaperData.PaperPath;
            this.context.PRACTICE_PAPER_REL.Add(paperData);
            if (isUpdate)
            {
                this.context.Entry(paperData).State = System.Data.Entity.EntityState.Modified;
            }
            return this.context.SaveChanges() > 0 ? paperMaster.paper_id : 0;
        }

        public async Task<List<PaperEntity>> GetAllPapers(long branchID)
        {
            var data = (from u in this.context.PRACTICE_PAPER
                        .Include("PRACTICE_PAPER_REL")
                        .Include("BRANCH_MASTER")
                        .Include("STD_MASTER")
                        .Include("SUBJECT_MASTER")
                        where (0 == branchID || u.branch_id == branchID && u.row_sta_cd == 1)
                        select new PaperEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Standard = new StandardEntity()
                            {
                                StandardID = u.std_id,
                                Standard = u.STD_MASTER.standard
                            },
                            Subject = new SubjectEntity()
                            {
                                SubjectID = u.SUBJECT_MASTER.subject_id,
                                Subject = u.SUBJECT_MASTER.subject
                            },
                            BatchTypeID = u.batch_type,
                            BatchTypeText = u.batch_type == 1 ? "Morning" : u.batch_type == 2 ? "Afternoon" : "Evening",
                            PaperID = u.paper_id,
                            Remarks = u.remarks,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            PaperData = new PaperData()
                            {
                                FilePath = u.PRACTICE_PAPER_REL.Count == 0 ? " " : "http://highpack-001-site12.dtempurl.com" + u.PRACTICE_PAPER_REL.FirstOrDefault().file_path,
                                PaperID = u.PRACTICE_PAPER_REL.Count == 0 ? 0 : u.PRACTICE_PAPER_REL.FirstOrDefault().paper_id,
                                PaperPath = u.PRACTICE_PAPER_REL.Count == 0 ? " " : u.PRACTICE_PAPER_REL.FirstOrDefault().paper_file,
                                UniqueID = u.PRACTICE_PAPER_REL.Count == 0 ? 0 : u.PRACTICE_PAPER_REL.FirstOrDefault().unique_id
                            }
                        }).ToList();

            //if (data?.Count > 0)
            //{
            //    foreach (var item in data)
            //    {
            //        int idx = data.IndexOf(item);
            //        data[idx].PaperData.PaperContentText = data[idx].PaperData.PaperContent.Length > 0 ? Convert.ToBase64String(data[idx].PaperData.PaperContent) : "";
            //    }
            //}

            return data;
        }


        public async Task<List<SubjectEntity>> GetPracticePaperSubject(long branchID, long stdID)
        {
            var data = (from u in this.context.PRACTICE_PAPER
                        .Include("PRACTICE_PAPER_REL")
                        .Include("SUBJECT_MASTER")
                        where u.branch_id == branchID
                        && u.std_id == stdID
                        select new SubjectEntity()
                        {
                            SubjectID = u.SUBJECT_MASTER.subject_id,
                            Subject = u.SUBJECT_MASTER.subject
                        }).ToList();

            return data;
        }

        public async Task<List<PaperEntity>> GetPracticePapersByStandardSubjectAndBranch(long branchID, long stdID, long subID, int batchTypeID)
        {
            var data = (from u in this.context.PRACTICE_PAPER
                        .Include("PRACTICE_PAPER_REL")
                        .Include("BRANCH_MASTER")
                        .Include("STD_MASTER")
                        .Include("SUBJECT_MASTER")
                        where u.branch_id == branchID
                        && (0 == stdID || u.std_id == stdID)
                        && (0 == subID || u.sub_id == subID)
                        && (0 == batchTypeID || u.batch_type == batchTypeID)
                        select new PaperEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Standard = new StandardEntity()
                            {
                                StandardID = u.std_id,
                                Standard = u.STD_MASTER.standard
                            },
                            Subject = new SubjectEntity()
                            {
                                SubjectID = u.SUBJECT_MASTER.subject_id,
                                Subject = u.SUBJECT_MASTER.subject
                            },
                            BatchTypeID = u.batch_type,
                            BatchTypeText = u.batch_type == 1 ? "Morning" : u.batch_type == 2 ? "Afternoon" : "Evening",
                            PaperID = u.paper_id,
                            Remarks = u.remarks,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            PaperData = new PaperData()
                            {
                                FilePath = u.PRACTICE_PAPER_REL.Count == 0 ? " " : "http://highpack-001-site12.dtempurl.com" + u.PRACTICE_PAPER_REL.FirstOrDefault().file_path,
                                PaperID = u.PRACTICE_PAPER_REL.Count == 0 ? 0 : u.PRACTICE_PAPER_REL.FirstOrDefault().paper_id,
                                PaperPath = u.PRACTICE_PAPER_REL.Count == 0 ? " " : u.PRACTICE_PAPER_REL.FirstOrDefault().paper_file,
                                UniqueID = u.PRACTICE_PAPER_REL.Count == 0 ? 0 : u.PRACTICE_PAPER_REL.FirstOrDefault().unique_id
                            }
                        }).ToList();

            if (data?.Count > 0)
            {
                foreach (var item in data)
                {
                    int idx = data.IndexOf(item);
                    data[idx].PaperData.PaperContentText = data[idx].PaperData.PaperContent.Length > 0 ? Convert.ToBase64String(data[idx].PaperData.PaperContent) : "";
                }
            }

            return data;
        }


        public async Task<List<PaperEntity>> GetAllPaperWithoutContent(long branchID)
        {
            var data = (from u in this.context.PRACTICE_PAPER
                        .Include("PRACTICE_PAPER_REL")
                        .Include("BRANCH_MASTER")
                        .Include("STD_MASTER")
                        .Include("SUBJECT_MASTER")
                        where (0 == branchID || u.branch_id == branchID) && u.row_sta_cd == 1
                        select new PaperEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Standard = new StandardEntity()
                            {
                                StandardID = u.std_id,
                                Standard = u.STD_MASTER.standard
                            },
                            Subject = new SubjectEntity()
                            {
                                SubjectID = u.SUBJECT_MASTER.subject_id,
                                Subject = u.SUBJECT_MASTER.subject
                            },
                            BatchTypeID = u.batch_type,
                            BatchTypeText = u.batch_type == 1 ? "Morning" : u.batch_type == 2 ? "Afternoon" : "Evening",
                            PaperID = u.paper_id,
                            Remarks = u.remarks,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            PaperData = new PaperData()
                            {
                                FilePath = u.PRACTICE_PAPER_REL.Count == 0?" " :"http://highpack-001-site12.dtempurl.com" + u.PRACTICE_PAPER_REL.FirstOrDefault().file_path,
                                PaperID = u.PRACTICE_PAPER_REL.Count == 0 ? 0 : u.PRACTICE_PAPER_REL.FirstOrDefault().paper_id,
                                PaperPath = u.PRACTICE_PAPER_REL.Count == 0 ? " " : u.PRACTICE_PAPER_REL.FirstOrDefault().paper_file,
                                UniqueID = u.PRACTICE_PAPER_REL.Count == 0 ? 0 : u.PRACTICE_PAPER_REL.FirstOrDefault().unique_id
                            }
                        }).ToList();

            return data;
        }

        public async Task<PaperEntity> GetPaperByPaperID(long paperID)
        {
            var data = (from u in this.context.PRACTICE_PAPER
                        .Include("PRACTICE_PAPER_REL")
                        .Include("BRANCH_MASTER")
                        .Include("STD_MASTER")
                        .Include("SUBJECT_MASTER")
                        where u.paper_id == paperID
                        select new PaperEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            Branch = new BranchEntity()
                            {
                                BranchID = u.BRANCH_MASTER.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Standard = new StandardEntity()
                            {
                                StandardID = u.std_id,
                                Standard = u.STD_MASTER.standard
                            },
                            Subject = new SubjectEntity()
                            {
                                SubjectID = u.SUBJECT_MASTER.subject_id,
                                Subject = u.SUBJECT_MASTER.subject
                            },
                            BatchTypeID = u.batch_type,
                            BatchTypeText = u.batch_type == 1 ? "Morning" : u.batch_type == 2 ? "Afternoon" : "Evening",
                            PaperID = u.paper_id,
                            Remarks = u.remarks,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            PaperData = new PaperData()
                            {
                                FilePath = u.PRACTICE_PAPER_REL.Count == 0 ? " " : u.PRACTICE_PAPER_REL.FirstOrDefault().file_path,
                                PaperID = u.PRACTICE_PAPER_REL.Count == 0 ? 0 : u.PRACTICE_PAPER_REL.FirstOrDefault().paper_id,
                                PaperPath = u.PRACTICE_PAPER_REL.Count == 0 ? " " : u.PRACTICE_PAPER_REL.FirstOrDefault().paper_file,
                                UniqueID = u.PRACTICE_PAPER_REL.Count == 0 ? 0 : u.PRACTICE_PAPER_REL.FirstOrDefault().unique_id
                            }
                        }).FirstOrDefault();

            //if (data != null)
            //{
            //    data.PaperData.PaperContentText = data.PaperData.PaperContent.Length > 0 ? Convert.ToBase64String(data.PaperData.PaperContent) : "";
            //}

            return data;
        }

        public bool RemovePaper(long paperID, string lastupdatedby)
        {
            var data = (from u in this.context.PRACTICE_PAPER
                        where u.paper_id == paperID
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
