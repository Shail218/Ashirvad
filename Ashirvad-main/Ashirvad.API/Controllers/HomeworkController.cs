using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Homework;
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
    [RoutePrefix("api/homework/v1")]
    [AshirvadAuthorization]
    public class HomeworkController : ApiController
    {
        private readonly IHomeworkService _homeworkService = null;
        private readonly IHomeworkDetailService _homeworkdetailService = null;
        public HomeworkController(IHomeworkService homeworkService, IHomeworkDetailService homeworkdetailService)
        {
            this._homeworkService = homeworkService;
            this._homeworkdetailService = homeworkdetailService;
        }


        [Route("HomeworkMaintenance")]
        [HttpPost]
        public OperationResult<HomeworkEntity> HomeworkMaintenance(HomeworkEntity homework)
        {
            OperationResult<HomeworkEntity> result = new OperationResult<HomeworkEntity>();

            var data = this._homeworkService.HomeworkMaintenance(homework);
            result.Completed = true;
            result.Data = data.Result;
            return result;
        }

        [Route("GetAllHomeworkByBranch")]
        [HttpGet]
        public OperationResult<List<HomeworkEntity>> GetAllHomeworkByBranch(long branchID)
        {
            var data = this._homeworkService.GetAllHomeworkByBranch(branchID);
            OperationResult<List<HomeworkEntity>> result = new OperationResult<List<HomeworkEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetAllHomeworkByBranchAndStd")]
        [HttpGet]
        public OperationResult<List<HomeworkEntity>> GetAllHomeworkByBranch(long branchID, long stdID, int batchTime)
        {
            var data = this._homeworkService.GetAllHomeworkByBranch(branchID, stdID, batchTime);
            OperationResult<List<HomeworkEntity>> result = new OperationResult<List<HomeworkEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetAllHomeworkWithoutContentByBranch")]
        [HttpGet]
        public OperationResult<List<HomeworkEntity>> GetAllHomeworkWithoutContentByBranch(long branchID)
        {
            var data = this._homeworkService.GetAllHomeworkWithoutContentByBranch(branchID);
            OperationResult<List<HomeworkEntity>> result = new OperationResult<List<HomeworkEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetAllHomeworkWithoutContentByBranchSTD")]
        [HttpGet]
        public OperationResult<List<HomeworkEntity>> GetAllHomeworkWithoutContentByBranch(long branchID, long stdID)
        {
            var data = this._homeworkService.GetAllHomeworkWithoutContentByBranch(branchID, stdID);
            OperationResult<List<HomeworkEntity>> result = new OperationResult<List<HomeworkEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetAllHomeworks")]
        [HttpGet]
        public OperationResult<List<HomeworkEntity>> GetAllHomeworks(DateTime hwDate, string searchParam)
        {
            var data = this._homeworkService.GetAllHomeworks(hwDate, searchParam);
            OperationResult<List<HomeworkEntity>> result = new OperationResult<List<HomeworkEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("GetHomeworkByHWID")]
        [HttpGet]
        public OperationResult<HomeworkEntity> GetHomeworkByHWID(long hwID)
        {
            var data = this._homeworkService.GetHomeworkByHomeworkID(hwID);
            OperationResult<HomeworkEntity> result = new OperationResult<HomeworkEntity>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }


        [Route("RemoveHomework")]
        [HttpPost]
        public OperationResult<bool> RemoveHomework(long hwID, string lastupdatedby)
        {
            var data = this._homeworkService.RemoveHomework(hwID, lastupdatedby);
            OperationResult<bool> result = new OperationResult<bool>();
            result.Completed = true;
            result.Data = data;
            return result;
        }

        [Route("GetAllHomeworks")]
        [HttpGet]
        public OperationResult<List<HomeworkDetailEntity>> GetAllHomeworks(long StudID)
        {
            var data = this._homeworkdetailService.GetAllHomeworkdetailByHomeWork(StudID);
            OperationResult<List<HomeworkDetailEntity>> result = new OperationResult<List<HomeworkDetailEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }
        
        [HttpPost]
        [Route("HomeworkDetailMaintenance/{HomeworkID}/{HomeworkDetailID}/{BranchID}/{StudentID}/{Remarks}/{Status}/{SubmitDate}/{CreateId}/{CreateBy}/{FileName}/{Filepath}")]
        public OperationResult<HomeworkDetailEntity> HomeworkDetailMaintenance(long HomeworkID, long HomeworkDetailID, long BranchID, long StudentID, 
            string Remarks, int?Status, DateTime SubmitDate,long CreateId,string CreateBy,string FileName,string Filepath)
        {
            HomeworkDetailEntity homeworkDetail = new HomeworkDetailEntity();
            HomeworkDetailEntity Response = new HomeworkDetailEntity();
           
            homeworkDetail.HomeworkEntity = new HomeworkEntity();
            homeworkDetail.BranchInfo = new BranchEntity();
            homeworkDetail.StudentInfo = new StudentEntity();
            var httpRequest = HttpContext.Current.Request;
            homeworkDetail.HomeworkEntity.HomeworkID = HomeworkID;
            homeworkDetail.HomeworkDetailID = HomeworkDetailID;
            homeworkDetail.BranchInfo.BranchID = BranchID;
            homeworkDetail.StudentInfo.StudentID = StudentID;
            homeworkDetail.Remarks = "";
            homeworkDetail.Status = Status.HasValue?Status.Value:0;
            homeworkDetail.SubmitDate = SubmitDate;
            homeworkDetail.AnswerSheetContentText = FileName;
            homeworkDetail.FilePath = Filepath;
            homeworkDetail.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            if (HomeworkID > 0)
            {
                homeworkDetail.Transaction = new TransactionEntity()
                {
                    LastUpdateBy = CreateBy,
                    LastUpdateId = CreateId,
                    LastUpdateDate = DateTime.Now,
                };
            }
            else
            {
                homeworkDetail.Transaction = new TransactionEntity()
                {
                    CreatedBy = CreateBy,
                    CreatedId = CreateId,
                    CreatedDate = DateTime.Now,
                };
            }

            homeworkDetail.Transaction = new TransactionEntity()
            {
                CreatedBy = CreateBy,
                CreatedId = CreateId,
                CreatedDate = DateTime.Now,
            };
            OperationResult<HomeworkDetailEntity> result = new OperationResult<HomeworkDetailEntity>();
            if (httpRequest.Files.Count > 0)
            {
                try
                {
                    foreach (string file in httpRequest.Files)
                    {
                        homeworkDetail.HomeworkDetailID = HomeworkDetailID;
                        string fileName;
                        string extension;
                        string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                        // for live server
                        //string UpdatedPath = currentDir.Replace("AshirvadAPI", "ashivadproduct");
                        // for local server
                        string UpdatedPath = currentDir.Replace("Ashirvad.API", "Ashirvad.Web");
                        var postedFile = httpRequest.Files[file];
                        string randomfilename = Common.Common.RandomString(20);
                        extension = Path.GetExtension(postedFile.FileName);
                        fileName = Path.GetFileName(postedFile.FileName);
                        string _Filepath = "/HomeWorkDetailImage/" + randomfilename + extension;
                        string _Filepath1 = "HomeWorkDetailImage/" + randomfilename + extension;
                        var filePath = HttpContext.Current.Server.MapPath("~/HomeWorkDetailImage/" + randomfilename + extension);
                        string _path = UpdatedPath + _Filepath1;
                        postedFile.SaveAs(_path);
                        homeworkDetail.AnswerSheetName = fileName;
                        homeworkDetail.FilePath = _Filepath1;                        
                        var data = this._homeworkdetailService.HomeworkdetailMaintenance(homeworkDetail);
                        Response = data.Result;
                    }
                    result.Data = null;
                    result.Completed = false;
                    if (Response.HomeworkDetailID > 0)
                    {
                        result.Data = Response;
                        result.Completed = true;
                        result.Message = "Homework Uploaded Successfully!!";
                    }

                }
                catch (Exception ex)
                {

                }

            }
            else
            {
                var data = this._homeworkdetailService.HomeworkdetailMaintenance(homeworkDetail);
                Response = data.Result;
                result.Data = null;
                result.Completed = false;
                if (Response.HomeworkDetailID > 0)
                {
                    result.Data = Response;
                    result.Completed = true;
                    result.Message = "Homework Uploaded Successfully!!";
                }
            }
            return result;
        }
    }
}
