using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Homework;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class HomeWorkController : BaseController
    {
        private readonly IHomeworkService _homeworkService = null;
        private readonly IHomeworkDetailService _homeworkdetailService = null;
        ResponseModel response = new ResponseModel();
        public HomeWorkController(IHomeworkService homeworkService, IHomeworkDetailService homeworkdetailService)
        {
            _homeworkService = homeworkService;
            _homeworkdetailService = homeworkdetailService;
        }


        // GET: HomeWork
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> HomeworkMaintenance(long homeworkID, long branchID)
        {
            HomeworkMaintenanceModel branch = new HomeworkMaintenanceModel();
            if (homeworkID > 0)
            {
                var homework = await _homeworkService.GetHomeworkByHomeworkID(homeworkID);
                branch.HomeworkInfo = homework;
            }

            if (branchID > 0)
            {
                var homeworkData = await _homeworkService.GetAllHomeworkWithoutContentByBranch(branchID);
                branch.HomeworkData = homeworkData;
            }
            else
            {
                var homeworkData = await _homeworkService.GetAllHomeworkWithoutContentByBranch(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
                branch.HomeworkData = homeworkData;
            }

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveHomework(HomeworkEntity homeworkEntity)
        {
            if (homeworkEntity.FileInfo != null)
            {
                //photos.FileInfo = Common.Common.ReadFully(photos.ImageFile.InputStream);
                string _FileName = Path.GetFileName(homeworkEntity.FileInfo.FileName);
                string extension = System.IO.Path.GetExtension(homeworkEntity.FileInfo.FileName);
                string randomfilename = Common.Common.RandomString(20);
                string _Filepath = "/HomeworkDocument/" + randomfilename + extension;
                string _path = Path.Combine(Server.MapPath("~/HomeworkDocument"), randomfilename + extension);
                homeworkEntity.FileInfo.SaveAs(_path);
                homeworkEntity.HomeworkContentFileName = _FileName;
                homeworkEntity.FilePath = _Filepath;
            }

            homeworkEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            homeworkEntity.Transaction = GetTransactionData(homeworkEntity.HomeworkID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            var data = await _homeworkService.HomeworkMaintenance(homeworkEntity);
            if (data.HomeworkID>0)
            {
                response.Status = true;
                response.Message = "Homework Created Successfully!!";
                
            }
            else if(data.HomeworkID < 0)
            {
                response.Status = false;
                response.Message = "Homework Already Exist!!";

            }
            else
            {
                response.Status = false;
                response.Message = "Homework Failed To Create!!";

            }
            return Json(response);
        }

        [HttpPost]
        public JsonResult RemoveHomework(long homeworkID)
        {
            var result = _homeworkService.RemoveHomework(homeworkID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        public async Task<JsonResult> Downloadhomework(long homeworkid)
        {
            string[] array = new string[4];
            try
            {
                var operationResult = await _homeworkService.GetHomeworkByHomeworkID(homeworkid);
                if (operationResult != null)
                {
                    string contentType = "";
                    string[] extarray = operationResult.HomeworkContentFileName.Split('.');
                    string ext = extarray[1];
                    switch (ext)
                    {
                        case "pdf":
                            contentType = "application/pdf";
                            break;
                        case "xlsx":
                            contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            break;
                        case "docx":
                            contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                            break;
                        case "png":
                            contentType = "image/png";
                            break;
                        case "jpg":
                            contentType = "image/jpeg";
                            break;
                        case "txt":
                            contentType = "application/text/plain";
                            break;
                        case "mp4":
                            contentType = "application/video";
                            break;
                        case "pptx":
                            contentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                            break;
                        case "zip":
                            contentType = "application/zip";
                            break;
                        case "rar":
                            contentType = "application/x-rar-compressed";
                            break;
                        case "xls":
                            contentType = "application/vnd.ms-excel";
                            break;
                    }
                    string file = operationResult.HomeworkContentText;
                    string filename = extarray[0];
                    array[0] = ext;
                    array[1] = file;
                    array[2] = filename;
                    array[3] = contentType;
                    return Json(array);
                }
            }
            catch (Exception ex)
            {

            }
            return Json(array);
        }

        public ActionResult StudentHomeworkDetails(long StudhID)
        {
           // var result = _homeworkdetailService.GetAllHomeworkdetailByHomeWork(StudhID);
            var result1 = _homeworkService.GetStudentHomeworkChecking(StudhID);

            
            return View(result1.Result);
        }
        public async Task<JsonResult> Updatehomeworkdetails(long HomeworkID,long StudentID,string Remark,int Status)
        {
            // var result = _homeworkdetailService.GetAllHomeworkdetailByHomeWork(StudhID);
            HomeworkDetailEntity homeworkDetail = new HomeworkDetailEntity();
            homeworkDetail.HomeworkEntity = new HomeworkEntity();
            homeworkDetail.StudentInfo = new StudentEntity();
            homeworkDetail.HomeworkEntity.HomeworkID = HomeworkID;
            homeworkDetail.StudentInfo.StudentID = StudentID;
            homeworkDetail.Remarks = Remark;
            homeworkDetail.Status = Status;
            homeworkDetail.Transaction = GetTransactionData(Common.Enums.TransactionType.Insert);
            var result1 = _homeworkdetailService.Homeworkdetailupdate(homeworkDetail).Result;
            if (result1.HomeworkDetailID > 0)
            {
                response.Status = true;
                response.Message = "Updated Successfully!!";
            }
            else
            {
                response.Status = false;
                response.Message = "Failed To Updated!!";
            }
            return Json(response);
        }

        
        public async Task<JsonResult> SaveZipFile(long homeworkid,long StudentID,DateTime Homework,string Student,string Class)
        {
            string[] array = new string[2];
            string FileName = "";
            try
            {

                var homeworks = _homeworkService.GetHomeworkdetailsFiles(homeworkid).Result;
                //string randomfilename = Common.Common.RandomString(20);
                string randomfilename = "HomeWork_"+ Homework.ToString("ddMMyyyy") + "_Student_"+ Student +"_Class_"+ Class;
                FileName = "/ZipFiles/HomeworkDetails/" + randomfilename + ".zip";
                if (homeworks.Count > 0)
                {
                   
                    string Ex = ".pdf";
                    if (System.IO.File.Exists(Server.MapPath
                                   ("~/ZipFiles/HomeworkDetails/" + randomfilename + ".zip")))
                    {
                        System.IO.File.Delete(Server.MapPath
                                      ("~/ZipFiles/HomeworkDetails/" + randomfilename + ".zip"));
                    }
                    ZipArchive zip = System.IO.Compression.ZipFile.Open(Server.MapPath
                             ("~/ZipFiles/HomeworkDetails/" + randomfilename + ".zip"), ZipArchiveMode.Create);

                    foreach (var item in homeworks)
                    {
                        zip.CreateEntryFromFile(Server.MapPath
                           ("~/" + item.FilePath), item.HomeworkContentFileName);

                    }
                    zip.Dispose();
                }
               

                
            }
            catch(Exception ex)
            {

            }

            HomeworkDetailEntity homeworkDetail = new HomeworkDetailEntity();
            homeworkDetail.HomeworkEntity = new HomeworkEntity();
            homeworkDetail.StudentInfo = new StudentEntity();
            homeworkDetail.HomeworkEntity.HomeworkID = homeworkid;            
            homeworkDetail.StudentInfo.StudentID = StudentID;            
            homeworkDetail.StudentFilePath = FileName;
            homeworkDetail.Transaction = GetTransactionData(Common.Enums.TransactionType.Insert);
            var result1 = _homeworkdetailService.HomeworkdetailFileupdate(homeworkDetail);
            return Json(FileName);

        }

    }
}