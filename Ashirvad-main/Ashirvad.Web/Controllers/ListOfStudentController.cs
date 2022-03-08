using Ashirvad.Data;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class ListOfStudentController : BaseController
    {
        private readonly IChartService _chartService = null;

        public ListOfStudentController(IChartService chartService)
        {
            _chartService = chartService;
        }

        // GET: ListOfStudent
        public ActionResult Index(long StandardID, long batchID)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetStudentContent(long stdID, long branchID, long batchID)
        {
            if (branchID == 0)
                branchID = SessionContext.Instance.LoginUser.BranchInfo.BranchID;
            var standardData = await _chartService.GetStudentContent(stdID,branchID, batchID,SessionContext.Instance.LoginUser.FinancialYear);
            return View("~/Views/ListOfStudent/FilteredData.cshtml", standardData);
        }
    }
}