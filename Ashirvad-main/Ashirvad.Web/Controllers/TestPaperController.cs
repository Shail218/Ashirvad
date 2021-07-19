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

        public async Task<ActionResult> TestPaperMaintenance(long testID)
        {
            TestMaintenanceModel branch = new TestMaintenanceModel();
            if (testID > 0)
            {
                var test = await _testService.GetTestByTestID(testID);
                branch.TestInfo = test.Data;

                var testpaper = await _testService.GetTestPaperByPaperID(testID);
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
            var data = await _testService.TestMaintenance(testEntity);
            if (data != null)
            {
                TestPaperEntity testpaperEntity = new TestPaperEntity();
                //testpaperEntity = testEntity.test;
                testpaperEntity.TestID = data.TestID;
                JsonResult jsonResult= SaveTestPaper(testpaperEntity).Result;
                return Json(data.TestID);
            }

            return Json(0);
        }

        [HttpPost]
        public async Task<JsonResult> SaveTestPaper(TestPaperEntity testpaperEntity)
        {
            testpaperEntity.Transaction = GetTransactionData(testpaperEntity.TestID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
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
            var result = _testService.RemoveTest(testID, SessionContext.Instance.LoginUser.Username);
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

    }
}