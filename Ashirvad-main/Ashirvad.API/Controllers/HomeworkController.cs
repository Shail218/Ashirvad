using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Homework;
using Grpc.Core;
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

        [Route("GetAllHomeworkByBranchStudent")]
        [HttpGet]
        public OperationResult<List<HomeworkEntity>> GetAllHomeworkByBranchStudent(long branchID, long stdID, int batchTime, long studentId)
        {
            var data = this._homeworkService.GetAllHomeworkByBranchStudent(branchID, stdID, batchTime, studentId);
            OperationResult<List<HomeworkEntity>> result = new OperationResult<List<HomeworkEntity>>();
            result.Data = data.Result;
            result.Completed = true;
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

        [Route("HomeworkMaintenance/{HomeworkID}/{Homework_Date}/{BranchID}/{StandardID}/{SubjectID}/{Batch_TimeID}/{Remark}/{CreateId}/{CreateBy}/{TransactionId}/{FileName}/{Extension}/{HasFile}")]
        [HttpPost]
        public OperationResult<HomeworkEntity> HomeworkMaintenance(long HomeworkID, DateTime Homework_Date, long BranchID, long StandardID,long SubjectID,int Batch_TimeID,
            string Remark, long CreateId, string CreateBy, long TransactionId, string FileName, string Extension, bool HasFile)
        {
            OperationResult<HomeworkEntity> result = new OperationResult<HomeworkEntity>();
            var httpRequest = HttpContext.Current.Request;
            HomeworkEntity homeworkEntity = new HomeworkEntity();
            HomeworkEntity data = new HomeworkEntity();
            homeworkEntity.BranchInfo = new BranchEntity();
            homeworkEntity.StandardInfo = new StandardEntity();
            homeworkEntity.SubjectInfo = new SubjectEntity();
            homeworkEntity.HomeworkID = HomeworkID;
            homeworkEntity.HomeworkDate = Homework_Date;
            homeworkEntity.BranchInfo.BranchID = BranchID;
            homeworkEntity.StandardInfo.StandardID = StandardID;
            homeworkEntity.SubjectInfo.SubjectID = SubjectID;
            homeworkEntity.BatchTimeID = Batch_TimeID;
            homeworkEntity.Remarks = Remark;
            homeworkEntity.HomeworkContentFileName = FileName;
            homeworkEntity.FilePath = "/HomeworkDocument/" + FileName + "." + Extension;
            homeworkEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            homeworkEntity.Transaction = new TransactionEntity()
            {
                TransactionId = TransactionId,
                LastUpdateBy = CreateBy,
                LastUpdateId = CreateId,
                CreatedBy = CreateBy,
                CreatedId = CreateId,
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
                            // for live server
                            //string UpdatedPath = currentDir.Replace("AshirvadAPI", "ashivadproduct");
                            // for local server
                            string UpdatedPath = currentDir.Replace("Ashirvad.API", "Ashirvad.Web");
                            var postedFile = httpRequest.Files[file];
                            string randomfilename = Common.Common.RandomString(20);
                            extension = Path.GetExtension(postedFile.FileName);
                            fileName = Path.GetFileName(postedFile.FileName);
                            string _Filepath = "/HomeworkDocument/" + randomfilename + extension;
                            string _Filepath1 = "HomeworkDocument/" + randomfilename + extension;
                            var filePath = HttpContext.Current.Server.MapPath("~/HomeworkDocument/" + randomfilename + extension);
                            string _path = UpdatedPath + _Filepath1;
                            postedFile.SaveAs(_path);
                            homeworkEntity.HomeworkContentFileName = fileName;
                            homeworkEntity.FilePath = _Filepath;
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
            data = this._homeworkService.HomeworkMaintenance(homeworkEntity).Result;
            result.Completed = false;
            result.Data = null;
            if (data.HomeworkID > 0)
            {
                result.Completed = true;
                result.Data = data;
                if (HomeworkID > 0)
                {
                    result.Message = "Homework Updated Successfully";
                }
                else
                {
                    result.Message = "Homework Created Successfully";
                }
            }
            else
            {
                result.Message = "Homework Already Exists!!";
            }
            return result;
        }

        [HttpPost]
        [Route("HomeworkDetailMaintenance/{HomeworkID}/{BranchID}/{StudentID}/{Remarks}/{Status}/{SubmitDate}/{CreateId}/{CreateBy}")]
        public OperationResult<HomeworkDetailEntity> HomeworkDetailMaintenance(long HomeworkID, long BranchID, long StudentID, 
            string Remarks, int?Status, DateTime SubmitDate,long CreateId,string CreateBy)
        {
            HomeworkDetailEntity homeworkDetail = new HomeworkDetailEntity();
            HomeworkDetailEntity Response = new HomeworkDetailEntity();
           
            homeworkDetail.HomeworkEntity = new HomeworkEntity();
            homeworkDetail.BranchInfo = new BranchEntity();
            homeworkDetail.StudentInfo = new StudentEntity();
            var httpRequest = HttpContext.Current.Request;
            homeworkDetail.HomeworkEntity.HomeworkID = HomeworkID;
            homeworkDetail.BranchInfo.BranchID = BranchID;
            homeworkDetail.StudentInfo.StudentID = StudentID;
            homeworkDetail.Remarks = "";
            homeworkDetail.Status = Status.HasValue?Status.Value:0;
            homeworkDetail.SubmitDate = SubmitDate;
            homeworkDetail.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            homeworkDetail.Transaction = new TransactionEntity()
            {
                CreatedBy = CreateBy,
                CreatedId = CreateId,
                CreatedDate = DateTime.Now,
            };
            var data1 = this._homeworkdetailService.RemoveHomeworkdetail(HomeworkID,StudentID);
            OperationResult<HomeworkDetailEntity> result = new OperationResult<HomeworkDetailEntity>();
            if (httpRequest.Files.Count > 0)
            {
                try
                {
                    for (int file = 0; file < httpRequest.Files.Count; file++)
                    {
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
                        homeworkDetail.FilePath = _Filepath;                        
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

        [Route("GetStudentHomeworkChecking")]
        [HttpGet]
        public OperationResult<List<HomeworkEntity>> GetStudentHomeworkChecking(long hwID)
        {
            var data = this._homeworkService.GetStudentHomeworkChecking(hwID);
            OperationResult<List<HomeworkEntity>> result = new OperationResult<List<HomeworkEntity>>();
            result.Data = data.Result;
            result.Completed = true;
            return result;
        }

        [Route("Test/{HomeworkID}")]
        [HttpPost]
        public OperationResult<HomeworkDetailEntity> Test(List<long> HomeworkID)
        {
            OperationResult<HomeworkDetailEntity> result = new OperationResult<HomeworkDetailEntity>();
            return result;
        }
    }
}
