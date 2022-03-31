using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ashirvad.Data;
using Ashirvad.Data.Model;

namespace Ashirvad.Web.Controllers
{
    public class ViewCompetitionlistController : BaseController
    {
        // GET: ViewCompetitionlist
        public ActionResult Index()
        {
            List<TestEntity> testEntity = new List<TestEntity>();
            //TestMaintenanceModel branch = new TestMaintenanceModel();
            //branch.TestInfo = new TestEntity();
            //branch.TestInfo.test = new TestPaperEntity();
            //branch.TestData = new List<TestEntity>();
            return View(testEntity);
        }
    }
}