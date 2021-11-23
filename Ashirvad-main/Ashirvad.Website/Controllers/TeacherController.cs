using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Faculty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Website.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly IFacultyService _facultyService;
        WebsiteModel websiteModel = new WebsiteModel();

        public TeacherController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }
        // GET: Teacher
        public async Task<ActionResult> Teacher(long branchID, long courseID, long classID, long SubjectID)
        {
            websiteModel = SessionContext.Instance.websiteModel;
            websiteModel.facultyEntities = await _facultyService.GetAllFacultyWebsite(branchID, courseID, classID, SubjectID);
            return View(websiteModel);
        }
    }
}