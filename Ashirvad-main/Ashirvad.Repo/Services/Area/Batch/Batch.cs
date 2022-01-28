using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Batch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area.Batch
{
    public class Batch : ModelAccess, IBatchAPI
    {
        public async Task<long> CheckBatch(int batchtime, long std, long courseid, long Id)
        {
            long result;
            bool isExists = this.context.BATCH_MASTER.Where(s => (Id == 0 || s.batch_id != Id) && s.batch_time == batchtime && s.class_dtl_id == std && s.course_dtl_id == courseid && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<long> BatchMaintenance(BatchEntity batchInfo)
        {
            Model.BATCH_MASTER batchMaster = new Model.BATCH_MASTER();
            if (CheckBatch((int)batchInfo.BatchType, batchInfo.BranchClass.Class_dtl_id, batchInfo.BranchCourse.course_dtl_id,batchInfo.BatchID).Result != -1)
            {
                bool isUpdate = true;
                var data = (from batch in this.context.BATCH_MASTER
                            where batch.batch_id == batchInfo.BatchID
                            select batch).FirstOrDefault();
                if (data == null)
                {
                    batchMaster = new Model.BATCH_MASTER();
                    isUpdate = false;
                }
                else
                {
                    batchMaster = data;
                    batchInfo.Transaction.TransactionId = data.trans_id;
                }
                batchMaster.batch_time = (int)batchInfo.BatchType;
                batchMaster.course_dtl_id = batchInfo.BranchCourse.course_dtl_id;
                batchMaster.class_dtl_id = batchInfo.BranchClass.Class_dtl_id;
                batchMaster.mon_fri_batch_time = batchInfo.MonFriBatchTime;
                batchMaster.sat_batch_time = batchInfo.SatBatchTime;
                batchMaster.sun_batch_time = batchInfo.SunBatchTime;
                batchMaster.branch_id = batchInfo.BranchInfo.BranchID;
                batchMaster.row_sta_cd = batchInfo.RowStatus.RowStatusId;
                batchMaster.trans_id = this.AddTransactionData(batchInfo.Transaction);
                this.context.BATCH_MASTER.Add(batchMaster);
                if (isUpdate)
                {
                    this.context.Entry(batchMaster).State = System.Data.Entity.EntityState.Modified;
                }
                return this.context.SaveChanges() > 0 ? batchMaster.batch_id : 0;
            }
            else
            {
                return -1;
            }
                
        }

        public async Task<List<BatchEntity>> GetAllBatches(long branchID,long STDID=0)
        {
            var data = (from u in this.context.BATCH_MASTER orderby u.batch_id descending
                        where (branchID == 0 || u.branch_id == branchID) && u.row_sta_cd == 1 && (STDID == 0 || u.class_dtl_id == STDID)
                        select new BatchEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            BatchTime = u.batch_time,
                            MonFriBatchTime = u.mon_fri_batch_time,
                            SatBatchTime = u.sat_batch_time,
                            SunBatchTime = u.sun_batch_time,
                            BatchID = u.batch_id,
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue == true ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity
                                {
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                }
                            },
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : Enums.BatchType.Evening
                        }).ToList();

            return data;
        }

        public async Task<List<BatchEntity>> GetAllCustomBatch(DataTableAjaxPostModel model, long branchID)
        {
            var Result = new List<BatchEntity>();
            bool Isasc = model.order[0].dir == "desc" ? false : true;
            long count = this.context.BATCH_MASTER.Where(s => s.row_sta_cd == 1 && s.branch_id == branchID).Count();
            var data = (from u in this.context.BATCH_MASTER                       
                        where u.branch_id == branchID && u.row_sta_cd == 1 
                        && (model.search.value == null
                        || model.search.value == ""
                        || u.mon_fri_batch_time.ToLower().Contains(model.search.value)
                        || u.sat_batch_time.ToLower().Contains(model.search.value)
                        || u.sun_batch_time.ToLower().Contains(model.search.value))
                        orderby u.batch_id descending
                        select new BatchEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            BatchTime = u.batch_time,
                            MonFriBatchTime = u.mon_fri_batch_time,
                            SatBatchTime = u.sat_batch_time,
                            SunBatchTime = u.sun_batch_time,
                            BatchID = u.batch_id,
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue == true ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                }
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id.HasValue ? u.course_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                }
                            },
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Count = count,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : Enums.BatchType.Evening,
                            BatchText = u.batch_time == 1 ? "Morning" : u.batch_time == 2 ? "Afternoon" : "Evening"
                        })
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();

            return data;
        }

        public async Task<List<BatchEntity>> GetAllBatches()
        {
            var data = (from u in this.context.BATCH_MASTER
                        orderby u.batch_id descending
                        select new BatchEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            BatchTime = u.batch_time,
                            MonFriBatchTime = u.mon_fri_batch_time,
                            SatBatchTime = u.sat_batch_time,
                            SunBatchTime = u.sun_batch_time,
                            BatchID = u.batch_id,
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue == true ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                }
                            },
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : Enums.BatchType.Evening
                        }).ToList();

            return data;
        }


        public async Task<BatchEntity> GetBatchByID(long branchID)
        {
            var data = (from u in this.context.BATCH_MASTER
                        where branchID == 0 || u.batch_id == branchID
                        select new BatchEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            BatchTime = u.batch_time,
                            MonFriBatchTime = u.mon_fri_batch_time,
                            SatBatchTime = u.sat_batch_time,
                            SunBatchTime = u.sun_batch_time,
                            BatchID = u.batch_id,
                            BranchClass = new BranchClassEntity()
                            {
                                Class_dtl_id = u.class_dtl_id.HasValue == true ? u.class_dtl_id.Value : 0,
                                Class = new ClassEntity()
                                {
                                    ClassName = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name
                                }
                            },
                            BranchCourse = new BranchCourseEntity()
                            {
                                course_dtl_id = u.course_dtl_id.HasValue == true ? u.course_dtl_id.Value : 0,
                                course = new CourseEntity()
                                {
                                    CourseName = u.COURSE_DTL_MASTER.COURSE_MASTER.course_name
                                }
                            },
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            BatchType = u.batch_time == 1 ? Enums.BatchType.Morning : u.batch_time == 2 ? Enums.BatchType.Afternoon : Enums.BatchType.Evening
                        }).FirstOrDefault();

            return data;
        }

        public bool RemoveBatch(long BatchID, string lastupdatedby)
        {
            var data = (from u in this.context.BATCH_MASTER
                        where u.batch_id == BatchID
                        select u).FirstOrDefault();
            if (data != null)
            {
                data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                data.trans_id = this.AddTransactionData(new TransactionEntity()
                {
                    TransactionId = data.trans_id,
                    LastUpdateBy = lastupdatedby
                });
                this.context.SaveChanges();
                return true;
            }

            return false;
        }

    }
}
