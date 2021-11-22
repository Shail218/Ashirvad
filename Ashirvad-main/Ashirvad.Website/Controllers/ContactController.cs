using Ashirvad.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Website.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        WebsiteModel websiteModel = new WebsiteModel();
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            websiteModel = SessionContext.Instance.websiteModel;
            return View(websiteModel);
        }
    }
}