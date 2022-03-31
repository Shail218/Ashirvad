using Ashirvad.Data;
using Ashirvad.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class StudentRankController : BaseController
    {
        // GET: StudentRank
        public ActionResult Index()
        {
            TestMaintenanceModel branch = new TestMaintenanceModel();
            branch.TestInfo = new TestEntity();
            branch.TestInfo.test = new TestPaperEntity();
            branch.TestData = new List<TestEntity>();
            return View(branch);
        }
    }
}