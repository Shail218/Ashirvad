using Ashirvad.Data.Model;
using Ashirvad.Repo.DataAcceessAPI.Area.DashboardChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class HomeController :BaseController
    {
        private readonly IDashboardChartAPI _chartService;

        public HomeController(IDashboardChartAPI chartService)
        {
            _chartService = chartService;
        }
        // GET: Home
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult ADashboard()
        {
            return View();
        }

        public async Task<JsonResult> GetAllBranchChart()
        {
            try
            {
                CommonChartModel model = new CommonChartModel();
                var result = await _chartService.AllBranchWithCount(SessionContext.Instance.LoginUser.FinancialYear);
                model.branchlist = result;
                foreach(var item in model.branchlist)
                {
                    foreach(var item2 in item.branchstandardlist)
                    {

                        model.branchstandardlist.Add(item2);
                    }
                    
                }
                return Json(model);
            }
            catch(Exception e)
            {
                throw;
            }
        }

        public async Task<JsonResult> AllBranchStandardWithCountByBranch()
        {
            try
            {
                CommonChartModel model = new CommonChartModel();
                var result = await _chartService.AllBranchStandardWithCountByBranch(SessionContext.Instance.LoginUser.BranchInfo.BranchID,SessionContext.Instance.LoginUser.FinancialYear);
                model.branchstandardlist = result;
                return Json(model);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}