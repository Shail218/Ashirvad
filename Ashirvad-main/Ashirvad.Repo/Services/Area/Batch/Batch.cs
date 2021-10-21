using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Batch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Batch
{
    public class Batch : ModelAccess, IBatchAPI
    {
        public async Task<long> CheckBatch(int batchtime, long std, long Id)
        {
            long result;
            bool isExists = this.context.BATCH_MASTER.Where(s => (Id == 0 || s.batch_id != Id) && s.batch_time == batchtime && s.std_id == std && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<long> BatchMaintenance(BatchEntity batchInfo)
        {
            Model.BATCH_MASTER batchMaster = new Model.BATCH_MASTER();
            if (CheckBatch((int)batchInfo.BatchType, batchInfo.StandardInfo.StandardID, batchInfo.BatchID).Result != -1)
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
                batchMaster.std_id = batchInfo.StandardInfo.StandardID;
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
            var data = (from u in this.context.BATCH_MASTER
                        where (branchID == 0 || u.branch_id == branchID) && u.row_sta_cd == 1 && (STDID == 0 || u.std_id == STDID)
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
                            StandardInfo = new StandardEntity()
                            {
                                StandardID = u.std_id,
                                Standard = u.STD_MASTER.standard
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
        
        public async Task<List<BatchEntity>> GetAllBatches()
        {
            var data = (from u in this.context.BATCH_MASTER
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
                            StandardInfo = new StandardEntity()
                            {
                                StandardID = u.std_id,
                                Standard = u.STD_MASTER.standard
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
                            StandardInfo = new StandardEntity()
                            {
                                StandardID = u.std_id,
                                Standard = u.STD_MASTER.standard
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
