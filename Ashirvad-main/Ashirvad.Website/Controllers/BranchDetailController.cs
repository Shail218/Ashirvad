using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.AboutUs;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Branch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Website.Controllers
{
    public class BranchDetailController : Controller
    {

        private readonly IBranchCourseService _branchcourseService;
        private readonly IBranchService _branchService;
        private readonly IAboutUsService _aboutUsService;

        public BranchDetailController(IBranchCourseService branchCourseService, IBranchService branchService, IAboutUsService aboutUsService)
        {
            _branchcourseService = branchCourseService;
            _branchService = branchService;
            _aboutUsService = aboutUsService;
        }
        // GET: BranchDetail
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> BranchDetail(long BranchID)
        {
            WebsiteModel websiteModel = new WebsiteModel();
            websiteModel = SessionContext.Instance.websiteModel;
            var BranchData = await _branchService.GetBranchByBranchID(BranchID);
            var aboutusData = await _aboutUsService.GetAllAboutUsWithoutContent(BranchID);
            websiteModel.branchEntity = BranchData.Data;
            websiteModel.aboutUs = aboutusData.Data;
            return View(websiteModel);
        }
    }
}