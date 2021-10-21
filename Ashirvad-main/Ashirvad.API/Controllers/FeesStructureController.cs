using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/FeesStructure/v1")]
    public class FeesStructureController : ApiController
    {

        private readonly IFeesService _FeesService;
        public FeesStructureController(IFeesService FeesService)
        {
            _FeesService = FeesService;
        }
        // GET: Fees

        [Route("FeesMaintenance/{FeesID}/{FeesDetailsID}/{StandardID}/{BranchID}/{Remark}/{SubmitDate}/{CreateId}/{CreateBy}")]
        [HttpPost]
        public OperationResult<FeesEntity> FeesMaintenance(long FeesID,long FeesDetailsID, long StandardID, long BranchID,
            string Remark,long CreateId, string CreateBy)
        {
            OperationResult<FeesEntity> result = new OperationResult<FeesEntity>();
            var httpRequest = HttpContext.Current.Request;
            FeesEntity feesEntity = new FeesEntity();            
            FeesEntity data = new FeesEntity();            
            feesEntity.BranchInfo = new BranchEntity();
            feesEntity.standardInfo = new StandardEntity();         
            feesEntity.BranchInfo.BranchID = BranchID;
            feesEntity.standardInfo.StandardID = StandardID;
            feesEntity.FeesID = FeesID;
            feesEntity.FeesDetailID = FeesDetailsID;            
            feesEntity.Remark = Remark;            
            feesEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            feesEntity.Transaction = new TransactionEntity()
            {
                CreatedBy = CreateBy,
                CreatedId = CreateId,
                CreatedDate = DateTime.Now,
            };
            
            if (httpRequest.Files.Count > 0)
            {
                try
                {
                    foreach (string file in httpRequest.Files)
                    {
                        string fileName;
                        string extension;
                        var postedFile = httpRequest.Files[file];
                        string randomfilename = Common.Common.RandomString(20);
                        extension = Path.GetExtension(postedFile.FileName);
                        fileName = Path.GetFileName(postedFile.FileName);
                        string _Filepath = "~/FeesImage/" + randomfilename + extension;
                        var filePath = HttpContext.Current.Server.MapPath("~/FeesImage/" + randomfilename + extension);
                        postedFile.SaveAs(filePath);                        
                        feesEntity.FileName = fileName;
                        feesEntity.FilePath = _Filepath;
                        data = this._FeesService.FeesMaintenance(feesEntity).Result;
                    }
                    result.Completed = false;
                    result.Data = null;
                    if (data.FeesID > 0 || data.FeesDetailID > 0)
                    {
                        result.Completed = true;
                        result.Data = data;
                        if (FeesID > 0)
                        {
                            result.Message = "Fees Structure Updated Successfully";
                        }
                        else
                        {
                            result.Message = "Fees Structure Created Successfully";
                        }
                    }


                }
                catch (Exception ex)
                {

                }
            }
            return result;
        }

        [Route("GetFeesByID")]
        [HttpPost]
        public OperationResult<FeesEntity> GetFeesByID(long FeesID)
        {
            var data = this._FeesService.GetFeesByFeesID(FeesID);
            OperationResult<FeesEntity> result = new OperationResult<FeesEntity>();
            result.Data = data.Result;
            return result;
        }

        [Route("RemoveFees")]
        [HttpPost]
        public OperationResult<bool> RemoveFees(long FeesID, string lastupdatedby)
        {
            var result = _FeesService.RemoveFees(FeesID, lastupdatedby);
            OperationResult<bool> response = new OperationResult<bool>();
            response.Completed = result;
            response.Data = result;
            return response;
        }
        [Route("GetAllFees")]
        [HttpPost]
        public OperationResult<List<FeesEntity>> GetAllFees()
        {
            var FeesData = this. _FeesService.GetAllFees();
            OperationResult<List<FeesEntity>> result = new OperationResult<List<FeesEntity>>();
            result.Data = FeesData.Result;
            return result;
        }


        [Route("GetFeesByBranchID")]
        [HttpPost]
        public OperationResult<List<FeesEntity>> GetFeesByBranchID(long BranchID,long StdID)
        {
            var data = this._FeesService.GetFeesByBranchID(BranchID,StdID);
            OperationResult<List<FeesEntity>> result = new OperationResult<List<FeesEntity>>();
            result.Data = data.Result;
            return result;
        }
    }
}
