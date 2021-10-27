using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Repo.Services.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.Services.Area;
using Ashirvad.Uploads;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/FeesStructure/v1")]
    public class FeesStructureWebAPIController : ApiController
    {
        private readonly FileUploadCommon fileUploadCommon = new FileUploadCommon();
        private readonly IFeesService _FeesService;
        //public FeesStructureWebAPIController(IFeesService FeesService)
        //{
        //    _FeesService = FeesService;
        //}
        public FeesStructureWebAPIController()
        {
            _FeesService = new FeesService(new Fees());
        }
        // GET: Fees

        [Route("FeesMaintenance/{FeesID}/{FeesDetailsID}/{StandardID}/{BranchID}/{Remark}/{SubmitDate}/{CreateId}/{CreateBy}/{TransactionId}/{FileName}/{Extension}/{HasFile}")]
        [HttpPost]
        public OperationResult<FeesEntity> FeesMaintenance(long FeesID, long FeesDetailsID, long StandardID, long BranchID,
            string Remark, long CreateId, string CreateBy, long TransactionId, string FileName, string Extension, bool HasFile)
        {
            FileModel fileModel = new FileModel();
            OperationResult<FeesEntity> result = new OperationResult<FeesEntity>();
            FeesEntity feesEntity = new FeesEntity();
            FeesEntity data = new FeesEntity();
            if (FeesID == 0)
            {
                var httpRequest = HttpContext.Current.Request;
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
                            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                            string UpdatedPath = currentDir.Replace("ashirvadapi", "ashivadproduct");
                            var postedFile = httpRequest.Files[file];
                            string randomfilename = Common.Common.RandomString(20);
                            extension = Path.GetExtension(postedFile.FileName);
                            fileName = Path.GetFileName(postedFile.FileName);
                            string _Filepath = "/FeesImage/" + randomfilename + extension;
                            var filePath = HttpContext.Current.Server.MapPath("~/FeesImage/" + randomfilename + extension);
                            string _path = UpdatedPath + _Filepath;
                            postedFile.SaveAs(_path);
                            feesEntity.FileName = randomfilename + extension;
                            feesEntity.FilePath = _Filepath;
                            data = this._FeesService.FeesMaintenance(feesEntity).Result;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else
            {
                var httpRequest = HttpContext.Current.Request;
                feesEntity.BranchInfo = new BranchEntity();
                feesEntity.standardInfo = new StandardEntity();
                feesEntity.BranchInfo.BranchID = BranchID;
                feesEntity.standardInfo.StandardID = StandardID;
                feesEntity.FeesID = FeesID;
                feesEntity.FeesDetailID = FeesDetailsID;
                feesEntity.Remark = Remark;
                feesEntity.Transaction = new TransactionEntity()
                {
                    TransactionId = TransactionId,
                    LastUpdateBy = CreateBy,
                    LastUpdateId = CreateId
                };
                feesEntity.RowStatus = new RowStatusEntity()
                {
                    RowStatusId = (int)Enums.RowStatus.Active
                };
                feesEntity.FileName = FileName + "." + Extension;
                feesEntity.FilePath = "/FeesImage/" + FileName + "." + Extension;
                if (HasFile)
                {
                    if (httpRequest.Files.Count > 0)
                    {
                        try
                        {
                            foreach (string file in httpRequest.Files)
                            {
                                string fileName;
                                string extension;
                                string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                                string UpdatedPath = currentDir.Replace("ashirvadapi", "ashivadproduct");
                                var postedFile = httpRequest.Files[file];
                                string randomfilename = Common.Common.RandomString(20);
                                extension = Path.GetExtension(postedFile.FileName);
                                fileName = Path.GetFileName(postedFile.FileName);
                                string _Filepath = "/FeesImage/" + randomfilename + extension;
                                var filePath = HttpContext.Current.Server.MapPath("~/FeesImage/" + randomfilename + extension);
                                string _path = UpdatedPath + _Filepath;
                                postedFile.SaveAs(_path);
                                feesEntity.FileName = randomfilename + extension;
                                feesEntity.FilePath = _Filepath;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
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
            var FeesData = this._FeesService.GetAllFees();
            OperationResult<List<FeesEntity>> result = new OperationResult<List<FeesEntity>>();
            result.Data = FeesData.Result;
            return result;
        }


        [Route("GetFeesByBranchID")]
        [HttpGet]
        public OperationResult<List<FeesEntity>> GetFeesByBranchID(long BranchID, long StdID = 0)
        {
            var data = this._FeesService.GetFeesByBranchID(BranchID, StdID);
            OperationResult<List<FeesEntity>> result = new OperationResult<List<FeesEntity>>();
            result.Data = data.Result;
            return result;
        }
    }
}
