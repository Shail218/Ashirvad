using Ashirvad.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Website.Controllers
{
    public class TeacherController : Controller
    {
        WebsiteModel websiteModel = new WebsiteModel();
        // GET: Teacher
        public ActionResult Teacher()
        {
            websiteModel = SessionContext.Instance.websiteModel;
            return View(websiteModel);
        }
    }
}