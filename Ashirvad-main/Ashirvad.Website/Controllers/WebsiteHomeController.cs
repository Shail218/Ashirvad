using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Website.Controllers
{
    public class WebsiteHomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}