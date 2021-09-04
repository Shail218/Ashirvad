using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class TestPaperController : BaseController
    {
        private readonly ITestService _testService = null;
        public TestPaperController(ITestService testService)
        {
            _testService = testService;
        }

        // GET: TestPaper
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> TestPaperMaintenance(long testID, long paperID = 0)
        {
            TestMaintenanceModel branch = new TestMaintenanceModel();
            if (testID > 0)
            {
                var test = await _testService.GetTestByTestID(testID);
                branch.TestInfo = test.Data;

                var testpaper = await _testService.GetTestPaperByPaperID(paperID);
                branch.TestPaperInfo = testpaper;
            }

            var testpaperByBranch = await _testService.GetAllTestByBranch(SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branch.TestData = testpaperByBranch.Data;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveTest(TestEntity testEntity)
        {
            testEntity.Transaction = GetTransactionData(testEntity.TestID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            testEntity.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _testService.TestMaintenance(testEntity);
            if (data != null)
            {
                return Json(data);
            }

            return Json(0);
        }

        [HttpPost]
        public async Task<JsonResult> SaveTestPaper(TestPaperEntity testpaperEntity)
        {
            testpaperEntity.Transaction = GetTransactionData(testpaperEntity.TestPaperID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            var data = await _testService.TestPaperMaintenance(testpaperEntity);
            if (data != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult RemoveTest(long testID)
        {
            var result = _testService.RemoveTest(testID, SessionContext.Instance.LoginUser.Username,true);
            return Json(result);
        }

        [HttpPost]
        public JsonResult RemoveTestPaper(long testpaperID)
        {
            var result = _testService.RemoveTestPaper(testpaperID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }


        public async Task<ActionResult> StudentAnswerSheetMaintenance(long testID)
        {
            return View(await _testService.GetAllAnswerSheetWithoutContentByTest(testID));
        }

        public async Task<JsonResult> Downloadtestpaper(long paperid)
        {
            string[] array = new string[4];
            try
            {
                var operationResult = await _testService.GetTestPaperByPaperID(paperid);
                if (operationResult != null)
                {
                    string contentType = "";
                    string[] extarray = operationResult.FileName.Split('.');
                    string ext = extarray[extarray.Count()-1];
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
                    string file = operationResult.DocContentText;
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