using Ashirvad.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Website.Controllers
{
    public class TeacherDetailController : Controller
    {
        WebsiteModel websiteModel = new WebsiteModel();
        // GET: TeacherDetail
        public ActionResult TeacherDetail()
        {
            websiteModel = SessionContext.Instance.websiteModel;
            return View(websiteModel);
        }
    }
}