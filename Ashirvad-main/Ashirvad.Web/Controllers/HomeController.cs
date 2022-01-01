using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class HomeController :BaseController
    {
        // GET: Home
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult ADashboard()
        {
            return View();
        }
    }
}