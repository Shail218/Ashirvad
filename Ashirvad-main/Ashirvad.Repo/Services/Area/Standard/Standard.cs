using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.Standard
{
    public class Standard : ModelAccess, IStandardAPI
    {
        ResponseModel res = new ResponseModel();

        public async Task<long> CheckStandard(string std, long branch, long stdID)
        {
            long result;
            bool isExists = this.context.STD_MASTER.Where(s => (stdID == 0 || s.std_id != stdID) && s.standard.ToLower() == std.ToLower() && s.branch_id == branch && s.row_sta_cd == 1).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<long> StandardMaintenance(StandardEntity standardInfo)
        {
            Model.STD_MASTER standardMaster = new Model.STD_MASTER();
            if (CheckStandard(standardInfo.Standard, standardInfo.BranchInfo.BranchID,standardInfo.StandardID).Result != -1)
            {
                bool isUpdate = true;
                var data = (from standard in this.context.STD_MASTER
                            where standard.std_id == standardInfo.StandardID
                            select standard).FirstOrDefault();
                if (data == null)
                {
                    standardMaster = new Model.STD_MASTER();

                    isUpdate = false;
                }
                else
                {
                    standardMaster = data;
                    standardInfo.Transaction.TransactionId = data.trans_id;
                }

                standardMaster.standard = standardInfo.Standard;
                standardMaster.branch_id = standardInfo.BranchInfo.BranchID;
                standardMaster.row_sta_cd = (int)standardInfo.RowStatus.RowStatus;
                standardMaster.class_dtl_id = standardInfo.Branchclass.Class_dtl_id == 0 ? (long?)null : standardInfo.Branchclass.Class_dtl_id;
                standardMaster.trans_id = standardInfo.Transaction.TransactionId;
                this.context.STD_MASTER.Add(standardMaster);
                if (isUpdate)
                {
                    this.context.Entry(standardMaster).State = System.Data.Entity.EntityState.Modified;
                }
                return this.context.SaveChanges() > 0 ? standardMaster.std_id : 0;
            }
            else
            {
                return -1;
            }
        }

        public async Task<List<StandardEntity>> GetAllStandards(long branchID)
        {
            var data = (from u in this.context.STD_MASTER
                        .Include("CLASS_DTL_MASTER") orderby u.std_id descending
                        where (branchID == 0 || u.branch_id == branchID) && u.row_sta_cd == 1
                        select new StandardEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Standard = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                            StandardID = u.std_id,
                            BranchInfo = new BranchEntity()
                            {
                                BranchID = u.branch_id,
                                BranchName = u.BRANCH_MASTER.branch_name
                            },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }       

        public async Task<List<StandardEntity>> GetAllStandardsName(long branchid)
        {
            var data = (from u in this.context.CLASS_MASTER                       
                        orderby u.class_id descending
                        where u.row_sta_cd == 1
                        select new StandardEntity()
                        {                           
                            Standard = u.class_name,                            
                        }).Distinct().ToList();

            return data;
        }

        public async Task<List<StandardEntity>> GetAllStandardsID(string standardname,long branchid)
        {
            var data = (from u in this.context.STD_MASTER
                        .Include("CLASS_DTL_MASTER")
                        orderby u.std_id descending
                        where u.row_sta_cd == 1 && u.CLASS_DTL_MASTER.CLASS_MASTER.class_name == standardname && (u.branch_id == branchid || branchid == 0)
                        select new StandardEntity()
                        {
                            Standard = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                            StandardID = u.std_id
                        }).ToList();

            return data;
        }

        public async Task<StandardEntity> GetStandardsByID(long standardID)
        {
            var data = (from u in this.context.STD_MASTER
                        where u.std_id == standardID
                        select new StandardEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = u.row_sta_cd
                            },
                            Standard = u.CLASS_DTL_MASTER.CLASS_MASTER.class_name,
                            StandardID = u.std_id,
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id },
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();

            return data;
        }

        public bool RemoveStandard(long StandardID, string lastupdatedby)
        {
            var data = (from u in this.context.STD_MASTER
                        where u.std_id == StandardID
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
