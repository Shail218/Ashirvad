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
using static Ashirvad.Common.Common;

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
            //var homeworkData = await _homeworkService.GetAllHomeworkWithoutContentByBranch(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branch.HomeworkData = new List<HomeworkEntity>();
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

        public ActionResult StudentHomeworkDetails(long StudhID)
        {
           // var result = _homeworkdetailService.GetAllHomeworkdetailByHomeWork(StudhID);
            var result1 = _homeworkService.GetStudentHomeworkChecking(StudhID);

            
            return View(result1.Result);
        }

        [HttpPost]
        public async Task<JsonResult> Updatehomeworkdetails(long HomeworkID,long StudentID,string Remark,int Status)
        {
            // var result = _homeworkdetailService.GetAllHomeworkdetailByHomeWork(StudhID);
            HomeworkDetailEntity homeworkDetail = new HomeworkDetailEntity();
            homeworkDetail.HomeworkEntity = new HomeworkEntity();
            homeworkDetail.HomeworkEntity.StudentInfo = new StudentEntity();
            homeworkDetail.HomeworkEntity.HomeworkID = HomeworkID;
            homeworkDetail.HomeworkEntity.StudentInfo.StudentID = StudentID;
            homeworkDetail.Remarks = Remark;
            homeworkDetail.Status = Status;
            homeworkDetail.Transaction = GetTransactionData(Common.Enums.TransactionType.Insert);
            var result1 = _homeworkdetailService.Homeworkdetailupdate(homeworkDetail).Result;
            if (result1.HomeworkEntity.HomeworkID > 0)
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
            string stdname = "";
            try
            {

                var homeworks = _homeworkService.GetHomeworkdetailsFiles(homeworkid).Result;
                //string randomfilename = Common.Common.RandomString(20);
                string studentname = Student.Replace(" ", "");
                stdname = Class.Replace(" ", "");
                if (stdname.Contains("."))
                {
                    stdname = stdname.Replace(".", "");
                }            
                string randomfilename = "HomeWork_"+ Homework.ToString("ddMMyyyy") + "_Student_"+ studentname +"_Class_"+ stdname;
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

        public async Task<JsonResult> CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            List<string> columns = new List<string>();
            columns.Add("HomeworkDate");
            foreach (var item in model.order)
            {
                item.name = columns[item.column];
            }
            var branchData = await _homeworkService.GetAllCustomHomework(model, SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            long total = 0;
            if (branchData.Count > 0)
            {
                total = branchData[0].Count;
            }
            return Json(new
            {
                draw = model.draw,
                iTotalRecords = total,
                iTotalDisplayRecords = total,
                data = branchData
            });

        }

    }
}