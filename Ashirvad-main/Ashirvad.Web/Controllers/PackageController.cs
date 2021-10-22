using Ashirvad.Data;
using Ashirvad.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class PackageController : Controller
    {
        // GET: Package
        public ActionResult Index()
        {
            PackageMaintenanceModel packageMaintenance = new PackageMaintenanceModel();
            packageMaintenance.PackageData = new List<PackageEntity>();
            return View(packageMaintenance);
        }
    }
}