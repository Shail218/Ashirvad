using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.Circular;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ashirvad.Common.Common;

namespace Ashirvad.Repo.Services.Area.Circular
{
    public class Circular : ModelAccess, ICircularAPI
    {
        public async Task<ResponseModel> CircularMaintenance(CircularEntity circularInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                Model.CIRCULAR_MASTER circularMaster = new Model.CIRCULAR_MASTER();
                bool isUpdate = true;
                var data = (from banner in this.context.CIRCULAR_MASTER
                            where banner.circular_id == circularInfo.CircularId
                            select banner).FirstOrDefault();
                if (data == null)
                {
                    data = new Model.CIRCULAR_MASTER();
                    isUpdate = false;
                }
                else
                {
                    circularMaster = data;
                    circularInfo.Transaction.TransactionId = data.trans_id;
                }
                circularMaster.circular_title = circularInfo.CircularTitle;
                circularMaster.circular_description = circularInfo.CircularDescription;
                circularMaster.file_name = circularInfo.FileName;
                circularMaster.file_path = circularInfo.FilePath;
                circularMaster.row_sta_cd = (int)Enums.RowStatus.Active;
                circularMaster.trans_id = this.AddTransactionData(circularInfo.Transaction);
                if (!isUpdate)
                {
                    this.context.CIRCULAR_MASTER.Add(circularMaster);
                }
                var res = this.context.SaveChanges();
                if(res>=0)
                {
                     circularInfo.CircularId=circularMaster.circular_id;
                   // responseModel.Data = circularInfo;
                    responseModel.Status = true;
                    responseModel.Message = isUpdate==true?"Circular Updated Successfully.":"Circular Inserted Successfully.";
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = isUpdate == true ? "Circular Not Updated." : "Circular Not Inserted.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message.ToString();
            }
            return responseModel;
            
        }

        public async Task<List<CircularEntity>> GetAllCircular()
        {
            var data = (from u in this.context.CIRCULAR_MASTER
                        orderby u.circular_id descending
                        where u.row_sta_cd == 1
                        select new CircularEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            CircularId = u.circular_id,
                            CircularTitle = u.circular_title,
                            CircularDescription = u.circular_description,
                            FileName = u.file_name,
                            FilePath = "https://mastermind.org.in" + u.file_path,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();
            return data;
        }

        public async Task<List<CircularEntity>> GetAllCustomCircular(DataTableAjaxPostModel model)
        {
            var Result = new List<CircularEntity>();
            bool Isasc = model.order[0].dir == "desc" ? false : true;
            long count = this.context.CIRCULAR_MASTER.Where(s => s.row_sta_cd == 1).Count();
            var data = (from u in this.context.CIRCULAR_MASTER
                        orderby u.circular_id descending
                        where u.row_sta_cd == 1 && (model.search.value == null
                        || model.search.value == ""
                        || u.circular_title.ToLower().Contains(model.search.value)
                        || u.circular_description.ToLower().Contains(model.search.value))
                        select new CircularEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            CircularId = u.circular_id,
                            CircularTitle = u.circular_title,
                            CircularDescription = u.circular_description,
                            FileName = u.file_name,
                            FilePath = "https://mastermind.org.in" + u.file_path,
                            Count = count,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        })
                        .Skip(model.start)
                        .Take(model.length)
                        .ToList();
            return data;
        }

        public async Task<CircularEntity> GetCircularById(long circularID)
        {
            var data = (from u in this.context.CIRCULAR_MASTER
                        orderby u.circular_id descending
                        where u.row_sta_cd == 1 && u.circular_id == circularID
                        select new CircularEntity()
                        {
                            RowStatus = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd,
                                RowStatusText = u.row_sta_cd == 1 ? "Active" : "Inactive"
                            },
                            CircularId = u.circular_id,
                            CircularTitle = u.circular_title,
                            CircularDescription = u.circular_description,
                            FileName = u.file_name,
                            FilePath = u.file_path,
                            Transaction = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();
            return data;
        }

        public ResponseModel RemoveCircular(long circularId, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var data = (from u in this.context.CIRCULAR_MASTER
                            where u.circular_id == circularId
                            select u).FirstOrDefault();
                if (data != null)
                {
                    data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                    responseModel.Status = true;
                    responseModel.Message = "Circular Removed Successfully.";
                }
                else
                {
                    responseModel.Status = false;
                    responseModel.Message = "Circular Not Found.";
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
