using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Branch;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Course;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Website.Controllers
{
    public class WebsiteHomeController : BaseController
    {

        private readonly ILibraryService _libraryService;
        private readonly IBranchCourseService _branchCourseService;
        private readonly ICourseService _courseService;
        private readonly IBranchService _branchService;

        public WebsiteHomeController(IBranchCourseService branchCourseService, IBranchService branchService,ICourseService courseService)
        {
            _branchCourseService = branchCourseService;
            _branchService = branchService;
            _courseService = courseService;
        }
        public async Task<ActionResult> Index()
        {
            WebsiteModel websiteModel = new WebsiteModel();
            websiteModel = SessionContext.Instance.websiteModel;
            var BranchData = await _branchService.GetAllBranchWithoutImage();
            var course = await _courseService.GetAllCourse();
            websiteModel.branchEntities = BranchData.Data;
            websiteModel.courseEntities = course.Data;
            return View(websiteModel);
        }
    }

}