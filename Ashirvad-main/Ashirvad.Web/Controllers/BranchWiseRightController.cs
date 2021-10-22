using Ashirvad.Data;
using Ashirvad.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class BranchWiseRightController : Controller
    {
        // GET: BranchWiseRight
        public ActionResult Index()
        {
            BranchWiseRightMaintenanceModel branchWiseRight = new BranchWiseRightMaintenanceModel();
            branchWiseRight.BranchWiseRightData = new List<BranchWiseRightEntity>();
            return View(branchWiseRight);
        }
    }
}