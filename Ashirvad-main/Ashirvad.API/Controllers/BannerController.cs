using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Banner;
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

        [Route("BannerMaintenance/{BannerID}/{BranchID}/{isAdmin}/{isTeacher}/{isStudent}/{CreateId}/{CreateBy}/{TransactionId}/{FileName}/{Extension}/{HasFile}")]
        [HttpPost]
        public OperationResult<BannerEntity> BannerMaintenance(long BannerID,long BranchID, bool isAdmin, bool isTeacher, bool isStudent,long CreateId, string CreateBy, long TransactionId, string FileName, string Extension, bool HasFile)
        {
            OperationResult<BannerEntity> result = new OperationResult<BannerEntity>();
            var httpRequest = HttpContext.Current.Request;            
            BannerEntity bannerEntity = new BannerEntity();
            BannerEntity data = new BannerEntity();
            bannerEntity.BranchInfo = new BranchEntity();
            bannerEntity.bannerTypeEntity = new BannerTypeEntity();
            bannerEntity.BannerType = new List<BannerTypeEntity>();
            bannerEntity.BannerID = BannerID;
            bannerEntity.BranchInfo.BranchID = BranchID;
            bannerEntity.FileName = FileName;
            bannerEntity.FilePath = "/BannerImage/" + FileName + "." + Extension;
            bannerEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            bannerEntity.Transaction = new TransactionEntity()
            {
                TransactionId = TransactionId,
                LastUpdateBy = CreateBy,
                LastUpdateId = CreateId,
                CreatedBy = CreateBy,
                CreatedId = CreateId,
            };
            if(isAdmin)
            {
                bannerEntity.bannerTypeEntity = new BannerTypeEntity();
                bannerEntity.bannerTypeEntity.TypeText = "Admin";
                bannerEntity.bannerTypeEntity.TypeID = 1;
                bannerEntity.BannerType.Add(bannerEntity.bannerTypeEntity);
            }
            if(isTeacher)
            {
                bannerEntity.bannerTypeEntity = new BannerTypeEntity();
                bannerEntity.bannerTypeEntity.TypeText = "Teacher";
                bannerEntity.bannerTypeEntity.TypeID = 2;
                bannerEntity.BannerType.Add(bannerEntity.bannerTypeEntity);
            }
            if(isStudent)
            {
                bannerEntity.bannerTypeEntity = new BannerTypeEntity();
                bannerEntity.bannerTypeEntity.TypeText = "Student";
                bannerEntity.bannerTypeEntity.TypeID = 3;
                bannerEntity.BannerType.Add(bannerEntity.bannerTypeEntity);
            }
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
                            // for live server
                            //string UpdatedPath = currentDir.Replace("mastermindapi", "mastermind");
                            // for local server
                            string UpdatedPath = currentDir.Replace("WebAPI", "wwwroot");
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
                string[] filename = FileName.Split(',');
                bannerEntity.FileName = filename[0];
                bannerEntity.FilePath = "/BannerImage/" + filename[1] + "." + Extension;
            }
            data = this._bannerService.BannerMaintenance(bannerEntity).Result;
            result.Completed = false;
            result.Data = null;
            if (data.BannerID > 0)
            {
                result.Completed = true;
                result.Data = data;
                if (BannerID > 0)
                {
                    result.Message = "Banner Updated Successfully";
                }
                else
                {
                    result.Message = "Banner Created Successfully";
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
