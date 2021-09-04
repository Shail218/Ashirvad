using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Homework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class HomeWorkController : BaseController
    {
        private readonly IHomeworkService _homeworkService = null;
        public HomeWorkController(IHomeworkService homeworkService)
        {
            _homeworkService = homeworkService;
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
            homeworkEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            homeworkEntity.Transaction = GetTransactionData(homeworkEntity.HomeworkID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            var data = await _homeworkService.HomeworkMaintenance(homeworkEntity);
            if (data != null)
            {
                return Json(true);
            }

            return Json(false);
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

    }
}