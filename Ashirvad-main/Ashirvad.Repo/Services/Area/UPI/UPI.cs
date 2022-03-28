using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.DataAcceessAPI.Area.UPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Repo.Services.Area.UPI
{
    public class UPI : ModelAccess, IUPIAPI
    {

        public async Task<long> CheckUpi(long upiID, long BranchID, string upicode)
        {
            long result;
            bool isExists =(from u in  this.context.UPI_MASTER where((upiID == 0 || u.unique_id != upiID)
                            && u.branch_id == BranchID && u.upi_code == upicode && u.row_sta_cd == 1)select u).FirstOrDefault() != null;
            result = isExists == true ? -1 : 1;
            return result;
        }

        public async Task<ResponseModel> UPIMaintenance(UPIEntity upiInfo)
        {
            ResponseModel responseModel = new ResponseModel();
            Model.UPI_MASTER upiMaster = new Model.UPI_MASTER();
            try 
            {
                if (CheckUpi(upiInfo.UPIId, upiInfo.BranchData.BranchID, upiInfo.UPICode).Result != -1)
                {
                    bool isUpdate = true;
                    var data = (from upi in this.context.UPI_MASTER
                                where upi.unique_id == upiInfo.UPIId
                                select upi).FirstOrDefault();
                    if (data == null)
                    {
                        data = new Model.UPI_MASTER();
                        isUpdate = false;
                    }
                    else
                    {
                        upiMaster = data;
                        upiInfo.TransactionData.TransactionId = data.trans_id;
                    }

                    upiMaster.row_sta_cd = upiInfo.RowStatusData.RowStatusId;
                    upiMaster.trans_id = this.AddTransactionData(upiInfo.TransactionData);
                    upiMaster.branch_id = upiInfo.BranchData.BranchID;
                    upiMaster.upi_code = upiInfo.UPICode;
                    if (!isUpdate)
                    {
                        this.context.UPI_MASTER.Add(upiMaster);
                    }

                    var upiID = this.context.SaveChanges() > 0 ? upiMaster.unique_id : 0;
                    if (upiID > 0)
                    {
                        upiInfo.UPIId = upiID;
                        responseModel.Data = upiInfo;
                        responseModel.Message = isUpdate == true ? "UPI-ID Updated Successfully." : "UPI-ID Inserted Successfully.";
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.Message = isUpdate == true ? "UPI-ID Not Updated." : "UPI-ID Not Inserted.";
                        responseModel.Status = false;
                    }
                }
            }
            catch(Exception ex)
            {
                responseModel.Message = ex.Message.ToString();
                responseModel.Status = false;
            }
          
            return responseModel;
        }

        public async Task<List<UPIEntity>> GetAllUPIs(long branchID)
        {
            var data = (from u in this.context.UPI_MASTER
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id orderby u.unique_id descending
                        where (0 == branchID || u.branch_id == branchID) && u.row_sta_cd == 1
                        select new UPIEntity()
                        {
                            RowStatusData = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            UPICode = u.upi_code,
                            UPIId = u.unique_id,
                            BranchData = new BranchEntity() { BranchID = b.branch_id, BranchName = b.branch_name },
                            TransactionData = new TransactionEntity() { TransactionId = u.trans_id }
                        }).ToList();

            return data;
        }

        public async Task<UPIEntity> GetUPIByID(long upiID)
        {
            var data = (from u in this.context.UPI_MASTER
                        join b in this.context.BRANCH_MASTER on u.branch_id equals b.branch_id
                        where u.unique_id == upiID
                        select new UPIEntity()
                        {
                            RowStatusData = new RowStatusEntity()
                            {
                                RowStatus = u.row_sta_cd == 1 ? Enums.RowStatus.Active : Enums.RowStatus.Inactive,
                                RowStatusId = (int)u.row_sta_cd
                            },
                            UPICode = u.upi_code,
                            UPIId = u.unique_id,
                            BranchData = new BranchEntity() { BranchID = b.branch_id, BranchName = b.branch_name },
                            TransactionData = new TransactionEntity() { TransactionId = u.trans_id }
                        }).FirstOrDefault();
            return data;
        }

        public ResponseModel RemoveUPI(long upiID, string lastupdatedby)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var data = (from u in this.context.UPI_MASTER
                            where u.unique_id == upiID
                            select u).FirstOrDefault();
                if (data != null)
                {
                    data.row_sta_cd = (int)Enums.RowStatus.Inactive;
                    data.trans_id = this.AddTransactionData(new TransactionEntity() { TransactionId = data.trans_id, LastUpdateBy = lastupdatedby });
                    this.context.SaveChanges();
                    responseModel.Message = "UPI-ID Removed Successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.Message = "UPI-ID Not Found.";
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
    }
}
