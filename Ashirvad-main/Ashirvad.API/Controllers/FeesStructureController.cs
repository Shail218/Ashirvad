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

        [Route("FeesMaintenance")]
        [HttpPost]
        public OperationResult<FeesEntity> FeesMaintenance(FeesEntity feesEntity)
        {

            var httpRequest = HttpContext.Current.Request;
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
                    }

                    

                }
                catch (Exception ex)
                {

                }
            }


          
            var data = this._FeesService.FeesMaintenance(feesEntity);
            OperationResult<FeesEntity> result = new OperationResult<FeesEntity>();
            result.Completed = true;
            result.Data = data.Result;
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
    }
}
