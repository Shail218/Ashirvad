using Ashirvad.API.Filter;
using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Homework;
using Grpc.Core;
using Ionic.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.UI;

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
            result.Completed = data.Result.Status;
            if (data.Result.Status)
            {
                result.Data = (HomeworkEntity)data.Result.Data;
            }
            result.Message = data.Result.Message;
            return result;
        }

        [Route("GetAllHomeworkByBranchStudent")]
        [HttpGet]
        public OperationResult<List<HomeworkEntity>> GetAllHomeworkByBranchStudent(long branchID,long courseid, long stdID, int batchTime, long studentId)
        {
            var data = this._homeworkService.GetAllHomeworkByBranchStudent(branchID,courseid, stdID, batchTime, studentId);
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
            result.Completed = data.Status;
            result.Data = data.Status;
            result.Message = data.Message;
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

        [Route("HomeworkMaintenance")]
        [HttpPost]
        public OperationResult<HomeworkEntity> HomeworkMaintenance(string model,bool HasFile)
        {
            OperationResult<HomeworkEntity> result = new OperationResult<HomeworkEntity>();
            var httpRequest = HttpContext.Current.Request;            
            HomeworkEntity homeworkEntity = new HomeworkEntity();
            homeworkEntity.BranchInfo = new BranchEntity();
            homeworkEntity.BranchCourse = new BranchCourseEntity();
            homeworkEntity.BranchClass = new BranchClassEntity();
            homeworkEntity.BranchSubject = new BranchSubjectEntity();
            var homeworkentity = JsonConvert.DeserializeObject<HomeworkEntity>(model);
            homeworkEntity.HomeworkID = homeworkentity.HomeworkID;
            homeworkEntity.HomeworkDate = homeworkentity.HomeworkDate;
            homeworkEntity.BranchInfo.BranchID = homeworkentity.BranchInfo.BranchID;
            homeworkEntity.BranchCourse.course_dtl_id = homeworkentity.BranchCourse.course_dtl_id;
            homeworkEntity.BranchClass.Class_dtl_id = homeworkentity.BranchClass.Class_dtl_id;
            homeworkEntity.BranchSubject.Subject_dtl_id = homeworkentity.BranchSubject.Subject_dtl_id;
            homeworkEntity.BatchTimeID = homeworkentity.BatchTimeID;
            homeworkEntity.Remarks = homeworkentity.Remarks;       
            homeworkEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            homeworkEntity.Transaction = new TransactionEntity()
            {
                TransactionId = homeworkentity.Transaction.TransactionId,
                LastUpdateBy = homeworkentity.Transaction.LastUpdateBy,
                LastUpdateId = homeworkentity.Transaction.LastUpdateId,
                CreatedBy = homeworkentity.Transaction.CreatedBy,
                CreatedId = homeworkentity.Transaction.CreatedId
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
                            string UpdatedPath = currentDir.Replace("WebAPI", "wwwroot");
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
            else
            {
                homeworkEntity.HomeworkContentFileName = homeworkentity.HomeworkContentFileName;
                homeworkEntity.FilePath = homeworkentity.FilePath;
            }
            var data = this._homeworkService.HomeworkMaintenance(homeworkEntity).Result;
            result.Completed = data.Status;
            if (data.Status && data.Data != null)
            {
                result.Data = (HomeworkEntity)data.Data;
            }
            result.Message = data.Message;
            return result;
        }

        public static string Decode(string Path)
        {
            byte[] mybyte = Convert.FromBase64String(Path);
            string returntext = Encoding.UTF8.GetString(mybyte);
            return returntext;
        }

        [HttpPost]
        [Route("HomeworkDetailMaintenance/{HomeworkID}/{BranchID}/{StudentID}/{Remarks}/{Status}/{SubmitDate}/{CreateId}/{CreateBy}")]
        public OperationResult<HomeworkDetailEntity> HomeworkDetailMaintenance(long HomeworkID, long BranchID, long StudentID,
            string Remarks, int? Status, DateTime SubmitDate, long CreateId, string CreateBy)
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
            homeworkDetail.Status = Status.HasValue ? Status.Value : 0;
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
            ResponseModel responseModel = new ResponseModel();
            var data1 = this._homeworkdetailService.RemoveHomeworkdetail(HomeworkID, StudentID);
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
                        //string UpdatedPath = currentDir.Replace("mastermindapi", "mastermind");
                        // for local server
                        string UpdatedPath = currentDir.Replace("WebAPI", "wwwroot");
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
                        responseModel = data.Result;
                    }
                   
                    result.Completed = responseModel.Status;
                    result.Message = responseModel.Message;
                    if (responseModel.Status && responseModel.Data != null)
                    {
                        result.Data = (HomeworkDetailEntity)responseModel.Data;
                    }
                   

                }
                catch (Exception ex)
                {
                    result.Completed = false;
                    result.Message = ex.Message.ToString();
                }

            }
            else
            {
                var data = this._homeworkdetailService.HomeworkdetailMaintenance(homeworkDetail);
                responseModel = data.Result;
                result.Completed = responseModel.Status;
                result.Message = responseModel.Message;
                if (responseModel.Status && responseModel.Data != null)
                {
                    result.Data = (HomeworkDetailEntity)responseModel.Data;
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

        [Route("Updatehomeworkdetails")]
        [HttpGet]
        public OperationResult<HomeworkDetailEntity> Updatehomeworkdetails(long HomeworkID, long StudentID, string Remark, int Status, string CreatedBy, long CreatedId)
        {
            OperationResult<HomeworkDetailEntity> result = new OperationResult<HomeworkDetailEntity>();
            HomeworkDetailEntity homeworkDetail = new HomeworkDetailEntity();
            homeworkDetail.HomeworkEntity = new HomeworkEntity();
            homeworkDetail.HomeworkEntity.StudentInfo = new StudentEntity();
            homeworkDetail.HomeworkEntity.HomeworkID = HomeworkID;
            homeworkDetail.HomeworkEntity.StudentInfo.StudentID = StudentID;
            homeworkDetail.Remarks = Remark;
            homeworkDetail.Status = Status;
            homeworkDetail.Transaction = new TransactionEntity()
            {
                CreatedBy = CreatedBy,
                CreatedId = CreatedId,
            };
            var result1 = _homeworkdetailService.Homeworkdetailupdate(homeworkDetail).Result;
            result.Completed = result1.Status;
            result.Message = result1.Message;
            if (result1.Status)
            {
                result.Data = (HomeworkDetailEntity)result1.Data;
            }
            return result;
        }

        [HttpGet]
        [Route("DownloadZipFile/{HomeworkID}/{StudentID}/{Homework}/{Student}/{Class}")]
        public OperationResult<HomeworkEntity> SaveZipFile(long HomeworkID, long StudentID, string Homework, string Student, string Class)
        {
            //hi = 11;
            //si = 2;
            OperationResult<HomeworkEntity> result = new OperationResult<HomeworkEntity>();
            string[] array = new string[2];
            string FileName = "";
            try
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                var homeworks = _homeworkService.GetHomeworkdetailsFiles(HomeworkID).Result;
                //string randomfilename = Common.Common.RandomString(20);
                string randomfilename = "HomeWork_" + Homework + "_Student_" + Student + "_Class_" + Class;
                FileName = "/ZipFiles/HomeworkDetails/" + randomfilename + ".zip";
                if (homeworks.Count > 0)
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath
                                   ("~/ZipFiles/HomeworkDetails/" + randomfilename + ".zip")))
                    {
                        File.Delete(HttpContext.Current.Server.MapPath
                                      ("~/ZipFiles/HomeworkDetails/" + randomfilename + ".zip"));
                    }

                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                        foreach (var item in homeworks)
                        {
                            //string filePath = HttpContext.Current.Server.MapPath(item.FilePath);
                            //zip.AddFile(filePath, "Files");
                            using (var client = new WebClient())
                            {
                                var buffer = client.DownloadData("https://mastermind.org.in" + item.FilePath);
                                zip.AddEntry(item.HomeworkContentFileName, buffer);
                            }
                        }

                        string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                        // for live server
                        //string UpdatedPath = currentDir.Replace("mastermindapi", "mastermind");
                        // for local server
                        string UpdatedPath = currentDir.Replace("WebAPI", "wwwroot");
                        //Save the Zip File to MemoryStream.
                        string _Filepath1 = "ZipFiles/HomeworkDetails/" + randomfilename + ".zip";
                        var filePath = HttpContext.Current.Server.MapPath("~/ZipFiles/HomeworkDetails/" + randomfilename + ".zip");
                        string _path = UpdatedPath + _Filepath1;
                        zip.Save(_path);
                    }

                    result.Data = new HomeworkEntity()
                    {
                        FilePath = "https://mastermind.org.in" + FileName
                    };
                    result.Completed = true;
                    result.Message = "Success";
                }
                else
                {
                    result.Completed = false;
                    result.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                result.Completed = false;
                result.Message = ex.ToString();
            }
            return result;
        }
    }
}
