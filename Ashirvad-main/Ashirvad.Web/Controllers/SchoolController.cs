using Ashirvad.Common;
using Ashirvad.Data;
using Ashirvad.Data.Model;
using Ashirvad.ServiceAPI.ServiceAPI.Area.School;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ashirvad.Web.Controllers
{
    public class SchoolController : BaseController
    {
        private readonly ISchoolService _schoolService;
        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        // GET: School
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> SchoolMaintenance(long branchID)
        {
            long schoolID = branchID;
            SchoolMaintainanceModel branch = new SchoolMaintainanceModel();
            if (schoolID > 0)
            {
                var result = await _schoolService.GetSchoolsByID(schoolID);
                branch.SchoolInfo = result;
            }

            var branchData = await _schoolService.GetAllSchools(SessionContext.Instance.LoginUser.UserType == Enums.UserType.SuperAdmin ? 0 : SessionContext.Instance.LoginUser.BranchInfo.BranchID);
            branch.SchoolData = branchData;

            return View("Index", branch);
        }

        public async Task<ActionResult> EditSchool(long schoolID, long branchID)
        {
            SchoolMaintainanceModel branch = new SchoolMaintainanceModel();
            if (schoolID > 0)
            {
                var result = await _schoolService.GetSchoolsByID(schoolID);
                branch.SchoolInfo = result;
            }

            if (branchID > 0)
            {
                var result = await _schoolService.GetAllSchools(branchID);
                branch.SchoolData = result;
            }

            var branchData = await _schoolService.GetAllSchools();
            branch.SchoolData = branchData;

            return View("Index", branch);
        }

        [HttpPost]
        public async Task<JsonResult> SaveSchool(SchoolEntity branch)
        {
            branch.Transaction = GetTransactionData(branch.SchoolID > 0 ? Common.Enums.TransactionType.Update : Common.Enums.TransactionType.Insert);
            branch.RowStatus = new RowStatusEntity()
            {
                RowStatusId = (int)Enums.RowStatus.Active
            };
            var data = await _schoolService.SchoolMaintenance(branch);
            if (data != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult RemoveSchool(long branchID)
        {
            var result = _schoolService.RemoveSchool(branchID, SessionContext.Instance.LoginUser.Username);
            return Json(result);
        }

        //public async Task<JsonResult> SchoolData()
        //{
        //    var branchData = await _schoolService.GetAllSchools();
        //    return Json(branchData);
        //}

        public async Task<JsonResult> SchoolData(long branchID)
        {
            var branchData = await _schoolService.GetAllSchools(branchID);
            return Json(branchData);
        }

    }
}