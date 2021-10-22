using Ashirvad.Data;
using Ashirvad.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class PackageRightController : Controller
    {
        // GET: PackageRight
        public ActionResult Index()
        {
            PackageRightMaintenanceModel packageRightMaintenance = new PackageRightMaintenanceModel();
            packageRightMaintenance.PackageRightsData = new List<PackageRightEntity>();
            return View(packageRightMaintenance);
        }
    }
}