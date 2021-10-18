using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Ashirvad.Repo.Services.Area
{
    public class Marks : ModelAccess, IMarksAPI
    {
        public async Task<long> MarksMaintenance(MarksEntity MarksInfo)
        {
            Model.MARKS_MASTER MarksMaster = new Model.MARKS_MASTER();           
            bool isUpdate = true;
            var data = (from Marks in this.context.MARKS_MASTER
                        where Marks.marks_id == MarksInfo.MarksID
                        select new
                        {
                            MarksMaster = Marks
                        }).FirstOrDefault();
            if (data == null)
            {
                MarksMaster = new Model.MARKS_MASTER();       
                isUpdate = false;
            }
            else
            {
                MarksMaster = data.MarksMaster;                
                MarksInfo.Transaction.TransactionId = data.MarksMaster.trans_id;
            }

            MarksMaster.branch_id = MarksInfo.BranchInfo.BranchID;
            MarksMaster.std_id = MarksInfo.StandardInfo.StandardID;     
            MarksMaster.row_sta_cd = MarksInfo.RowStatus.RowStatusId;
            MarksMaster.trans_id = this.AddTransactionData(MarksInfo.Transaction);
            this.context.MARKS_MASTER.Add(MarksMaster);
            if (isUpdate)
            {
                this.context.Entry(MarksMaster).State = System.Data.Entity.EntityState.Modified;
            }
            var result = this.context.SaveChanges();
            if (result > 0)
            {
                MarksInfo.MarksID = MarksMaster.marks_id;
               var result2 = MarksDetailMaintenance(MarksInfo).Result;
               return result2 > 0 ? MarksInfo.MarksID : 0;
            }
            return this.context.SaveChanges() > 0 ? MarksMaster.marks_id : 0;
        }

        public async Task<long> MarksDetailMaintenance(MarksEntity MarksInfo)
        {
            Model.MARKS_MASTER_DTL MarksMaster = new Model.MARKS_MASTER_DTL();
            bool isUpdate = true;
            var data = (from Marks in this.context.MARKS_MASTER_DTL
                        where Marks.marks_master_dtl_id == MarksInfo.MarksDetailID
                        select new
                        {
                            MarksMaster = Marks
                        }).FirstOrDefault();
            if (data == null)
            {
                MarksMaster = new Model.MARKS_MASTER_DTL();
                isUpdate = false;
            }
            else
            {
                MarksMaster = data.MarksMaster;
                MarksInfo.Transaction.TransactionId = data.MarksMaster.trans_id;
            }

            MarksMaster.marks_sheet_content = MarksInfo.MarksContent;
            MarksMaster.marks_sheet_name = MarksInfo.MarksContentFileName;
            MarksMaster.marks_id = MarksInfo.MarksID;
            MarksMaster.marks_filepath = MarksInfo.MarksFilepath;
            MarksMaster.row_sta_cd = MarksInfo.RowStatus.RowStatusId;
            MarksMaster.trans_id = this.AddTransactionData(MarksInfo.Transaction);
            this.context.MARKS_MASTER_DTL.Add(MarksMaster);
            if (isUpdate)
            {
                this.context.Entry(MarksMaster).State = System.Data.Entity.EntityState.Modified;
            }

            return this.context.SaveChanges() > 0 ? MarksMaster.marks_master_dtl_id : 0;
        }

        public async Task<List<MarksEntity>> GetAllMarks()
        {
            var data = (from u in this.context.MARKS_MASTER
                        join b in this.context.MARKS_MASTER_DTL on u.marks_id equals b.marks_id
                        where u.row_sta_cd == 1
                        select new MarksEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            MarksID = u.marks_id,                         
                            MarksFilepath= b.marks_filepath,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            StandardInfo = new StandardEntity() { StandardID = u.std_id,Standard=u.STD_MASTER.standard },
                            BranchInfo = new BranchEntity() { BranchID = u.branch_id }
                        }).ToList();

            return data;
        }

        public Task<List<MarksEntity>> GetAllMarksWithoutImage()
        {
            throw new NotImplementedException();
        }

        public async Task<MarksEntity> GetMarksByMarksID(long MarksID)
        {
            var data = (from u in this.context.MARKS_MASTER
                        join b in this.context.MARKS_MASTER_DTL on u.marks_id equals b.marks_id
                        where u.marks_id == MarksID
                        select new MarksEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            MarksID = u.marks_id,                                                 
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id },
                            BranchInfo= new BranchEntity() { BranchID = u.branch_id },
                            StandardInfo= new StandardEntity() { StandardID = u.std_id },
                            MarksDetailID= b.marks_master_dtl_id,
                            MarksFilepath = b.marks_filepath,
                            MarksContentFileName=b.marks_sheet_name,
                            MarksContent=b.marks_sheet_content
                        }).FirstOrDefault();

            return data;
        }

        public bool RemoveMarks(long MarksID, string lastupdatedby)
        {
            var data = (from u in this.context.MARKS_MASTER
                        where u.marks_id == MarksID
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
