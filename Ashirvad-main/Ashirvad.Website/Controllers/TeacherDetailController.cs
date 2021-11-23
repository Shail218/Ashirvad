using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Faculty;
using Ashirvad.ServiceAPI.Services.Area.Faculty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Website.Controllers
{
    public class TeacherDetailController : BaseController
    {
        private readonly IFacultyService _FacultyService = null;
        WebsiteModel websiteModel = new WebsiteModel();

        public TeacherDetailController(IFacultyService FacultyService)
        {
            _FacultyService = FacultyService;
        }
        public async Task<ActionResult> TeacherDetail(long facultyID)
        {
            websiteModel = SessionContext.Instance.websiteModel;
            websiteModel.facultyEntities = await _FacultyService.GetFacultyDetail(facultyID);
            return View(websiteModel);
        }
    }
}