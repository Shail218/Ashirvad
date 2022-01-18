using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Branch;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Course;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Faculty;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Gallery;
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
        private readonly IFacultyService _facultyService;
        private readonly IGalleryService _galleryService;

        public WebsiteHomeController(IBranchCourseService branchCourseService, IBranchService branchService,ICourseService courseService,IFacultyService facultyService,IGalleryService galleryService)
        {
            _branchCourseService = branchCourseService;
            _branchService = branchService;
            _courseService = courseService;
            _facultyService = facultyService;
            _galleryService = galleryService;
        }
        public async Task<ActionResult> Index()
        {
            WebsiteModel websiteModel = new WebsiteModel();
            websiteModel = SessionContext.Instance.websiteModel;
            var BranchData = await _branchService.GetAllBranchWithoutImage();
            var course = await _courseService.GetAllCourse();
            var faculty = await _facultyService.GetAllFaculty();
            var gallery = await _galleryService.GetAllGallery(1,0);
            websiteModel.branchEntities = BranchData.Data;
            websiteModel.courseEntities = course.Data;
            websiteModel.facultyEntities = faculty;
            websiteModel.galleryEntities = gallery;
            return View(websiteModel);
        }
    }

}