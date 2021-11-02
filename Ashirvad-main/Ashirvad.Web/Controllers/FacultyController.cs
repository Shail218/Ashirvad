using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ashirvad.Data;
using Ashirvad.Data.Model;

namespace Ashirvad.Web.Controllers
{
    public class FacultyController : BaseController
    {
        // GET: Faculty
        public ActionResult Index()
        {
            //FacultyEntity faculty = new FacultyEntity();
            FacultyMaintenanceModel faculty = new FacultyMaintenanceModel();
            List<FacultyEntity> facultylist = new List<FacultyEntity>();
            return View(faculty);
        }
    }
}