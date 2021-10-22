using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.Subject;
using Ashirvad.ServiceAPI.Services.Area.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class PageController : Controller
    {
        private readonly ISubjectService _subjectService;
        public ResponseModel res = new ResponseModel();

        public PageController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }
        // GET: Page
        public ActionResult Index()
        {
            PageMaintenanceModel pageEntity = new PageMaintenanceModel();
            pageEntity.PageData = new List<PageEntity>();
            return View(pageEntity);
        }

        public async Task<ActionResult> PageMaintenance(long branchID)
        {
            long pageID = branchID;
            PageMaintenanceModel branch = new PageMaintenanceModel();
            //if (subID > 0)
            //{
            //    var result = await ISubjectService.GetSubjectByIDAsync(subID);
            //    branch.SubjectInfo = result;
            //}

            //var branchData = await _subjectService.GetAllSubjects(SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            //branch.PageData = branchData;

            return View("Index", branch);
        }

        public async Task<JsonResult> PageData()
        {
            var pageData = await _subjectService.GetAllSubjects();
            return Json(pageData);
        }
    }
}