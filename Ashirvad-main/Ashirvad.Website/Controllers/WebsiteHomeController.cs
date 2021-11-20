using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Website.Controllers
{
    public class WebsiteHomeController : BaseController
    {
      
        private readonly ILibraryService _libraryService;
        private readonly IBranchCourseService _branchcourseService;
        WebsiteModel websiteModel = new WebsiteModel();
        public WebsiteHomeController(IBranchCourseService branchCourseService)
        {
            _branchcourseService = branchCourseService;
        }
        public ActionResult Index()
        {
            websiteModel = SessionContext.Instance.websiteModel; 
            return View(websiteModel);
        }
    }
}