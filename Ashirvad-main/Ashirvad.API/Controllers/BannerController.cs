using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Banner;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Ashirvad.API.Controllers
{
    [RoutePrefix("api/banner/v1")]
    [AshirvadAuthorization]
    public class BannerController : ApiController
    {
        private readonly IBannerService _bannerService = null;
        public BannerController(IBannerService bannerService)
        {
            this._bannerService = bannerService;
        }

        [Route("BannerMaintenance")]
        [HttpPost]
        public OperationResult<BannerEntity> BannerMaintenance(BannerEntity bannerInfo)
        {
            var data = this._bannerService.BannerMaintenance(bannerInfo);
            OperationResult<BannerEntity> result = new OperationResult<BannerEntity>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllBanner")]
        [HttpGet]
        public OperationResult<List<BannerEntity>> GetAllBanner()
        {
            var data = this._bannerService.GetAllBanner();
            OperationResult<List<BannerEntity>> result = new OperationResult<List<BannerEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllBannerByBranchAndType")]
        [HttpGet]
        public OperationResult<List<BannerEntity>> GetAllBanner(long branchID, int bannerTypeID)
        {
            var data = this._bannerService.GetAllBanner(branchID, bannerTypeID);
            return data.Result;
        }

        [Route("GetAllBannerByBranch")]
        [HttpGet]
        public OperationResult<List<BannerEntity>> GetAllBanner(long branchID)
        {
            var data = this._bannerService.GetAllBanner(branchID);
            OperationResult<List<BannerEntity>> result = new OperationResult<List<BannerEntity>>();
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }


        [Route("GetBannerByID")]
        [HttpPost]
        public OperationResult<BannerEntity> GetBannerByID(long bannerID)
        {
            var data = this._bannerService.GetBannerByBannerID(bannerID);
            OperationResult<BannerEntity> result = new OperationResult<BannerEntity>();
            result = data.Result;
            return result;
        }

        [Route("RemoveBanner")]
        [HttpPost]
        public OperationResult<bool> RemoveBanner(long bannerID, string lastupdatedby)
        {
            var data = this._bannerService.RemoveBanner(bannerID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("BannerMaintenance")]
        [HttpPost]
        public OperationResult<BannerEntity> BannerMaintenance(string model,bool HasFile)
        {
            OperationResult<BannerEntity> result = new OperationResult<BannerEntity>();
            var httpRequest = HttpContext.Current.Request;            
            BannerEntity bannerEntity = new BannerEntity();
            bannerEntity.BranchInfo = new BranchEntity();
            bannerEntity.BannerType = new List<BannerTypeEntity>();
            var entity = JsonConvert.DeserializeObject<BannerEntity>(model);
            bannerEntity.BannerID = entity.BannerID;
            bannerEntity.BranchInfo.BranchID = entity.BranchInfo.BranchID;
            bannerEntity.BannerType = entity.BannerType;
            bannerEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            bannerEntity.Transaction = new TransactionEntity()
            {
                TransactionId = entity.Transaction.TransactionId,
                LastUpdateBy = entity.Transaction.LastUpdateBy,
                LastUpdateId = entity.Transaction.LastUpdateId,
                CreatedBy = entity.Transaction.CreatedBy,
                CreatedId = entity.Transaction.CreatedId
            };
            if (HasFile)
            {
                try
                {
                    if (httpRequest.Files.Count > 0)
                    {
                        foreach (string file in httpRequest.Files)
                        {
                            string fileName;
                            string extension;
                            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                            string UpdatedPath = currentDir.Replace("Ashirvad.API", "Ashirvad.Web");
                            var postedFile = httpRequest.Files[file];
                            string randomfilename = Common.Common.RandomString(20);
                            extension = Path.GetExtension(postedFile.FileName);
                            fileName = Path.GetFileName(postedFile.FileName);
                            string _Filepath = "/BannerImage/" + randomfilename + extension;
                            string _Filepath1 = "BannerImage/" + randomfilename + extension;
                            var filePath = HttpContext.Current.Server.MapPath("~/BannerImage/" + randomfilename + extension);
                            string _path = UpdatedPath + _Filepath1;
                            postedFile.SaveAs(_path);
                            bannerEntity.FileName = fileName;
                            bannerEntity.FilePath = _Filepath;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Completed = false;
                    result.Data = null;
                    result.Message = ex.ToString();
                }
            }
            else
            {
                bannerEntity.FileName = entity.FileName;
                bannerEntity.FilePath = entity.FilePath;
            }
            var data = this._bannerService.BannerMaintenance(bannerEntity).Result;
            result.Completed = false;
            result.Data = null;
            if (data.BannerID > 0)
            {
                result.Completed = true;
                result.Data = data;
                if (entity.BannerID > 0)
                {
                    result.Message = "Banner Updated Successfully.";
                }
                else
                {
                    result.Message = "Banner Created Successfully.";
                }
            }
            return result;
        }

        [Route("Test")]
        [HttpGet]
        public OperationResult<string> Test()
        {
            OperationResult<string> result = new OperationResult<string>();
            try
            {
                var data = AppDomain.CurrentDomain.BaseDirectory;
                result.Data = data;
            }
            catch(Exception ex)
            {
                result.Data = ex.Message;
            }    
            
            return result;
        }
    }
}
