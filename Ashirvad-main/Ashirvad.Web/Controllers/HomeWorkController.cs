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

    }
}