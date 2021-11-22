using Ashirvad.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Website.Controllers
{
    public class AboutController : Controller
    {
        WebsiteModel websiteModel = new WebsiteModel();
        // GET: About
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            websiteModel = SessionContext.Instance.websiteModel;
            return View(websiteModel);
        }
    }
}