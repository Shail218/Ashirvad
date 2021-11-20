using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.Repo.Services.Area.Branch;
using Ashirvad.ServiceAPI.ServiceAPI.Area;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Course;
using Ashirvad.ServiceAPI.Services.Area;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Website.Controllers
{

    public class BaseController : Controller
    {
        ResponseModel responsedata = new ResponseModel();
        
        public BranchCourse _branchcourseService = new BranchCourse();
        public BranchClass _BranchClassService = new BranchClass();
        public BranchSubject _BranchSubjectService = new BranchSubject();
        WebsiteModel websiteModel = new WebsiteModel();
        
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionContext.Instance.websiteModel == null)
            {
                websiteModel = GetHeaderData().Result;
                SessionContext.Instance.websiteModel = websiteModel;
            }
            

        }

        public async Task<WebsiteModel> GetHeaderData()
        {
            WebsiteModel websiteModel1 = new WebsiteModel();
            var courseData = await _branchcourseService.GetAllSelectedCourses(0);
            var classdata= await _BranchClassService.GetAllSelectedClasses(0,0);
            var subjectData= await _BranchSubjectService.GetAllSelectedSubjects(0,0,0);
            websiteModel1.branchClassData = classdata;
            websiteModel1.branchCoursesData = courseData;
            websiteModel1.branchSubjectData = subjectData;
            return websiteModel1;
        }

    }
}